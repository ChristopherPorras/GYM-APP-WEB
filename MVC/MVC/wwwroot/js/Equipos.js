document.addEventListener('DOMContentLoaded', function () {
    const API = "https://localhost:7280/api/";
    const apiUrlCreate = API + "Equipo/sp_CrearEquipo";
    const apiUrlUpdate = API + "Equipo/sp_ActualizarEquipo";
    const apiUrlDelete = API + "Equipo/sp_EliminarEquipo";
    const apiUrlGetAll = API + "Equipo/sp_ObtenerTodosLosEquipos";
    const apiUrlGetById = API + "Equipo/sp_ObtenerEquipoPorId";

    // Cargar y renderizar la lista de equipos al cargar la página
    loadEquipos();

    function loadEquipos() {
        fetch(apiUrlGetAll)
            .then(response => response.json())
            .then(data => {
                const equiposTableBody = document.getElementById('equiposTableBody');
                equiposTableBody.innerHTML = '';

                data.forEach(equipo => {
                    const tr = document.createElement('tr');
                    tr.innerHTML = `
                        <td>${equipo.nombre}</td>
                        <td>${equipo.descripcion}</td>
                        <td>${equipo.grupoMuscular}</td>
                        <td>${equipo.cantidad}</td>
                        <td>${equipo.disponibilidad ? 'Sí' : 'No'}</td>
                        <td>
                            <button class="btn btn-primary" onclick="editEquipo(${equipo.equipoId})">Modificar</button>
                            <button class="button-unavailable" onclick="deleteEquipo(${equipo.equipoId})">Eliminar</button>
                        </td>
                    `;
                    equiposTableBody.appendChild(tr);
                });
            })
            .catch(error => console.error('Error al cargar los equipos:', error));
    }

    // Función para enviar el formulario de creación de equipo
    document.getElementById('createForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const equipoData = {
            nombre: document.getElementById('createNombre').value.trim(),
            descripcion: document.getElementById('createDescripcion').value.trim(),
            grupoMuscular: document.getElementById('createGrupoMuscular').value.trim(),
            cantidad: parseInt(document.getElementById('createCantidad').value.trim(), 10),
            disponibilidad: document.getElementById('createDisponible').checked
        };

        if (!equipoData.nombre || !equipoData.descripcion || !equipoData.grupoMuscular || isNaN(equipoData.cantidad)) {
            Swal.fire('Error', 'Todos los campos son obligatorios.', 'error');
            return;
        }

        fetch(apiUrlCreate, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(equipoData)
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'El equipo se ha creado correctamente.', 'success');
                    const createModal = bootstrap.Modal.getInstance(document.getElementById('createModal'));
                    createModal.hide();
                    loadEquipos();
                } else {
                    throw new Error('Error al crear el equipo');
                }
            })
            .catch(error => {
                console.error('Error al crear el equipo:', error);
                Swal.fire('Error', 'Hubo un problema al crear el equipo. Inténtalo de nuevo.', 'error');
            });
    });

    // Función para enviar el formulario de edición de equipo
    document.getElementById('editForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const equipoData = {
            equipoId: document.getElementById('editID').value,
            nombre: document.getElementById('editNombre').value.trim(),
            descripcion: document.getElementById('editDescripcion').value.trim(),
            grupoMuscular: document.getElementById('editGrupoMuscular').value.trim(),
            cantidad: parseInt(document.getElementById('editCantidad').value.trim(), 10),
            disponibilidad: document.getElementById('editDisponible').checked
        };

        fetch(`${apiUrlUpdate}/${equipoData.equipoId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(equipoData)
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'El equipo se ha modificado correctamente.', 'success');
                    const editModal = bootstrap.Modal.getInstance(document.getElementById('editModal'));
                    editModal.hide();
                    loadEquipos();
                } else {
                    throw new Error('Error al modificar el equipo');
                }
            })
            .catch(error => {
                console.error('Error al modificar el equipo:', error);
                Swal.fire('Error', 'Hubo un problema al modificar el equipo. Inténtalo de nuevo.', 'error');
            });
    });

    // Función para enviar el formulario de eliminación de equipo
    document.getElementById('deleteForm').addEventListener('submit', function (e) {
        e.preventDefault();
        const equipoId = document.getElementById('deleteID').value;

        fetch(`${apiUrlDelete}/${equipoId}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'El equipo se ha eliminado correctamente.', 'success');
                    const deleteModal = bootstrap.Modal.getInstance(document.getElementById('deleteModal'));
                    deleteModal.hide();
                    loadEquipos();
                } else {
                    throw new Error('Error al eliminar el equipo');
                }
            })
            .catch(error => {
                console.error('Error al eliminar el equipo:', error);
                Swal.fire('Error', 'Hubo un problema al eliminar el equipo. Inténtalo de nuevo.', 'error');
            });
    });

    // Función para obtener y mostrar detalles de un equipo en el formulario de edición
    window.editEquipo = function (equipoId) {
        fetch(`${apiUrlGetById}/${equipoId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('editID').value = data.equipoId;
                document.getElementById('editNombre').value = data.nombre;
                document.getElementById('editDescripcion').value = data.descripcion;
                document.getElementById('editGrupoMuscular').value = data.grupoMuscular;
                document.getElementById('editCantidad').value = data.cantidad;
                document.getElementById('editDisponible').checked = data.disponibilidad;
                const editModal = new bootstrap.Modal(document.getElementById('editModal'));
                editModal.show();
            })
            .catch(error => console.error('Error al obtener los detalles del equipo:', error));
    }

    // Función para configurar el ID del equipo a eliminar
    window.deleteEquipo = function (equipoId) {
        document.getElementById('deleteID').value = equipoId;
        const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
        deleteModal.show();
    }

    // Función para obtener y mostrar detalles de un equipo
    window.viewEquipo = function (equipoId) {
        fetch(`${apiUrlGetById}/${equipoId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('viewID').value = data.equipoId;
                document.getElementById('viewNombre').value = data.nombre;
                document.getElementById('viewDescripcion').value = data.descripcion;
                document.getElementById('viewGrupoMuscular').value = data.grupoMuscular;
                document.getElementById('viewCantidad').value = data.cantidad;
                document.getElementById('viewDisponible').checked = data.disponibilidad;
                const viewModal = new bootstrap.Modal(document.getElementById('viewModal'));
                viewModal.show();
            })
            .catch(error => console.error('Error al obtener los detalles del equipo:', error));
    }
});
