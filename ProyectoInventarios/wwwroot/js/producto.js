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
            "url": "/Admin/Producto/ObtenerTodos",
        },
        "columns": [
            { "data": "numeroSerie", "title": "Número de Serie", "width": "15%" },
            { "data": "descripcion", "title": "Descripción", "width": "25%" },
            { "data": "categoria.nombre", "title": "Categoría", "width": "15%" },
            { "data": "marca.nombre", "title": "Marca", "width": "15%" },
            {
                "data": "precio",
                "title": "Precio",
                "className": "text-end",
                "render": function (data) {
                    return `$${data.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,')}`;
                },
                "width": "10%"
            },
            {
                "data": "estado",
                "title": "Estado",
                "render": function (data) {
                    return data
                        ? `<span class="badge badge-success">Activo</span>`
                        : `<span class="badge badge-error">Inactivo</span>`;
                },
                "width": "10%"
            },
            {
                "data": "id",
                "title": "Acciones",
                "render": function (data) {
                    return `
                        <div class="flex justify-center space-x-2">
                            <a href="/Admin/Producto/Upssert/${data}" 
                               class="btn btn-info btn-sm flex items-center space-x-1">
                                <i class="fa-solid fa-pen"></i>
                                
                            </a>
                            <a onclick=Delete("/Admin/Producto/Delete/${data}") 
                            class="btn btn-error btn-sm flex items-center space-x-1">
                            <i class="bi bi-trash3"></i>
                            </a>

                            <button onclick="AbrirModal(${data})" 
                            class="btn btn-primary btn-sm flex items-center space-x-1">
                            <i class="fa-solid fa-plus"></i>
                            </button>
                        </div>
                    `;
                },
                "width": "20%"
            }
        ],
        "createdRow": function (row, data, dataIndex) {
            if (dataIndex % 2 === 0) {
                $(row).addClass('bg-gray-50'); // Filas pares
            } else {
                $(row).addClass('bg-gray-100'); // Filas impares
            }
        }
    });
}

function Delete(url) {

    swal({
        title: "Esta seguro de eliminar la Producto",
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


function AbrirModal(productoId) {
    console.log(`Producto ID recibido: ${productoId}`);
    document.getElementById('productoId').value = productoId; // Asigna el ID al campo oculto
    document.getElementById('cantidad').value = ''; // Limpia el campo de cantidad
    document.getElementById('modalInventario').classList.add('modal-open'); // Abre el modal
}

function CerrarModal() {
    document.getElementById('modalInventario').classList.remove('modal-open'); // Cierra el modal
}




function GuardarCantidad() {
    const productoId = document.getElementById("productoId").value;
    const cantidad = document.getElementById("cantidad").value;

    fetch('/Admin/Producto/AgregarCantidad', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            ProductoId: parseInt(productoId),
            Cantidad: parseInt(cantidad),
        }),
    })
        .then(response => {
            if (!response.ok) {
                // Intentar procesar como JSON si es posible
                return response.json().catch(() => {
                    throw new Error('Error en la respuesta del servidor.');
                });
            }
            return response.json();
        })
        .then((data) => {
            alert(data.mensaje);
            location.reload();
        })
        .catch((error) => {
            console.error('Error:', error);
            alert(error.message || 'Error al procesar la solicitud.');
        });


}


