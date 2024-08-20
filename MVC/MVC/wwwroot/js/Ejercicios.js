document.addEventListener('DOMContentLoaded', function () {
    const API = "https://localhost:7280/api/";
    const apiUrlCreate = API + "Ejercicio/sp_RegistrarEjercicio";
    const apiUrlUpdate = API + "Ejercicio/sp_ActualizarEjercicio";
    const apiUrlDelete = API + "Ejercicio/sp_EliminarEjercicio";
    const apiUrlGetAll = API + "Ejercicio/sp_ObtenerTodosLosEjercicios";
    const apiUrlGetById = API + "Ejercicio/sp_ObtenerEjercicioPorId";

    // Cargar y renderizar la lista de ejercicios al cargar la página
    loadEjercicios();

    function loadEjercicios() {
        fetch(apiUrlGetAll)
            .then(response => response.json())
            .then(data => {
                const ejerciciosTableBody = document.getElementById('ejerciciosTableBody');
                ejerciciosTableBody.innerHTML = '';


                data.forEach(ejercicio => {
                    const tr = document.createElement('tr');
                    tr.innerHTML = `
                        <td>${ejercicio.nombre}</td>
                        <td>${ejercicio.tipo}</td>
                        <td>${ejercicio.descripcion}</td>
                        <td>
                            <button class="ghost" onclick="editEjercicio(${ejercicio.ejercicioId})">Modificar</button>
                            <button class="ghost" onclick="deleteEjercicio(${ejercicio.ejercicioId})">Eliminar</button>
                    `;
                    ejerciciosTableBody.appendChild(tr);
                });
            })
            .catch(error => console.error('Error al cargar los ejercicios:', error));
    }

    // Función para enviar el formulario de creación de ejercicio
    document.getElementById('createForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const ejercicioData = {
            nombre: document.getElementById('createNombre').value.trim(),
            tipo: document.getElementById('createTipo').value.trim(),
            descripcion: document.getElementById('createDescripcion').value.trim()
        };

        fetch(apiUrlCreate, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(ejercicioData)
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'El ejercicio se ha creado correctamente.', 'success');
                    const createModal = new bootstrap.Modal(document.getElementById('createModal'));
                    createModal.hide();
                    loadEjercicios();
                } else {
                    throw new Error('Error al crear el ejercicio');
                }
            })
            .catch(error => {
                console.error('Error al crear el ejercicio:', error);
                Swal.fire('Error', 'Hubo un problema al crear el ejercicio. Inténtalo de nuevo.', 'error');
            });
    });

    // Función para enviar el formulario de edición de ejercicio
    document.getElementById('editForm').addEventListener('submit', function (e) {
        e.preventDefault();

        const ejercicioData = {
            ejercicioId: document.getElementById('editID').value,
            nombre: document.getElementById('editNombre').value.trim(),
            tipo: document.getElementById('editTipo').value.trim(),
            descripcion: document.getElementById('editDescripcion').value.trim()
        };

        fetch(`${apiUrlUpdate}/${ejercicioData.ejercicioId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(ejercicioData)
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'El ejercicio se ha modificado correctamente.', 'success');
                    const editModal = new bootstrap.Modal(document.getElementById('editModal'));
                    editModal.hide();
                    loadEjercicios();
                } else {
                    throw new Error('Error al modificar el ejercicio');
                }
            })
            .catch(error => {
                console.error('Error al modificar el ejercicio:', error);
                Swal.fire('Error', 'Hubo un problema al modificar el ejercicio. Inténtalo de nuevo.', 'error');
            });
    });

    // Función para enviar el formulario de eliminación de ejercicio
    document.getElementById('deleteForm').addEventListener('submit', function (e) {
        e.preventDefault();
        const ejercicioId = document.getElementById('deleteID').value;

        fetch(`${apiUrlDelete}/${ejercicioId}`, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    Swal.fire('Éxito', 'El ejercicio se ha eliminado correctamente.', 'success');
                    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
                    deleteModal.hide();
                    loadEjercicios();
                } else {
                    throw new Error('Error al eliminar el ejercicio');
                }
            })
            .catch(error => {
                console.error('Error al eliminar el ejercicio:', error);
                Swal.fire('Error', 'Hubo un problema al eliminar el ejercicio. Inténtalo de nuevo.', 'error');
            });
    });

    // Función para obtener y mostrar detalles de un ejercicio en el formulario de edición
    window.editEjercicio = function (ejercicioId) {
        fetch(`${apiUrlGetById}/${ejercicioId}`)
            .then(response => response.json())
            .then(data => {
                document.getElementById('editID').value = data.ejercicioId;
                document.getElementById('editNombre').value = data.nombre;
                document.getElementById('editTipo').value = data.tipo;
                document.getElementById('editDescripcion').value = data.descripcion;
                const editModal = new bootstrap.Modal(document.getElementById('editModal'));
                editModal.show();
            })
            .catch(error => console.error('Error al obtener los detalles del ejercicio:', error));
    }

    // Función para configurar el ID del ejercicio a eliminar
    window.deleteEjercicio = function (ejercicioId) {
        document.getElementById('deleteID').value = ejercicioId;
        const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
        deleteModal.show();
    }
});
