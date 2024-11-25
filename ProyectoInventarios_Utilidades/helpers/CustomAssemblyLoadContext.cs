using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Utilidades.helpers
{
    public class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        private IntPtr _nativeLibraryHandle;

        public CustomAssemblyLoadContext() : base(isCollectible: true) { }

        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            _nativeLibraryHandle = LoadNativeLibrary(absolutePath);
            return _nativeLibraryHandle;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return _nativeLibraryHandle != IntPtr.Zero
                ? _nativeLibraryHandle
                : base.LoadUnmanagedDll(unmanagedDllName);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null; // No implementación necesaria para esta clase.
        }

        private IntPtr LoadNativeLibrary(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return LoadLibrary(path);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return dlopen(path, RTLD_LAZY);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return dlopen(path, RTLD_LAZY);

            throw new PlatformNotSupportedException("Unsupported platform");
        }

        private static void FreeNativeLibrary(IntPtr handle)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FreeLibrary(handle);
            }
            else
            {
                dlclose(handle);
            }
        }

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private const int RTLD_LAZY = 1;

        [DllImport("libdl", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlopen(string fileName, int flags);

        [DllImport("libdl", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern int dlclose(IntPtr handle);
    }
}

