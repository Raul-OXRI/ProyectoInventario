let datatable;

//Espera a que el documento Index o un html esté completamente cargado
$(document).ready(function () {
    //llama a la función para cargar la tabla de datos cuando el documento esté listo
    loadDataTable();
});

//función para cargar y configurar la DataTable
function loadDataTable() {
    datatable = $('#tblDatos').DataTable({
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "No hay registros disponibles",
            "info": "Mostrando página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros",
            "infoFiltered": "(filtrado de _MAX_ registros totales)",
            "search": "Buscar:",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        "ajax": {
            "url": "/Admin/Bodega/ObtenerTodos",
        },
        "columns": [
            { "data": "nombre", "width": "25%" },
            { "data": "descripcion", "width": "40%" },
            {
                "data": "estado",
                "render": function (data) {
                    return data ?
                        `<span class="badge badge-success">Activo</span>` :
                        `<span class="badge badge-error">Inactivo</span>`;
                }, "width": "15%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="flex justify-center space-x-2">
                            <a href="/Admin/Bodega/Upssert/${data}" 
                               class="btn btn-info btn-sm">
                               <i class="fa-solid fa-pen"></i>
                            </a>
                            <button onclick=Delete("/Admin/Bodega/Delete/${data}") 
                                    class="btn btn-error btn-sm">
                                    <i class="fa-solid fa-trash"></i>
                            </button>
                            <button onclick="generatePdf(${data})" 
                                    class="btn btn-primary btn-sm">
                                    <i class="fa-solid fa-file-pdf"></i>
                            </button>
                        </div>`;
                }, "width": "20%"
            }
        ]
    });
}



function Delete(url) {

    swal({
        title: "Esta seguro de eliminar la Bodega",
        text: "Este registro no se podra recuperar",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((borrar) => {
        if (borrar) {
            $.ajax({
                type: "POST",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}

function generatePdf(id) {
    // Enviar una solicitud al servidor para generar el PDF
    $.ajax({
        url: `/Admin/Bodega/GenerarReportePdf/${id}`,
        method: 'GET',
        xhrFields: {
            responseType: 'blob' // Esto asegura que se reciba el archivo como un Blob
        },
        success: function (data) {
            // Crear un enlace temporal para descargar el PDF
            const url = window.URL.createObjectURL(data);
            const a = document.createElement('a');
            a.href = url;
            a.download = `Reporte_Bodega_${id}.pdf`;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
        },
        error: function (xhr, status, error) {
            // Manejo de errores
            console.error('Error al generar el PDF:', error);
            toastr.error("Ocurrió un error al generar el PDF. Inténtalo de nuevo.");
        }
    });
}
