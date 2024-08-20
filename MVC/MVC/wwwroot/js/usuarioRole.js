// Usamos jQuery.noConflict() para evitar conflictos con otras versiones de jQuery
var $j = jQuery.noConflict();

$j(document).ready(function () {
    loadUsuariosRoles();
    loadRolesOptions();

    $j('#createUsuarioRoleForm').on('submit', function (event) {
        event.preventDefault();
        createUsuarioRole();
    });

    $j('#editUsuarioRoleForm').on('submit', function (event) {
        event.preventDefault();
        checkEmailChangeAndUpdate();
    });

    $j('#filterUsuarioRoleForm').on('submit', function (event) {
        event.preventDefault();
        filterUsuariosRoles();
    });

    $j('#resetFiltersButton').on('click', function () {
        resetFilters();
    });

    // Recargar la página al cerrar los modales (excepto el modal de filtrado)
    $j('#createUsuarioRoleModal, #editUsuarioRoleModal').on('hidden.bs.modal', function () {
        location.reload();
    });

    // No recargar la página al cerrar el modal de filtrado
    $j('#filterUsuarioRoleModal').on('hidden.bs.modal', function () {
        // Aquí no hacemos nada para evitar la recarga
    });

    // Recargar la tabla al cerrar los SweetAlerts
    $j(document).on('click', '.swal2-confirm, .swal2-cancel', function () {
        loadUsuariosRoles();
    });
});

function sanitizeId(id) {
    return id.replace(/[^a-zA-Z0-9]/g, '_');
}

function loadUsuariosRoles() {
    $j.ajax({
        url: 'https://localhost:7280/api/UsuarioRole/GetAllUsuariosConRoles/',
        method: 'GET',
        success: function (data) {
            renderUsuariosRoles(data);
        },
        error: function (xhr, status, error) {
            console.error('Error loading UsuarioRoles:', error);
        }
    });
}

function renderUsuariosRoles(data) {
    var tbody = $j('#usuarios-roles-table tbody');
    tbody.empty(); // Limpiar el contenido de la tabla antes de llenarla

    $j.each(data, function (i, usuarioRole) {
        var sanitizedId = sanitizeId(usuarioRole.correoElectronico);
        var estadoClass = usuarioRole.estado ? "estado-activo" : "estado-inactivo";
        tbody.append('<tr id="' + sanitizedId + '">' +
            '<td>' + usuarioRole.correoElectronico + '</td>' +
            '<td>' + usuarioRole.nombre + '</td>' +
            '<td class="fecha-registro">' + usuarioRole.fechaRegistro.split("T")[0] + '</td>' +
            '<td>' + usuarioRole.telefono + '</td>' +
            // '<td>' + usuarioRole.tipoUsuario + '</td>' + // Oculto
            '<td class="' + estadoClass + '">' + (usuarioRole.estado ? "Activo" : "Inactivo") + '</td>' +
            '<td>' + (usuarioRole.haPagado ? "Sí" : "No") + '</td>' +
            // '<td>' + (usuarioRole.correoVerificado ? "Sí" : "No") + '</td>' + // Oculto
            // '<td>' + (usuarioRole.telefonoVerificado ? "Sí" : "No") + '</td>' + // Oculto
            '<td><a href="#" class="rol-link" data-rol-id="' + usuarioRole.rolId + '">' + getRoleName(usuarioRole.rolId) + '</a></td>' +
            '<td>' +
            '<button class="btn btn-primary" onclick="editUsuarioRole(\'' + usuarioRole.correoElectronico + '\')"><i class="fas fa-pencil-alt"></i></button>' +
            '<button class="btn btn-danger" onclick="deleteUsuarioRole(\'' + usuarioRole.correoElectronico + '\')"><i class="fas fa-trash-alt"></i></button>' +
            '</td>' +
            '</tr>');
    });

    // Asignar el evento click para mostrar la descripción del rol
    $j('.rol-link').click(function (event) {
        event.preventDefault();
        var rolId = $j(this).data('rol-id');
        showRoleDescription(rolId);
    });
}


function getRoleName(roleId) {
    switch (roleId) {
        case 1:
            return "Administrador";
        case 2:
            return "Entrenador";
        case 3:
            return "Recepcionista";
        case 4:
            return "Usuario";
        default:
            return "N/A";
    }
}

function showRoleDescription(roleId) {
    var roleDescriptions = {
        1: "Administrador: Tiene acceso a todos los módulos y funcionalidades del sistema.",
        2: "Entrenador: Puede crear y gestionar rutinas, asistir a los clientes en sus entrenamientos.",
        3: "Recepcionista: Puede agendar citas y gestionar la atención al cliente.",
        4: "Usuario: Tiene acceso a su perfil, rutinas y puede registrar sus entrenamientos."
    };

    var description = roleDescriptions[roleId] || "Descripción no disponible.";
    Swal.fire("Descripción del Rol", description, "info");
}

function loadRolesOptions() {
    var roles = [
        { id: 1, nombre: "Administrador" },
        { id: 2, nombre: "Entrenador" },
        { id: 3, nombre: "Recepcionista" },
        { id: 4, nombre: "Usuario" }
    ];

    var createRolSelect = $j('#createRolId');
    var editRolSelect = $j('#editRolId');
    var filterRolSelect = $j('#filterRolId');
    createRolSelect.empty();
    editRolSelect.empty();
    filterRolSelect.empty();

    filterRolSelect.append('<option value="">Todos</option>');

    $j.each(roles, function (i, role) {
        var option = '<option value="' + role.id + '">' + role.nombre + '</option>';
        createRolSelect.append(option);
        editRolSelect.append(option);
        filterRolSelect.append(option);
    });
}

function createUsuarioRole() {
    var usuarioRole = {
        correoElectronico: $j('#createCorreoElectronico').val(),
        nombre: $j('#createNombre').val(),
        contrasena: $j('#createContrasena').val(),
        fechaRegistro: $j('#createFechaRegistro').val(),
        telefono: $j('#createTelefono').val(),
        tipoUsuario: $j('#createTipoUsuario').val(),
        estado: $j('#createEstado').is(':checked'),
        haPagado: $j('#createHaPagado').is(':checked'),
        correoVerificado: $j('#createCorreoVerificado').is(':checked'),
        telefonoVerificado: $j('#createTelefonoVerificado').is(':checked'),
        rolId: parseInt($j('#createRolId').val())
    };

    $j.ajax({
        url: 'https://localhost:7280/api/UsuarioRole/CreateUsuarioConRol/',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(usuarioRole),
        success: function () {
            $j('#createUsuarioRoleModal').modal('hide');
            Swal.fire("Usuario creado", "El usuario ha sido creado exitosamente", "success")
                .then(() => {
                    loadUsuariosRoles(); // Refrescar la tabla
                });
        },
        error: function (xhr, status, error) {
            console.error('Error creating UsuarioRole:', xhr.responseText);
            Swal.fire("Error", "Hubo un error al crear el usuario: " + xhr.responseText, "error")
                .then(() => {
                    loadUsuariosRoles(); // Refrescar la tabla
                });
        }
    });
}

function editUsuarioRole(correoElectronico) {
    $j.ajax({
        url: 'https://localhost:7280/api/UsuarioRole/GetUsuarioRoleByEmail/' + correoElectronico,
        method: 'GET',
        success: function (data) {
            $j('#editUsuarioRoleForm #editId').val(data.correoElectronico); // Almacenar el correo original en un campo oculto
            $j('#editUsuarioRoleForm #editCorreoElectronico').val(data.correoElectronico);
            $j('#editUsuarioRoleForm #editNombre').val(data.nombre);
            $j('#editUsuarioRoleForm #editFechaRegistro').val(data.fechaRegistro.split('T')[0]);
            $j('#editUsuarioRoleForm #editTelefono').val(data.telefono);
            $j('#editUsuarioRoleForm #editTipoUsuario').val(data.tipoUsuario);
            $j('#editUsuarioRoleForm #editEstado').prop('checked', data.estado);
            $j('#editUsuarioRoleForm #editHaPagado').prop('checked', data.haPagado);
            $j('#editUsuarioRoleForm #editCorreoVerificado').prop('checked', data.correoVerificado);
            $j('#editUsuarioRoleForm #editTelefonoVerificado').prop('checked', data.telefonoVerificado);
            $j('#editUsuarioRoleForm #editRolId').val(data.rolId);
            $j('#editUsuarioRoleModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.error('Error fetching UsuarioRole:', error);
            Swal.fire("Error", "Hubo un error al cargar el usuario", "error");
        }
    });
}

function checkEmailChangeAndUpdate() {
    var originalEmail = $j('#editId').val(); // El correo original
    var newEmail = $j('#editCorreoElectronico').val();

    if (originalEmail !== newEmail) {
        Swal.fire({
            title: "¿Estás seguro?",
            text: "El identificador se va a modificar. ¿Quieres continuar?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Sí, cambiarlo!"
        }).then((result) => {
            if (result.isConfirmed) {
                // Eliminar la fila antigua si el correo se ha cambiado
                $j("#" + sanitizeId(originalEmail)).remove();
                updateUsuarioRole(originalEmail);
            }
        });
    } else {
        updateUsuarioRole(originalEmail);
    }
}

function updateUsuarioRole(originalEmail) {
    var usuarioRole = {
        originalCorreoElectronico: originalEmail, // Agregar el correo original
        correoElectronico: $j('#editCorreoElectronico').val(),
        nombre: $j('#editNombre').val(),
        fechaRegistro: $j('#editFechaRegistro').val(),
        telefono: $j('#editTelefono').val(),
        tipoUsuario: $j('#editTipoUsuario').val(),
        estado: $j('#editEstado').is(':checked'),
        haPagado: $j('#editHaPagado').is(':checked'),
        correoVerificado: $j('#editCorreoVerificado').is(':checked'),
        telefonoVerificado: $j('#editTelefonoVerificado').is(':checked'),
        rolId: parseInt($j('#editRolId').val())
    };

    $j.ajax({
        url: 'https://localhost:7280/api/UsuarioRole/UpdateUsuarioRole/',
        method: 'PUT',
        contentType: 'application/json',
        data: JSON.stringify(usuarioRole),
        success: function () {
            $j('#editUsuarioRoleModal').modal('hide');
            Swal.fire("Usuario actualizado", "El usuario ha sido actualizado exitosamente", "success")
                .then(() => {
                    loadUsuariosRoles(); // Refrescar la tabla
                });
        },
        error: function (xhr, status, error) {
            console.error('Error updating UsuarioRole:', xhr.responseText);
            if (xhr.responseText.includes("FK_ClasesGrupales_Instructor")) {
                Swal.fire("Error", "No se puede actualizar el usuario porque está referenciado en ClasesGrupales. Por favor, actualice o elimine las referencias antes de proceder.", "error");
            } else {
                Swal.fire("Error", "Hubo un error al actualizar el usuario: " + xhr.responseText, "error");
            }
        }
    });
}

function deleteUsuarioRole(correoElectronico) {
    Swal.fire({
        title: "¿Estás seguro?",
        text: "Una vez eliminado, no podrás recuperar este usuario",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, eliminarlo!"
    }).then((result) => {
        if (result.isConfirmed) {
            $j.ajax({
                url: 'https://localhost:7280/api/UsuarioRole/DeleteUsuarioRole/' + correoElectronico,
                method: 'DELETE',
                success: function () {
                    Swal.fire("Usuario eliminado", "El usuario ha sido eliminado exitosamente", "success")
                        .then(() => {
                            loadUsuariosRoles(); // Refrescar la tabla
                        });
                },
                error: function (xhr, status, error) {
                    console.error('Error deleting UsuarioRole:', error);
                    Swal.fire("Error", "Hubo un error al eliminar el usuario", "error");
                }
            });
        }
    });
}

function filterUsuariosRoles() {
    var filters = {
        correoElectronico: $j('#filterCorreoElectronico').val(),
        nombre: $j('#filterNombre').val(),
        fechaRegistro: $j('#filterFechaRegistro').val(),
        telefono: $j('#filterTelefono').val(),
        tipoUsuario: $j('#filterTipoUsuario').val(),
        estado: $j('#filterEstado').val(),
        haPagado: $j('#filterHaPagado').val(),
        correoVerificado: $j('#filterCorreoVerificado').val(),
        telefonoVerificado: $j('#filterTelefonoVerificado').val(),
        rolId: $j('#filterRolId').val()
    };

    $j.ajax({
        url: 'https://localhost:7280/api/UsuarioRole/GetAllUsuariosConRoles',
        method: 'GET',
        success: function (data) {
            var filteredData = data.filter(function (item) {
                return (!filters.correoElectronico || item.correoElectronico.includes(filters.correoElectronico)) &&
                    (!filters.nombre || item.nombre.includes(filters.nombre)) &&
                    (!filters.fechaRegistro || item.fechaRegistro.split("T")[0] === filters.fechaRegistro) &&
                    (!filters.telefono || item.telefono.includes(filters.telefono)) &&
                    (!filters.tipoUsuario || item.tipoUsuario.includes(filters.tipoUsuario)) &&
                    (!filters.estado || item.estado.toString() === filters.estado) &&
                    (!filters.haPagado || item.haPagado.toString() === filters.haPagado) &&
                    (!filters.correoVerificado || item.correoVerificado.toString() === filters.correoVerificado) &&
                    (!filters.telefonoVerificado || item.telefonoVerificado.toString() === filters.telefonoVerificado) &&
                    (!filters.rolId || item.rolId.toString() === filters.rolId);
            });
            renderUsuariosRoles(filteredData);
        },
        error: function (xhr, status, error) {
            console.error('Error filtering UsuarioRoles:', error);
        }
    });
}

function resetFilters() {
    $j('#filterUsuarioRoleForm')[0].reset();
    loadUsuariosRoles();
}
