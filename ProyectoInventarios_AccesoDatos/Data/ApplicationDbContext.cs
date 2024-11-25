using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoInventarios_Modelos.ViewsModels;
using System.Reflection;

namespace ProyectoInventarios_AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mbodega> Bodega { get; set; }
        public DbSet<MCategoria> Categorias { get; set; }
        public DbSet<Mmarca> Marca { get; set; }
        public DbSet<MProducto> Produto { get; set; }
        public DbSet<MCarrito> Carrito { get; set; }
        public DbSet<MStockProducto> StockProducto { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MCarrito>()
        .HasOne(c => c.Usuario)
        .WithMany() // Si IdentityUser tiene una colección de MCarrito, ajusta aquí
        .HasForeignKey(c => c.UsuarioId)
        .OnDelete(DeleteBehavior.Restrict); // Evita la eliminación en cascada
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
