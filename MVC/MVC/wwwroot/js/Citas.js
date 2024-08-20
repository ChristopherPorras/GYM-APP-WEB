$(document).ready(function () {
    const userEmail = localStorage.getItem('userEmail');

    if (!userEmail) {
        console.error('Usuario no autenticado. Redirigiendo a la página de inicio de sesión.');
        window.location.href = '/Home/LogPage';
    } else {
        console.log('Usuario autenticado:', userEmail);
        cargarAgenda();
        cargarNombreUsuario(userEmail);
    }

    window.onload = function () {
        cargarAgenda();
    };
});

function cargarAgenda() {
    const userEmail = localStorage.getItem('userEmail');

    Promise.all([
        fetch(`https://localhost:7280/api/Cita/GetDisponibilidad`).then(response => response.json()),
        fetch(`https://localhost:7280/api/Cita/GetCitaAgendada/${encodeURIComponent(userEmail)}`).then(response => response.json())
    ])
        .then(([disponibilidad, citasUsuario]) => {
            const agendaBody = document.getElementById("agendaBody");
            const message = document.getElementById("message");
            agendaBody.innerHTML = '';
            message.innerHTML = '';

            let hasData = false;
            const dias = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"];
            const horarios = ["6 am - 8 am", "10 am - 12 pm", "5 pm - 7 pm", "7 pm - 9 pm"];

            horarios.forEach(horario => {
                let fila = `<tr><td class="class-time">${horario}</td>`;
                dias.forEach(dia => {
                    const fechaHora = truncarFecha(new Date(getFechaHoraPredeterminada(dia, horario)));

                    const citaUsuarioAgendada = citasUsuario.find(cita => truncarFecha(new Date(cita.fechaCita)).getTime() === fechaHora.getTime());

                    if (citaUsuarioAgendada) {
                        fila += `<td class=""><button class="button-unavailable hover-bg" disabled>Cita agendada</button></td>`;
                    } else if (disponibilidad[dia] && disponibilidad[dia][horario]) {
                        fila += `<td class=""><button class="button-available" onclick="agendarCita('${dia}', '${horario}', this)">Disponible</button></td>`;
                        hasData = true;
                    } else {
                        fila += `<td class=""><button class="button-unavailable hover-bg" disabled>No disponible</button></td>`;
                    }
                });
                fila += '</tr>';
                agendaBody.innerHTML += fila;
            });

            if (!hasData) {
                message.innerHTML = 'No hay citas disponibles para agendar en este momento.';
            }
        })
        .catch(error => {
            console.error('Error al cargar la agenda:', error);
            agendaBody.innerHTML = '<tr><td colspan="8">Error al cargar la disponibilidad.</td></tr>';
            message.innerHTML = 'Error al cargar la disponibilidad. Por favor, inténtelo de nuevo más tarde.';
        });
}

function truncarFecha(fecha) {
    return new Date(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes());
}

function getFechaHoraPredeterminada(dia, horario) {
    const fecha = new Date();
    const daysOfWeek = ["Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado"];
    const dayIndex = daysOfWeek.indexOf(dia);
    const currentDayIndex = fecha.getDay();
    const daysToAdd = dayIndex >= currentDayIndex ? dayIndex - currentDayIndex : 7 - (currentDayIndex - dayIndex);
    fecha.setDate(fecha.getDate() + daysToAdd);

    const timeParts = horario.split(' ')[0].split('-');
    const hourStart = parseInt(timeParts[0]);
    const isPM = horario.includes('pm');
    fecha.setHours(isPM ? hourStart + 12 : hourStart, 0, 0, 0);

    return fecha.toISOString().slice(0, 19);
}

function cargarNombreUsuario(email) {
    fetch(`https://localhost:7280/api/User/GetUserNameByEmail?email=${encodeURIComponent(email)}`)
        .then(response => response.json())
        .then(data => {
            document.getElementById("usuarioNombre").value = data.nombre;
        })
        .catch(error => {
            console.error('Error al obtener el nombre del usuario:', error);
        });
}

function agendarCita(dia, horario, boton) {
    abrirModal(dia, horario);

    document.getElementById('agendarCitaForm').onsubmit = function (event) {
        event.preventDefault();
        const cita = {
            Id: 0,
            CorreoElectronico: localStorage.getItem('userEmail'),
            EntrenadorCorreo: document.getElementById('entrenadorCorreo').value,
            FechaCita: document.getElementById('FechaCita').value,
            estado: "programada"
        };

        fetch(`https://localhost:7280/api/Cita/CreateCita`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(cita)
        })
            .then(response => response.json())
            .then(data => {
                Swal.fire({
                    icon: 'success',
                    title: 'Cita agendada con éxito',
                    text: 'Se ha enviado un correo de confirmación.',
                    confirmButtonText: 'Aceptar'
                });

                boton.classList.remove('button-available');
                boton.classList.add('button-unavailable');
                boton.textContent = 'Cita agendada';
                boton.disabled = true;

                document.getElementById("citaModal").style.display = "none";
                document.getElementById('agendarCitaForm').reset();

                setTimeout(() => {
                    cargarAgenda();
                }, 500);
            })
            .catch(error => {
                console.error('Error al agendar cita:', error);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Hubo un problema al agendar la cita. Por favor, inténtelo de nuevo.',
                    confirmButtonText: 'Aceptar'
                });
            });
    };
}

function abrirModal(dia, horario) {
    const diaSeleccionado = document.getElementById("diaSeleccionado");
    const horarioSeleccionado = document.getElementById("horarioSeleccionado");
    const modal = document.getElementById("citaModal");

    diaSeleccionado.value = dia;
    horarioSeleccionado.value = horario;

    const FechaCita = getFechaHoraPredeterminada(dia, horario);
    document.getElementById("fechaHoraDisplay").textContent = formatFechaHora(FechaCita);
    document.getElementById("FechaCita").value = FechaCita;

    cargarEntrenadores();
    modal.style.display = "block";
}

function cargarEntrenadores() {
    fetch(`https://localhost:7280/api/Cita/GetAllEntrenadores`)
        .then(response => response.json())
        .then(entrenadores => {
            const selectEntrenador = document.getElementById("entrenadorCorreo");
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
            const selectEntrenador = document.getElementById("entrenadorCorreo");
            selectEntrenador.innerHTML = '<option value="">Error al cargar entrenadores</option>';
        });
}

function formatFechaHora(fecha) {
    const options = {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    };
    return new Date(fecha).toLocaleDateString('es-ES', options);
}

document.getElementById("cancelarBtnModal").addEventListener("click", function () {
    document.getElementById("citaModal").style.display = "none";
    document.getElementById('agendarCitaForm').reset();
});

document.getElementsByClassName("close-button")[0].addEventListener("click", function () {
    document.getElementById("citaModal").style.display = "none";
    document.getElementById('agendarCitaForm').reset();
});
