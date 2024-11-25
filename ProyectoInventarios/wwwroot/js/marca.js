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
            "zeroRecords": "No se encontraron registros",
            "info": "Mostrando página _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros disponibles",
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
            "url": "/Admin/Marca/ObtenerTodos",
        },
        "columns": [
            {
                "data": "nombre",
                "width": "20%"
            },
            {
                "data": "descripcion",
                "width": "40%"
            },
            {
                "data": "estado",
                "render": function (data) {
                    return data
                        ? `<span class="badge badge-success">Activo</span>`
                        : `<span class="badge badge-error">Inactivo</span>`;
                },
                "width": "20%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="flex justify-center space-x-2">
                            <a href="/Admin/Marca/Upssert/${data}" 
                               class="btn btn-info btn-sm flex items-center space-x-1">
                                <i class="fa-solid fa-pen"></i>
                                
                            </a>
                            <button onclick=Delete("/Admin/Marca/Delete/${data}") 
                                    class="btn btn-error btn-sm flex items-center space-x-1">
                                <i class="fa-solid fa-trash"></i>
                                
                            </button>
                        </div>
                    `;
                },
                "width": "20%"
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (dataIndex % 2 === 0) {
                $(row).addClass('bg-gray-50'); // Filas pares con fondo gris claro
            } else {
                $(row).addClass('bg-gray-100'); // Filas impares con un tono más oscuro
            }
        }
    });
}

function Delete(url) {

    swal({
        title: "Esta seguro de eliminar la Marca",
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