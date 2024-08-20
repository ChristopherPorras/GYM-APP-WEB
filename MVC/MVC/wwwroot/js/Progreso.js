$(document).ready(function () {
    var userEmail = localStorage.getItem('userEmail');

    if (userEmail) {
        cargarProgreso(userEmail);
    } else {
        Swal.fire('Error', 'No se encontró el correo del usuario.', 'error');
    }

    // Función para cargar el progreso del usuario
    function cargarProgreso(email) {
        $.ajax({
            url: `/api/ProgresoUsuario/ObtenerProgreso/${email}`,
            method: 'GET',
            success: function (data) {
                console.log("Datos recibidos del API:", data);  // Verificar lo que devuelve el API
                if (data && data.length > 0) {
                    console.log("Hay datos para mostrar en el gráfico.");

                    // Convertir fechas y ajustar la zona horaria si es necesario
                    var fechas = data.map(x => {
                        var fecha = new Date(x.fechaProgreso);
                        return fecha instanceof Date && !isNaN(fecha) ? fecha.toLocaleDateString() : 'Fecha inválida';
                    });
                    var pesos = data.map(x => x.peso);

                    Highcharts.chart('contenedor', {
                        chart: {
                            type: 'line',
                            backgroundColor: '#212529'
                        },
                        title: {
                            text: 'Progreso de Peso del Usuario',
                            align: 'left',
                            style: {
                                color: 'white' 
                            }
                        },
                        xAxis: {
                            categories: fechas,
                            title: {
                                text: 'Fecha',
                                style: {
                                    color: 'white' 
                                }
                            },
                            labels: {
                                style: {
                                    color: 'white' 
                                }
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Peso (kg)',
                                style: {
                                    color: 'white' 
                                }
                            },
                            labels: {
                                style: {
                                    color: 'white' 
                                }
                            }
                        },
                        tooltip: {
                            style: {
                                color: 'black'
                            },
                            backgroundColor: 'black',
                        },
                        series: [{
                            name: 'Peso',
                            data: pesos,
                            color: 'white'
                        }]
                    });
                } else {
                    console.log("No hay datos, mostrando gráfico de ejemplo.");
                    Highcharts.chart('contenedor', {
                        chart: {
                            type: 'line'
                        },
                        title: {
                            text: 'Progreso de Peso del Usuario',
                            align: 'left'
                        },
                        xAxis: {
                            categories: ['Semana 1', 'Semana 2', 'Semana 3', 'Semana 4'],
                            title: {
                                text: 'Tiempo'
                            }
                        },
                        yAxis: {
                            title: {
                                text: 'Peso (kg)'
                            },
                            min: 0
                        },
                        series: [{
                            name: 'Peso',
                            data: [60, 62, 65, 68] // Datos ficticios para simular progreso
                        }],
                        lang: {
                            noData: "Estamos emocionados por ver tu progreso, " + email + "!"
                        },
                        noData: {
                            style: {
                                fontWeight: 'bold',
                                fontSize: '16px',
                                color: 'white',
                                backgroundColor: '#212529'
                            }
                        }
                    });
                }
            },
            error: function () {
                Swal.fire('Error', 'No se pudo cargar el progreso del usuario.', 'error');
            }
        });
    }

    // Manejar el formulario de crear/actualizar progreso
    $('#form-progreso').on('submit', function (e) {
        e.preventDefault();

        var id = $('#progresoId').val();
        var progreso = {
            CorreoElectronico: userEmail,
            FechaProgreso: $('#fechaProgreso').val(),
            Peso: parseFloat($('#peso').val()),
            MasaMuscular: parseFloat($('#masaMuscular').val()),
            PorcentajeGrasa: parseFloat($('#porcentajeGrasa').val())
        };

        if (id) {
            // Actualizar progreso
            progreso.ID = parseInt(id);
            $.ajax({
                url: '/api/ProgresoUsuario/ActualizarProgreso',
                method: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(progreso),
                success: function () {
                    Swal.fire('Éxito', 'Progreso actualizado correctamente', 'success');
                    cargarProgreso(userEmail);
                    $('#form-progreso')[0].reset();
                },
                error: function () {
                    Swal.fire('Error', 'No se pudo actualizar el progreso.', 'error');
                }
            });
        } else {
            // Crear progreso
            $.ajax({
                url: '/api/ProgresoUsuario/CrearProgreso',
                method: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(progreso),
                success: function () {
                    Swal.fire('Éxito', 'Progreso creado correctamente', 'success');
                    cargarProgreso(userEmail);
                    $('#form-progreso')[0].reset();
                },
                error: function () {
                    Swal.fire('Error', 'No se pudo crear el progreso.', 'error');
                }
            });
        }
    });

    // Manejar la eliminación de progreso
    $('#btnEliminar').on('click', function () {
        var id = $('#progresoId').val();
        if (id) {
            $.ajax({
                url: `/api/ProgresoUsuario/EliminarProgreso/${id}`,
                method: 'DELETE',
                success: function () {
                    Swal.fire('Éxito', 'Progreso eliminado correctamente', 'success');
                    cargarProgreso(userEmail);
                    $('#form-progreso')[0].reset();
                },
                error: function () {
                    Swal.fire('Error', 'No se pudo eliminar el progreso.', 'error');
                }
            });
        } else {
            Swal.fire('Error', 'Debe seleccionar un progreso para eliminar.', 'error');
        }
    });

    // Llenar el formulario con los datos del progreso seleccionado (simulando la selección)
    $(document).on('click', '.btnEditar', function () {
        var id = $(this).data('id');
        var progreso = $(this).data('progreso');
        $('#progresoId').val(id);
        $('#fechaProgreso').val(progreso.fechaProgreso.split('T')[0]);
        $('#peso').val(progreso.peso);
        $('#masaMuscular').val(progreso.masaMuscular);
        $('#porcentajeGrasa').val(progreso.porcentajeGrasa);
    });
});
