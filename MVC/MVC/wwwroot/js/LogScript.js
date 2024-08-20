$(document).ready(function () {
    console.log("Documento listo");

    var userEmail = localStorage.getItem('userEmail');
    console.log("User email:", userEmail);

    if (userEmail) {
        console.log("Usuario autenticado, mostrando botón de logout");
        $('#loginRegisterOption').hide();
        $('#logoutOption').show();
    } else {
        console.log("Usuario no autenticado, mostrando botón de login/registro");
        $('#loginRegisterOption').show();
        $('#logoutOption').hide();
    }

    var emailToVerify = '';  // Declarar una variable para almacenar el correo a verificar

    // Manejo de registro de usuario
    $('#createUserForm').submit(function (event) {
        event.preventDefault();
        var password = $('#contrasena').val();
        var confirmPassword = $('#confirmarContrasena').val();
        var strength = getPasswordStrength(password);

        if (password !== confirmPassword) {
            Swal.fire({
                title: 'Error',
                text: 'Las contraseñas no coinciden. Inténtalo nuevamente.',
                icon: 'error'
            });
            return;
        }

        if (strength < 75) {
            Swal.fire({
                title: 'Contraseña débil',
                text: 'La contraseña es demasiado débil. Por favor, elige una más fuerte.',
                icon: 'warning'
            });
            return;
        }

        var email = $('#correo').val();
        checkEmailExists(email).then(emailExists => {
            if (emailExists) {
                Swal.fire({
                    title: 'Correo ya registrado',
                    text: 'El correo electrónico ya está registrado. Por favor, inicia sesión.',
                    icon: 'info'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#container').removeClass("right-panel-active");
                    }
                });
            } else {
                emailToVerify = email; // Guardar el correo en la variable emailToVerify
                registerUser();
            }
        });
    });

    // Función para verificar si el correo ya está registrado
    function checkEmailExists(email) {
        return new Promise((resolve, reject) => {
            $.ajax({
                type: 'POST',
                url: 'https://localhost:7280/api/User/CheckEmailExists',
                data: JSON.stringify({ emailRecipient: email }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    resolve(data.exists);
                },
                error: function (xhr) {
                    console.error('Error checking email existence:', xhr);
                    Swal.fire({
                        title: 'Error',
                        text: xhr.responseText || 'Hubo un problema verificando el correo. Inténtalo nuevamente.',
                        icon: 'error'
                    });
                    reject(xhr);
                }
            });
        });
    }

    // Función para registrar un nuevo usuario
    function registerUser() {
        var userData = {
            nombre: $('#nombre').val(),
            telefono: $('#telefono').val(),
            correoElectronico: $('#correo').val(),
            contrasena: $('#contrasena').val(),
            tipoUsuario: 'Cliente',
            fechaRegistro: new Date().toISOString(),
            correoVerificado: false,
            telefonoVerificado: false,
            estado: true,
            haPagado: false
        };

        $.ajax({
            type: 'POST',
            url: 'https://localhost:7280/api/User/CreateUser',
            data: JSON.stringify(userData),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                Swal.fire({
                    title: 'Registro exitoso',
                    text: 'Se ha enviado un código de verificación a tu correo electrónico.',
                    icon: 'success'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $('#modalBackground').css('display', 'block');
                        $('#verificationModal').css('display', 'block');
                    }
                });
            },
            error: function (xhr) {
                console.error('Error en la solicitud:', xhr);
                Swal.fire({
                    title: 'Error',
                    text: xhr.responseText || 'Hubo un problema en el registro. Inténtalo nuevamente.',
                    icon: 'error'
                });
            }
        });
    }
    $(document).ready(function () {
            updateNavBar();
    });
  
   
    //Manejo Inicio Sesión
    $('#iniciarForm').submit(function (event) {
        event.preventDefault();
        console.log("Formulario de inicio de sesión enviado");

        var loginData = {
            email: $('#correois').val(),
            password: $('#contrasenais').val()
        };

        console.log("Datos de inicio de sesión:", loginData);

        $.ajax({
            type: 'POST',
            url: 'https://localhost:7280/api/User/Login',
            data: JSON.stringify(loginData),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                console.log("Respuesta de la API:", response);

                if (response.success) {
                    // Almacenar el correo electrónico en localStorage
                    localStorage.setItem('userEmail', response.userAutenticado.correoElectronico);

                    // Actualizar la UI para reflejar que el usuario está autenticado
                    $('#loginRegisterOption').hide();
                    $('#logoutOption').show();
                    console.log("Cambio realizado: Usuario autenticado");

                    // Redireccionar según el rol del usuario
                    var getUserRoleUrl = 'https://localhost:7280/api/User/GetUserRol?' +
                        $.param({ email: loginData.email });

                    $.ajax({
                        type: 'GET',
                        url: getUserRoleUrl,
                        dataType: 'json',
                        success: function (roleResponse) {
                            console.log('Respuesta del rol:', roleResponse);
                            var rol = roleResponse;
                            // Asegúrate de almacenar el rol como una cadena
                            localStorage.setItem('userRole', rol.toString());
                            console.log('Valor de rol:', rol);
                            switch (rol) {
                                case 1:
                                    window.location.href = '/Home/SesionAdmin';
                                    updateNavBar();
                                    break;
                                case 2:
                                    window.location.href = '/Home/SesionEntre';
                                    break;
                                case 3:
                                    window.location.href = '/Home/SesionRecep';
                                    break;
                                case 4:
                                    window.location.href = '/Home/SesionUsuario';
                                    break;
                                default:
                                    window.location.href = '/Home/Index';
                            }
                        },
                        error: function (xhr) {
                            console.error('Error al obtener el rol del usuario:', xhr);
                            Swal.fire({
                                title: 'Error',
                                text: xhr.responseText || 'No se pudo determinar el rol del usuario.',
                                icon: 'error'
                            });
                        }
                    });

                } else if (response.requireVerification) {
                    // Guardar el email en la variable cuando se requiera verificación
                    emailToVerify = loginData.email;

                    Swal.fire({
                        title: 'Correo no verificado',
                        text: response.message,
                        icon: 'warning'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $('#modalBackground').css('display', 'block');
                            $('#verificationModal').css('display', 'block');
                        }
                    });
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: response.message,
                        icon: 'error'
                    });
                }
            },
            error: function (xhr) {
                console.log("Error en la solicitud AJAX", xhr);

                if (xhr.status === 401) {
                    var responseJSON = xhr.responseJSON;

                    if (responseJSON && responseJSON.requireVerification) {
                        // Guardar el email en la variable cuando se requiera verificación
                        emailToVerify = loginData.email;

                        Swal.fire({
                            title: 'Correo no verificado',
                            text: responseJSON.message,
                            icon: 'warning'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                $('#modalBackground').css('display', 'block');
                                $('#verificationModal').css('display', 'block');
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Error',
                            text: responseJSON ? responseJSON.message : 'Correo electrónico o contraseña incorrectos. Inténtalo nuevamente.',
                            icon: 'error'
                        });
                    }
                } else {
                    Swal.fire({
                        title: 'Error',
                        text: 'Hubo un problema con la solicitud. Inténtalo nuevamente.',
                        icon: 'error'
                    });
                }
            }
        });
    });

    function updateNavBar() {
        console.log("Actualizando el navbar...");
        console.log('updateNavBar called');
        var userRole = localStorage.getItem('userRole');

        // Si userRole es 'undefined' o 'null', se mantiene como null
        if (userRole === null) {
            userRole = "null";  // Esto es redundante porque userRole ya es null, pero está aquí para claridad
        }


        $('.op ul').empty(); // Limpia los elementos del menú actual

        // Opciones comunes para todos los usuarios
        var menuItems = `

    `;

        // Opciones específicas según el rol del usuario
        switch (userRole) {
            case '1': // Administrador
                menuItems += `
                <li><a href="/Home/SesionAdmin">Home</a></li>
                            <li><a href="/Rol/UsuarioRole">Roles</a></li>
                            <li><a href="/Home/ConfigCitas">Citas</a></li>
                            <li><a href="/Home/Equipos">Maquinas</a></li>
                            <li><a href="/Home/Ejercicios">Ejercicios</a></li>
                                    <li><a href="/Home/Entrenador">Apartar clase</a></li>
                                    <li><a href="/Home/Rutinas">Rutinas</a></li>
                                      <li><a href="/Home/Progreso">Progreso del entrenamiento</a></li>
                                    <li><a href="/Home/PayPage">Pagos</a></li>
                            <li><a href="/Home/Team">Sobre Nosotros</a></li>
        <li><a href="/Home/Contactenos">Contáctenos</a></li>
            `;
                break;
            case '2': // Entre
                menuItems += `
                   <li><a href="/Home/SesionEntre">Home</a></li>
                             <li><a href="/Home/ConfigCitas">Citas</a></li>
                             <li><a href="/Home/Equipos">Maquinas</a></li>
                            <li><a href="/Home/Ejercicios">Ejercicios</a></li>
                           <li><a href="/Home/Entrenador">Apartar clase</a></li>
                             <li><a href="/Home/Rutinas">Rutinas</a></li>
                            <li><a href="/Home/Team">Sobre Nosotros</a></li>
        <li><a href="/Home/Contactenos">Contáctenos</a></li>
            `;
                break;
            case '3': // Recepcionista
                menuItems += `
                <li><a href="/Home/SesionEntre">Home</a></li>
                            <li><a href="/Home/ConfigCitas">Citas</a></li>
                            <li><a href="/Home/Equipos">Maquinas</a></li>
                              <li><a href="/Home/PayPage">Pagos</a></li>
                              <li><a href="/Home/Team">Sobre Nosotros</a></li>
                           <li><a href="/Home/Contactenos">Contáctenos</a></li>
            `;
                break;
            case '4': // Usuario
                menuItems += `
                   <li><a href="/Home/SesionUsuario">Home</a></li>
                            <li><a href="/Home/ConfigCitas">Citas</a></li>
                             <li><a href="/Home/Entrenador">Apartar clase</a></li>
                              <li><a href="/Home/Progreso">Progreso del entrenamiento</a></li>
                            <li><a href="/Home/PayPage">Pagos</a></li>
                             <li><a href="/Home/Team">Sobre nosotros</a></li>
        <li><a href="/Home/Contactenos">Contáctenos</a></li>
            `;
                break;
            case 'null': 
                menuItems += `
                   <li><a href="/Home/Index">Home</a></li>
                    <li><a href="/Home/Team">Sobre nosotros</a></li>
                    <li><a href="/Home/Contactenos">Contáctenos</a></li>
            `;
                break;

            default:
                menuItems += `
                   <li><a href="/Home/Index">Home</a></li>
                  <li><a href="/Home/Team">Sobre Nosotros</a></li>
        <li><a href="/Home/Contactenos">Contáctenos</a></li>
            `;
                break;
        }

        $('.op ul').append(menuItems);
    }



    // Manejo de verificación de código
    $('#verifyCodeButton').click(function () {
        var enteredCode = $('.code-input').map(function () {
            return this.value;
        }).get().join('');

        if (emailToVerify) {
            alert('Correo: ' + emailToVerify + ' Code: ' + enteredCode);

            $.ajax({
                type: 'POST',
                url: 'https://localhost:7280/api/User/VerifyCode',
                data: JSON.stringify({ email: emailToVerify, code: enteredCode }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (data) {
                    if (data.isValid) {
                        Swal.fire({
                            title: 'Verificación exitosa',
                            text: `${data.userName}, te has registrado con éxito a tu mejor experiencia Fitness.`,
                            icon: 'success'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = data.redirectUrl;
                            }
                        });
                    } else {
                        Swal.fire({
                            title: 'Código incorrecto',
                            text: data.message || 'El código ingresado no es válido. Inténtalo nuevamente.',
                            icon: 'error'
                        });
                    }
                },
                error: function (xhr) {
                    console.error('Error en la solicitud:', xhr);
                    Swal.fire({
                        title: 'Error',
                        text: xhr.responseText || 'Hubo un problema en la verificación. Inténtalo nuevamente.',
                        icon: 'error'
                    });
                }
            });
        } else {
            console.error("No se pudo obtener el campo de correo electrónico.");
        }
    });

    // Manejo de cierre de sesión
    $('#logoutButton').click(function (event) {
        event.preventDefault();

        $.ajax({
            type: 'POST',
            url: 'https://localhost:7280/api/User/Logout',
            success: function (response) {
                // Limpiar localStorage
                localStorage.removeItem('userEmail');
                localStorage.removeItem('userRole');

                // Actualizar la UI para reflejar que el usuario ha cerrado sesión
                $('#loginRegisterOption').show();
                $('#logoutOption').hide();

                // Redirigir al usuario a la página de inicio de sesión o a la página principal
                window.location.href = '/Home/Index';
                console.log("Redirigiendo a Home/Index...");
            },
            error: function (xhr) {
                Swal.fire({
                    title: 'Error',
                    text: xhr.responseText || 'Error al cerrar sesión. Inténtalo nuevamente.',
                    icon: 'error'
                });
            }
        });
    });

    // Cambiar entre los formularios
    $('#signUp').click(function () {
        $('#container').addClass("right-panel-active");
    });

    $('#signIn').click(function () {
        $('#container').removeClass("right-panel-active");
    });

    // Mostrar el nombre del usuario
    var urlParams = new URLSearchParams(window.location.search);
    var userName = urlParams.get('userName');
    if (userName) {
        $('#userWelcomeMessage').text('Te damos la bienvenida, ' + userName);
    }

    // Validar la fuerza de la contraseña
    $('#contrasena').on('input', function () {
        var password = $(this).val();
        var strength = getPasswordStrength(password);
        var strengthBar = $('#strengthBar div');

        strengthBar.css('width', strength + '%');

        if (strength <= 33) {
            strengthBar.css('background-color', 'red');
        } else if (strength <= 66) {
            strengthBar.css('background-color', 'yellow');
        } else {
            strengthBar.css('background-color', 'green');
        }
    });

    // Función para calcular la fuerza de la contraseña
    function getPasswordStrength(password) {
        var strength = 0;
        if (password.length >= 6) strength += 25;
        if (/[A-Z]/.test(password)) strength += 25;
        if (/[0-9]/.test(password)) strength += 25;
        if (/[\W]/.test(password)) strength += 25;
        return strength;
    }

    // Focus automático en el siguiente campo de verificación
    $('.code-input').on('input', function () {
        if (this.value.length === this.maxLength) {
            $(this).next('.code-input').focus();
        }
        this.value = this.value.toUpperCase();
    });
});



