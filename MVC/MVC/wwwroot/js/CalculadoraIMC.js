document.addEventListener('DOMContentLoaded', function () {
    const API = "https://localhost:7280/api/";
    const apiUrlCreate = API + "MedidasCorporales/InsertarMedidaCorporal";

    // Function to submit the form and create a new BMI measurement
    document.getElementById('medidasForm').addEventListener('submit', function (e) {
        e.preventDefault();

        // Get values from input fields
        const medidaData = {
            altura: parseFloat(document.getElementById('altura').value.trim()), // No multiplicar por 100 si está en cm
            peso: parseFloat(document.getElementById('peso').value.trim()),
            correoElectronico: document.getElementById('correoElectronico').value.trim(),
            EntrenadorCorreo: document.getElementById('EntrenadorCorreo').value.trim(), // Using 'EntrenadorCorreo'
            fechaMedicion: new Date().toISOString().split('T')[0] // YYYY-MM-DD format
        };

        // Validate input
        if (isNaN(medidaData.peso) || isNaN(medidaData.altura) || medidaData.altura === 0 || !medidaData.EntrenadorCorreo) {
            Swal.fire('Error', 'Por favor, ingresa valores válidos.', 'error');
            return;
        }

        // Calculate BMI
        medidaData.imc = medidaData.peso / ((medidaData.altura / 100) * (medidaData.altura / 100)); // Si la altura está en cm

        // Send data to the API
        fetch(apiUrlCreate, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(medidaData)
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'Las medidas se han guardado correctamente.', 'success');
                    document.getElementById('medidasForm').reset();
                } else {
                    return response.json().then(errorData => {
                        // Log the full error response from the server for debugging
                        console.error('Detalles del error:', errorData);
                        Swal.fire('Error', `Error al guardar las medidas: ${JSON.stringify(errorData.errors)}`, 'error');
                        throw new Error('Error al guardar las medidas');
                    });
                }
            })
            .catch(error => {
                // Log the caught error for debugging purposes
                console.error('Error al guardar las medidas corporales:', error);
                Swal.fire('Error', 'Hubo un problema al guardar las medidas. Inténtalo de nuevo.', 'error');
            });
    });
});
