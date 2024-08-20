$(document).ready(function () {
    // Manejo del envío del formulario de restablecimiento de contraseña en LogPage
    $('#passwordResetForm').submit(function (event) {
        event.preventDefault();
        var email = $('#email').val();

        $.ajax({
            type: 'POST',
            url: '/Home/SendPasswordResetLink',
            data: JSON.stringify({ email: email }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (response) {
                Swal.fire({
                    title: 'Éxito',
                    text: 'Se ha enviado un enlace de restablecimiento de contraseña a tu correo electrónico. Revisa tu bandeja de entrada.',
                    icon: 'success'
                });
            },
            error: function (xhr) {
                Swal.fire({
                    title: 'Error',
                    text: 'Hubo un problema al enviar el enlace de restablecimiento de contraseña. Inténtalo nuevamente.',
                    icon: 'error'
                });
            }
        });
    });

    // Manejo del envío del formulario en la vista de recuperación de contraseña
    $('#passwordRecoveryForm').submit(function (event) {
        event.preventDefault();
        var email = $('#verificationCode').val();

        if (!email || !validateEmail(email)) {
            Swal.fire({
                title: 'Error',
                text: 'Por favor, ingresa un correo electrónico válido.',
                icon: 'error'
            });
            return;
        }

        $.ajax({
            type: 'POST',
            url: 'https://localhost:7280/api/User/ForgotPassword',
            data: JSON.stringify({ emailRecipient: email }),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                Swal.fire({
                    title: 'Correo enviado',
                    text: 'Se ha enviado un enlace de restablecimiento de contraseña a tu correo electrónico.',
                    icon: 'success'
                });
            },
            error: function (xhr) {
                Swal.fire({
                    title: 'Error',
                    text: xhr.responseText || 'Hubo un problema enviando el correo. Inténtalo nuevamente.',
                    icon: 'error'
                });
            }
        });
    });

    function validateEmail(email) {
        var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }

});
