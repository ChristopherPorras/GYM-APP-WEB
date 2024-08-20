// SweetAlert CSS
document.write('<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css">');
// SweetAlert JS
document.write('<script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"><\/script>');

var shoppingCart = (function () {

    var cart = [];

    // Constructor
    function Item(name, price, count) {
        this.name = name;
        this.price = price;
        this.count = count;
    }

    // Guardar en el carrito
    function saveCart() {
        sessionStorage.setItem('shoppingCart', JSON.stringify(cart));
    }

    var obj = {};

    // Add to cart
    obj.addItemToCart = function (name, price, count) {
        var itemFound = false;
        for (var item in cart) {
            if (cart[item].name === name) {
                cart[item].count = count;
                itemFound = true;
                break;
            }
        }
        if (!itemFound) {
            var item = new Item(name, price, count);
            cart.push(item);
        }
        saveCart();
    };

    // Borrar producto del carro
    obj.clearCart = function () {
        cart = [];
        saveCart();
    };

    // Cantidad del carrito
    obj.totalCount = function () {
        var totalCount = 0;
        for (var item in cart) {
            totalCount += cart[item].count;
        }
        return totalCount;
    };

    // Precio en el carrito
    obj.totalCart = function () {
        var totalCart = 0;
        for (var item in cart) {
            totalCart += cart[item].price * cart[item].count;
        }
        return Number(totalCart.toFixed(2));
    };

    // Lista carrito
    obj.listCart = function () {
        var cartCopy = [];
        for (var i in cart) {
            var item = cart[i];
            var itemCopy = Object.assign({}, item);
            itemCopy.total = Number(item.price * item.count).toFixed(2);
            cartCopy.push(itemCopy);
        }
        return cartCopy;
    };

    return obj;
})();

$(document).ready(function () {

    $('.add-to-cart').on('change', function () {
        if ($(this).is(':checked')) {

            $('.add-to-cart').not(this).prop('checked', false);

            var name = $(this).data('name');
            var price = Number($(this).data('price'));

            shoppingCart.clearCart();
            shoppingCart.addItemToCart(name, price, 1);
            displayCart();
        } else {
            shoppingCart.clearCart();
            displayCart();
        }
    });

    displayCart();

    var discountApplied = false;

    $('#apply-discount').click(function () {
        if (discountApplied) {
            Swal.fire({
                icon: 'warning',
                title: 'Código ya utilizado',
                text: 'Este código ya ha sido utilizado.'
            });
            return;
        }

        var discountCode = $('#discount-code').val().trim();

        // Definicion de los descuentos
        var validCodes = {
            "DISCOUNT10": 10,
            "DISCOUNT20": 20,
            "DescuentosDeLaDesarrolladora": 30
        };

        if (validCodes[discountCode]) {
            var discountPercentage = validCodes[discountCode];
            var total = shoppingCart.totalCart();
            var discountedTotal = total - (total * (discountPercentage / 100));
            $('.total-cart').text(discountedTotal.toFixed(2));

            Swal.fire({
                icon: 'success',
                title: 'Descuento aplicado',
                text: 'Su descuento del ' + discountPercentage + '% fue aplicado con éxito!'
            });

            discountApplied = true;
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Código inválido',
                text: 'Código de descuento inválido.'
            });
        }
    });

    $('#realizar-compra').click(function () {
        const cartItems = shoppingCart.listCart();
        const totalAmount = parseFloat($('.total-cart').text()); // Usa el total ajustado en lugar del original.

        // Validación de campos
        const cardName = $('#card-name').val().trim();
        const cardNumber = $('#card-number').val().trim();
        const cardExpiry = $('#card-expiry').val().trim();
        const cardCvv = $('#card-cvv').val().trim();

        if (!cardName || !cardNumber || !cardExpiry || !cardCvv) {
            Swal.fire({
                icon: 'warning',
                title: 'Campos incompletos',
                text: 'Por favor, complete todos los campos de la tarjeta antes de proceder con el pago.'
            });
            return;
        }

        // Validación de 16 dígitos para el número de tarjeta
        if (cardNumber.length !== 16 || isNaN(cardNumber)) {
            Swal.fire({
                icon: 'error',
                title: 'Número de tarjeta inválido',
                text: 'El número de tarjeta debe tener exactamente 16 dígitos.'
            });
            return;
        }

        if (cartItems.length === 0) {
            Swal.fire({
                icon: 'warning',
                title: 'Carrito vacío',
                text: 'Tu carrito está vacío.'
            });
            return;
        }

        // Obtener el correo del localStorage
        const userEmail = localStorage.getItem('userEmail');
        console.log('User email:', userEmail); // Mensaje de depuración
        if (!userEmail) {
            Swal.fire({
                icon: 'error',
                title: 'Correo electrónico faltante',
                text: 'El campo CorreoElectronico es requerido. Por favor, asegúrate de que el correo esté almacenado en localStorage.'
            });
            return;
        }

        const pago = {
            CorreoElectronico: userEmail,
            FechaPago: new Date().toISOString(),
            Monto: totalAmount, // Aquí usamos el total con el descuento aplicado
            MetodoPago: 'Tarjeta', // O 'Paypal' dependiendo de lo que elija el usuario
            EstadoPago: 'Completado',
            TransactionId: '1234567890' // Genera o pasa un identificador de transacción único
        };

        $.ajax({
            type: "POST",
            url: "/api/Pagos",
            data: JSON.stringify(pago),
            contentType: "application/json",
            success: function (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'Compra realizada',
                    text: 'Compra realizada con éxito. ¡Gracias por su compra!'
                }).then(() => {
                    // Redirigir a la página de inicio
                    window.location.href = '/Home/Index';
                });

                shoppingCart.clearCart();
                displayCart();
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error al procesar el pago',
                    text: 'Error al procesar el pago: ' + xhr.responseText
                });
            }
        });
    });

    // Añadir '/' automáticamente en la fecha de expiración
    $('#card-expiry').on('input', function () {
        var input = $(this).val().replace(/\D/g, '').substring(0, 4); // Solo permite números y limita a 4 caracteres
        var month = input.substring(0, 2);
        var year = input.substring(2, 4);
        if (input.length >= 3) {
            $(this).val(`${month}/${year}`);
        } else {
            $(this).val(month);
        }
    });

});

function displayCart() {
    var cartArray = shoppingCart.listCart();
    console.log(cartArray);
    var output = "";

    for (var i in cartArray) {
        output += "<tr>"
            + "<td>" + cartArray[i].name + "</td>"
            + "<td>$" + cartArray[i].price + "</td>"
            + "<td>" + cartArray[i].count + "</td>"
            + "<td>$" + cartArray[i].total + "</td>"
            + "</tr>";
    }

    $('.show-cart').html(output);
    $('.total-cart').html(shoppingCart.totalCart());
    $('.total-count').html(shoppingCart.totalCount());
}
