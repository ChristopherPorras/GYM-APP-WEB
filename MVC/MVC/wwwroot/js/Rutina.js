document.addEventListener('DOMContentLoaded', function () {
    const API_BASE = "https://localhost:7280/api/";
    const API_RUTINA_CREATE = API_BASE + "Rutina/CreateRutina";
    const API_EJERCICIOS_GETALL = API_BASE + "Ejercicio/sp_ObtenerTodosLosEjercicios";
    const API_USUARIOS_SEARCH = API_BASE + "User/SearchByName";
    const API_SEND_PDF = API_BASE + "Rutina/SendRutinaPdf";
    const entrenadorCorreo = localStorage.getItem('userEmail');
    let ejerciciosPorDia = {};

    // Variable para almacenar los datos de la rutina para el PDF y el correo
    let rutinaParaPdf = null;

    // Colocar el correo del entrenador en el input
    document.getElementById('entrenadorCorreo').value = entrenadorCorreo;

    // Cargar la lista de ejercicios desde la base de datos
    fetch(API_EJERCICIOS_GETALL)
        .then(response => response.json())
        .then(data => {
            const exerciseSelect = document.getElementById('exerciseSelect');
            if (exerciseSelect) {
                data.forEach(ejercicio => {
                    const option = document.createElement('option');
                    option.value = ejercicio.ejercicioId;
                    option.textContent = ejercicio.nombre;
                    exerciseSelect.appendChild(option);
                });
            }
        })
        .catch(error => console.error('Error al cargar los ejercicios:', error));

    // Buscar cliente por nombre
    document.getElementById('clientSearch').addEventListener('input', function () {
        const query = this.value.trim();
        if (query.length > 2) {
            fetch(API_USUARIOS_SEARCH + "?name=" + encodeURIComponent(query))
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Error al buscar clientes: ' + response.statusText);
                    }
                    return response.json();
                })
                .then(data => {
                    const suggestions = document.getElementById('suggestions');
                    suggestions.innerHTML = '';
                    if (data.length > 0) {
                        suggestions.style.display = 'block';
                        data.forEach(user => {
                            const li = document.createElement('li');
                            li.classList.add('aaa');
                            li.textContent = `${user.nombre} (${user.correoElectronico})`;
                            li.addEventListener('click', function () {
                                document.getElementById('clientSearch').value = user.nombre;
                                document.getElementById('clientEmail').value = user.correoElectronico;
                                suggestions.style.display = 'none';
                            });
                            suggestions.appendChild(li);
                        });
                    } else {
                        suggestions.style.display = 'none';
                    }
                })
                .catch(error => console.error('Error al buscar clientes:', error));
        } else {
            document.getElementById('suggestions').style.display = 'none';
        }
    });

    // Agregar ejercicio a la rutina del día seleccionado
    document.getElementById('addExercise').addEventListener('click', function () {
        const daySelect = document.getElementById('daySelect');
        const selectedDays = Array.from(daySelect.selectedOptions).map(option => option.value);
        const exerciseSelect = document.getElementById('exerciseSelect');

        if (!exerciseSelect) {
            Swal.fire('Error', 'El selector de ejercicios no está disponible.', 'error');
            return;
        }

        const exerciseId = exerciseSelect.value;
        const sets = document.getElementById('sets').value;
        const repeticiones = document.getElementById('repeticiones').value;
        const peso = document.getElementById('peso').value;

        if (!exerciseId || !sets || !repeticiones || !peso || selectedDays.length === 0) {
            Swal.fire('Error', 'Por favor, llena todos los campos antes de agregar un ejercicio.', 'error');
            return;
        }

        selectedDays.forEach(day => {
            if (!ejerciciosPorDia[day]) {
                ejerciciosPorDia[day] = [];
            }
            ejerciciosPorDia[day].push({ exerciseId, sets, repeticiones, peso });
        });

        actualizarVistaEjercicios();
    });

    function actualizarVistaEjercicios() {
        const exerciseList = document.getElementById('exerciseList');
        exerciseList.innerHTML = '';

        Object.keys(ejerciciosPorDia).forEach(day => {
            const dayHeader = document.createElement('h5');
            dayHeader.textContent = day;
            exerciseList.appendChild(dayHeader);

            ejerciciosPorDia[day].forEach((ejercicio, index) => {
                const ejercicioElement = document.createElement('div');
                const exerciseName = document.querySelector(`#exerciseSelect option[value="${ejercicio.exerciseId}"]`).textContent;
                ejercicioElement.textContent = `Ejercicio: ${exerciseName}, Sets: ${ejercicio.sets}, Repeticiones: ${ejercicio.repeticiones}, Peso: ${ejercicio.peso} kg`;
                exerciseList.appendChild(ejercicioElement);
            });
        });
    }

    // Manejar el envío del formulario para crear la rutina
    document.getElementById('rutinaForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const clientEmail = document.getElementById('clientEmail').value;

        if (!clientEmail || Object.keys(ejerciciosPorDia).length === 0) {
            Swal.fire('Error', 'Por favor, asegúrate de que todos los campos obligatorios estén completos.', 'error');
            return;
        }

        rutinaParaPdf = Object.keys(ejerciciosPorDia).map(day => ({
            correoElectronico: clientEmail,
            entrenadorCorreo: entrenadorCorreo,
            medicionId: 1,
            fechaCreacion: new Date(),
            diaSemana: day,
            ejercicios: ejerciciosPorDia[day]
        }));

        Promise.all(rutinaParaPdf.map(rutinaData =>
            fetch(API_RUTINA_CREATE, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(rutinaData)
            })
        ))
            .then(responses => {
                if (responses.every(response => response.ok)) {
                    Swal.fire('Éxito', 'Las rutinas se han guardado correctamente.', 'success');
                    actualizarVistaEjercicios(); // Actualizar vista pero no reiniciar formulario
                } else {
                    throw new Error('Error al guardar algunas rutinas.');
                }
            })
            .catch(error => {
                console.error('Error al guardar las rutinas:', error);
                Swal.fire('Error', 'Hubo un problema al guardar las rutinas. Inténtalo de nuevo.', 'error');
            });
    });

    // Crear PDF y descargarlo manualmente
    document.getElementById('downloadPdf').addEventListener('click', function () {
        if (!rutinaParaPdf) {
            Swal.fire('Error', 'No hay rutinas disponibles para descargar. Guarda una rutina primero.', 'error');
            return;
        }

        crearPdf(document.getElementById('clientEmail').value);
    });

    // Enviar PDF por correo
    document.getElementById('sendPdfByEmail').addEventListener('click', function () {
        if (!rutinaParaPdf) {
            Swal.fire('Error', 'No hay rutinas disponibles para enviar. Guarda una rutina primero.', 'error');
            return;
        }

        crearPdf(document.getElementById('clientEmail').value, true); // Generar el PDF y enviarlo por correo
    });

    function crearPdf(clientEmail, sendByEmail = false) {
        const { jsPDF } = window.jspdf;
        const doc = new jsPDF();
        doc.text("Rutina de Ejercicios", 10, 10);
        doc.text(`Cliente: ${clientEmail}`, 10, 20);
        doc.text(`Entrenador: ${entrenadorCorreo}`, 10, 30);

        let yOffset = 40;
        Object.keys(ejerciciosPorDia).forEach(day => {
            doc.text(`Día: ${day}`, 10, yOffset);
            yOffset += 10;
            ejerciciosPorDia[day].forEach((ejercicio, index) => {
                const exerciseName = document.querySelector(`#exerciseSelect option[value="${ejercicio.exerciseId}"]`).textContent;
                doc.text(`Ejercicio: ${exerciseName}, Sets: ${ejercicio.sets}, Repeticiones: ${ejercicio.repeticiones}, Peso: ${ejercicio.peso} kg`, 10, yOffset);
                yOffset += 10;
            });
        });

        doc.text(`IMC Comentario: ${document.getElementById('imcComentario').textContent}`, 10, yOffset + 10);

        // Obtener el PDF como una cadena base64
        const pdfBase64 = doc.output('datauristring').split(',')[1];

        if (sendByEmail) {
            enviarPdfPorCorreo(clientEmail, pdfBase64);
        } else {
            doc.save(`rutina_${clientEmail}.pdf`);
        }
    }

    function enviarPdfPorCorreo(clientEmail, pdfBase64) {
        const payload = {
            ToEmail: clientEmail,
            Subject: entrenadorCorreo,
            Message: pdfBase64
        };

        console.log('Enviando payload:', payload);

        fetch(API_SEND_PDF, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'El PDF ha sido enviado por correo.', 'success');
                } else {
                    return response.text().then(err => {
                        throw new Error(`Error al enviar el PDF por correo: ${err}`);
                    });
                }
            })
            .catch(error => {
                console.error('Error al enviar el PDF:', error);
                Swal.fire('Error', 'Hubo un problema al enviar el PDF. Inténtalo de nuevo.', 'error');
            });
    }


    // Calcular el IMC del cliente
    document.getElementById('calcularIMC').addEventListener('click', function () {
        const altura = parseFloat(document.getElementById('altura').value);
        const peso = parseFloat(document.getElementById('pesoIMC').value);

        if (!isNaN(altura) && !isNaN(peso) && altura > 0 && peso > 0) {
            const alturaMetros = altura / 100;
            const imc = peso / (alturaMetros * alturaMetros);
            document.getElementById('resultadoIMC').value = imc.toFixed(2);

            let imcComentario = '';
            if (imc < 18.5) {
                imcComentario = 'Bajo peso';
            } else if (imc >= 18.5 && imc < 24.9) {
                imcComentario = 'Peso normal';
            } else if (imc >= 25 && imc < 29.9) {
                imcComentario = 'Sobrepeso';
            } else {
                imcComentario = 'Obesidad';
            }

            document.getElementById('imcComentario').textContent = `Estado según IMC: ${imcComentario}`;
        } else {
            Swal.fire('Error', 'Por favor, ingresa valores válidos para altura y peso.', 'error');
        }
    });
});
