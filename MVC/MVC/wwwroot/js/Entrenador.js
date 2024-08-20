$(document).ready(function () {
    const userEmail = localStorage.getItem('userEmail');

    if (!userEmail) {
        console.error('Usuario no autenticado. Redirigiendo a la página de inicio de sesión.');
        window.location.href = '/Home/LogPage';
    } else {
        // Asignar el correo del usuario al campo
        $('#correoUsuario').val(userEmail);
        console.log('Usuario autenticado:', userEmail);

        // Cargar nombre del usuario y entrenadores
        cargarNombreUsuario(userEmail);
        cargarEntrenadores(); // Cargar los entrenadores al iniciar la página
    }

    var selectedClass = null;

    $('.class-timetable').on('click', 'td.ts-meta', function () {
        if (selectedClass) {
            selectedClass.removeClass('selected-class');
        }
        selectedClass = $(this);
        selectedClass.addClass('selected-class');
    });

    $('#submitRegistrarClase').on('click', function () {
        if (selectedClass) {
            var className = selectedClass.find('h5').text();
            var instructorName = selectedClass.find('span').text();
            var nombre = $('#nombre').val();
            var cupos = $('#cupos').val() || 1;  // Establecer cupos en 1 si está vacío
            var usuarioCorreo = $('#correoUsuario').val();
            var entrenadorCorreo = $('#EntrenadorCorreo').val();

            // Validar si ya está registrado
            validarRegistroClase(usuarioCorreo, className).then(isRegistered => {
                if (isRegistered) {
                    Swal.fire('Error', 'Ya estás registrado en esta clase.', 'error');
                } else {
                    registrarClase(className, instructorName, usuarioCorreo, entrenadorCorreo, nombre, cupos);
                }
            }).catch(error => {
                console.error('Error en la validación del registro:', error);
                Swal.fire('Error', 'Hubo un problema al validar el registro. Inténtalo de nuevo.', 'error');
            });
        } else {
            Swal.fire('Error', 'Por favor, selecciona una clase primero.', 'error');
        }
    });
});

// Función para registrar la clase si no está registrada
function registrarClase(className, instructorName, usuarioCorreo, entrenadorCorreo, nombre, cupos) {
    var fechaActual = new Date();
    var horario = fechaActual.toISOString();
    var descripcion = "Gracias por matricular la clase, lo esperamos";

    var data = {
        Nombre: className,
        InstructorCorreo: instructorName,
        UsuarioCorreo: usuarioCorreo,
        Cupo: cupos,
        Horario: horario,
        Descripcion: descripcion,
        EntrenadorCorreo: entrenadorCorreo
    };

    console.log('Datos enviados:', data);

    $.ajax({
        url: '/api/ClasesGrupales',
        method: 'POST',
        data: JSON.stringify(data),
        contentType: 'application/json',
        success: function (response) {
            Swal.fire('Éxito', 'Clase registrada exitosamente', 'success');
            $('#registrarClaseModal').modal('hide');
            selectedClass.off('click').removeClass('selected-class').addClass('registered-class').css('background-color', 'gray');
            selectedClass = null;
        },
        error: function (xhr, status, error) {
            console.error('Error al registrar la clase:', xhr.responseText);
            Swal.fire('Error', 'Hubo un problema al registrar la clase. Inténtalo de nuevo.', 'error');
        }
    });
}

// Función para validar si el usuario ya está registrado en la clase
function validarRegistroClase(usuarioCorreo, className) {
    return fetch(`https://localhost:7280/api/User/GetUserNameByEmail?email=${encodeURIComponent(usuarioCorreo)}&className=${encodeURIComponent(className)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error HTTP: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            return data.isRegistered;  // Este valor lo retornará la API
        });
}

// Función para cargar el nombre del usuario desde el backend usando el correo
function cargarNombreUsuario(email) {
    fetch(`https://localhost:7280/api/User/GetUserNameByEmail?email=${encodeURIComponent(email)}`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error HTTP: ${response.status}`);
            }
            return response.json();
        })
        .then(userDetails => {
            if (userDetails && userDetails.nombre) {
                $('#nombre').val(userDetails.nombre); // Asignar el nombre del usuario
            } else {
                throw new Error('No se pudo obtener el nombre del usuario.');
            }
        })
        .catch(error => {
            console.error('Error al obtener el nombre del usuario:', error);
            Swal.fire('Error', 'No se pudo cargar el nombre del usuario.', 'error');
        });
}

// Función para cargar los entrenadores desde el backend
function cargarEntrenadores() {
    fetch(`https://localhost:7280/api/Cita/GetAllEntrenadores`)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Error HTTP: ${response.status}`);
            }
            return response.text();  // Leer como texto en lugar de JSON directamente
        })
        .then(text => {
            if (text.trim() === "") {
                throw new Error("Respuesta vacía del servidor");
            }

            let entrenadores;
            try {
                entrenadores = JSON.parse(text);
            } catch (error) {
                throw new Error("Error al parsear JSON: " + error.message);
            }

            const selectEntrenador = document.getElementById("EntrenadorCorreo");
            selectEntrenador.innerHTML = '';

            if (entrenadores.length === 0) {
                const option = document.createElement("option");
                option.value = "";
                option.textContent = "No hay entrenadores disponibles";
                selectEntrenador.appendChild(option);
                return;
            }

            entrenadores.forEach(entrenador => {
                const option = document.createElement("option");
                option.value = entrenador.correoElectronico;
                option.textContent = entrenador.nombre || "Sin nombre";
                selectEntrenador.appendChild(option);
            });
        })
        .catch(error => {
            console.error('Error al cargar entrenadores:', error);
            const selectEntrenador = document.getElementById("EntrenadorCorreo");
            selectEntrenador.innerHTML = '<option value="">Error al cargar entrenadores</option>';
        });
}
