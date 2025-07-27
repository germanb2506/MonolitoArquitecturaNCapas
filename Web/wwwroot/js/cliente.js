// Variables globales
let clientesOriginales = [];
let modalCliente;
let modalDetalle;

// Inicialización cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function() {
    inicializarModales();
    cargarClientes();
    cargarFiltros();
});

// Inicializar modales de Bootstrap
function inicializarModales() {
    modalCliente = new bootstrap.Modal(document.getElementById('clienteModal'));
    modalDetalle = new bootstrap.Modal(document.getElementById('detalleModal'));
}

// Cargar clientes desde el servidor
async function cargarClientes() {
    try {
        const response = await fetch('/api/Cliente');
        if (response.ok) {
            const clientes = await response.json();
            clientesOriginales = clientes;
            actualizarTabla(clientes);
            cargarFiltros();
        } else {
            mostrarAlerta('Error al cargar los clientes', 'error');
        }
    } catch (error) {
        console.error('Error:', error);
        mostrarAlerta('Error de conexión', 'error');
    }
}

// Actualizar tabla de clientes
function actualizarTabla(clientes) {
    const tbody = document.getElementById('tbodyClientes');
    
    if (!clientes || clientes.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="10" class="text-center text-muted">
                    <i class="fas fa-inbox fa-2x mb-2"></i>
                    <p>No hay clientes registrados</p>
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = clientes.map(cliente => `
        <tr data-cliente-id="${cliente.id}">
            <td>${cliente.id}</td>
            <td>${cliente.nombre}</td>
            <td>
                <span class="badge bg-${getBadgeColor(cliente.tipoCliente)}">
                    ${cliente.tipoCliente || 'Sin tipo'}
                </span>
            </td>
            <td>${cliente.documentoIdentidad || '-'}</td>
            <td>${cliente.correo || '-'}</td>
            <td>${cliente.telefono || '-'}</td>
            <td>${cliente.empresa || '-'}</td>
            <td>${cliente.ciudad || '-'}</td>
            <td>${formatearFecha(cliente.creadoEn)}</td>
            <td>
                <div class="btn-group" role="group">
                    <button type="button" class="btn btn-sm btn-outline-primary" onclick="editarCliente(${cliente.id})" title="Editar">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button type="button" class="btn btn-sm btn-outline-info" onclick="verCliente(${cliente.id})" title="Ver detalles">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button type="button" class="btn btn-sm btn-outline-danger" onclick="eliminarCliente(${cliente.id})" title="Eliminar">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

// Obtener color del badge según tipo de cliente
function getBadgeColor(tipoCliente) {
    switch (tipoCliente) {
        case 'Individual': return 'primary';
        case 'Empresa': return 'success';
        case 'Gobierno': return 'warning';
        default: return 'secondary';
    }
}

// Formatear fecha
function formatearFecha(fecha) {
    if (!fecha) return '-';
    return new Date(fecha).toLocaleDateString('es-ES');
}

// Cargar filtros dinámicos
function cargarFiltros() {
    const ciudades = [...new Set(clientesOriginales.map(c => c.ciudad).filter(c => c))];
    const ddlCiudad = document.getElementById('ddlCiudad');
    
    ddlCiudad.innerHTML = '<option value="">Todas las ciudades</option>';
    ciudades.forEach(ciudad => {
        ddlCiudad.innerHTML += `<option value="${ciudad}">${ciudad}</option>`;
    });
}

// Filtrar clientes
function filtrarClientes() {
    const busqueda = document.getElementById('txtBuscar').value.toLowerCase();
    const tipoCliente = document.getElementById('ddlTipoCliente').value;
    const ciudad = document.getElementById('ddlCiudad').value;

    const clientesFiltrados = clientesOriginales.filter(cliente => {
        const cumpleBusqueda = !busqueda || cliente.nombre.toLowerCase().includes(busqueda);
        const cumpleTipo = !tipoCliente || cliente.tipoCliente === tipoCliente;
        const cumpleCiudad = !ciudad || cliente.ciudad === ciudad;

        return cumpleBusqueda && cumpleTipo && cumpleCiudad;
    });

    actualizarTabla(clientesFiltrados);
}

// Limpiar filtros
function limpiarFiltros() {
    document.getElementById('txtBuscar').value = '';
    document.getElementById('ddlTipoCliente').value = '';
    document.getElementById('ddlCiudad').value = '';
    actualizarTabla(clientesOriginales);
}

// Limpiar formulario
function limpiarFormulario() {
    document.getElementById('formCliente').reset();
    document.getElementById('clienteId').value = '0';
    document.getElementById('clienteModalLabel').innerHTML = '<i class="fas fa-user me-2"></i>Nuevo Cliente';
    
    // Limpiar clases de validación
    const inputs = document.querySelectorAll('#formCliente .form-control, #formCliente .form-select');
    inputs.forEach(input => {
        input.classList.remove('is-valid', 'is-invalid');
    });
}

// Editar cliente
async function editarCliente(id) {
    try {
        const response = await fetch(`/api/Cliente/${id}`);
        if (response.ok) {
            const cliente = await response.json();
            llenarFormulario(cliente);
            document.getElementById('clienteModalLabel').innerHTML = '<i class="fas fa-edit me-2"></i>Editar Cliente';
            modalCliente.show();
        } else {
            mostrarAlerta('Error al cargar el cliente', 'error');
        }
    } catch (error) {
        console.error('Error:', error);
        mostrarAlerta('Error de conexión', 'error');
    }
}

// Llenar formulario con datos del cliente
function llenarFormulario(cliente) {
    document.getElementById('clienteId').value = cliente.id;
    document.getElementById('nombre').value = cliente.nombre || '';
    document.getElementById('tipoCliente').value = cliente.tipoCliente || '';
    document.getElementById('documentoIdentidad').value = cliente.documentoIdentidad || '';
    document.getElementById('correo').value = cliente.correo || '';
    document.getElementById('telefono').value = cliente.telefono || '';
    document.getElementById('empresa').value = cliente.empresa || '';
    document.getElementById('ciudad').value = cliente.ciudad || '';
    document.getElementById('pais').value = cliente.pais || '';
    document.getElementById('direccion').value = cliente.direccion || '';
    document.getElementById('sitioWeb').value = cliente.sitioWeb || '';
    document.getElementById('notasInternas').value = cliente.notasInternas || '';
}

// Ver detalles del cliente
async function verCliente(id) {
    try {
        const response = await fetch(`/api/Cliente/${id}`);
        if (response.ok) {
            const cliente = await response.json();
            mostrarDetalles(cliente);
            modalDetalle.show();
        } else {
            mostrarAlerta('Error al cargar los detalles', 'error');
        }
    } catch (error) {
        console.error('Error:', error);
        mostrarAlerta('Error de conexión', 'error');
    }
}

// Mostrar detalles del cliente
function mostrarDetalles(cliente) {
    const content = document.getElementById('detalleClienteContent');
    content.innerHTML = `
        <div class="row">
            <div class="col-md-6">
                <h6 class="text-primary">Información Personal</h6>
                <table class="table table-borderless">
                    <tr><td><strong>ID:</strong></td><td>${cliente.id}</td></tr>
                    <tr><td><strong>Nombre:</strong></td><td>${cliente.nombre}</td></tr>
                    <tr><td><strong>Tipo:</strong></td><td><span class="badge bg-${getBadgeColor(cliente.tipoCliente)}">${cliente.tipoCliente || 'Sin tipo'}</span></td></tr>
                    <tr><td><strong>Documento:</strong></td><td>${cliente.documentoIdentidad || '-'}</td></tr>
                    <tr><td><strong>Correo:</strong></td><td>${cliente.correo || '-'}</td></tr>
                    <tr><td><strong>Teléfono:</strong></td><td>${cliente.telefono || '-'}</td></tr>
                </table>
            </div>
            <div class="col-md-6">
                <h6 class="text-primary">Información de Empresa</h6>
                <table class="table table-borderless">
                    <tr><td><strong>Empresa:</strong></td><td>${cliente.empresa || '-'}</td></tr>
                    <tr><td><strong>Ciudad:</strong></td><td>${cliente.ciudad || '-'}</td></tr>
                    <tr><td><strong>País:</strong></td><td>${cliente.pais || '-'}</td></tr>
                    <tr><td><strong>Sitio Web:</strong></td><td>${cliente.sitioWeb ? `<a href="${cliente.sitioWeb}" target="_blank">${cliente.sitioWeb}</a>` : '-'}</td></tr>
                    <tr><td><strong>Fecha Creación:</strong></td><td>${formatearFecha(cliente.creadoEn)}</td></tr>
                </table>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <h6 class="text-primary">Dirección</h6>
                <p>${cliente.direccion || 'No especificada'}</p>
            </div>
        </div>
        ${cliente.notasInternas ? `
        <div class="row mt-3">
            <div class="col-12">
                <h6 class="text-primary">Notas Internas</h6>
                <p>${cliente.notasInternas}</p>
            </div>
        </div>
        ` : ''}
    `;
}

// Eliminar cliente
async function eliminarCliente(id) {
    if (!confirm('¿Está seguro de que desea eliminar este cliente?')) {
        return;
    }

    try {
        const response = await fetch(`/api/Cliente/${id}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            mostrarAlerta('Cliente eliminado exitosamente', 'success');
            cargarClientes();
        } else {
            const error = await response.text();
            mostrarAlerta(`Error al eliminar: ${error}`, 'error');
        }
    } catch (error) {
        console.error('Error:', error);
        mostrarAlerta('Error de conexión', 'error');
    }
}

// Manejar envío del formulario
document.getElementById('formCliente').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    if (!validarFormulario()) {
        return;
    }

    const formData = new FormData(this);
    const cliente = Object.fromEntries(formData.entries());
    const id = cliente.Id;
    
    try {
        const url = id === '0' ? '/api/Cliente' : `/api/Cliente/${id}`;
        const method = id === '0' ? 'POST' : 'PUT';

        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(cliente)
        });

        if (response.ok) {
            const mensaje = id === '0' ? 'Cliente creado exitosamente' : 'Cliente actualizado exitosamente';
            mostrarAlerta(mensaje, 'success');
            modalCliente.hide();
            cargarClientes();
        } else {
            const error = await response.text();
            mostrarAlerta(`Error: ${error}`, 'error');
        }
    } catch (error) {
        console.error('Error:', error);
        mostrarAlerta('Error de conexión', 'error');
    }
});

// Validar formulario
function validarFormulario() {
    let esValido = true;
    const nombre = document.getElementById('nombre');
    const correo = document.getElementById('correo');

    // Validar nombre
    if (!nombre.value.trim()) {
        nombre.classList.add('is-invalid');
        esValido = false;
    } else {
        nombre.classList.remove('is-invalid');
        nombre.classList.add('is-valid');
    }

    // Validar correo si se proporciona
    if (correo.value.trim() && !isValidEmail(correo.value)) {
        correo.classList.add('is-invalid');
        esValido = false;
    } else if (correo.value.trim()) {
        correo.classList.remove('is-invalid');
        correo.classList.add('is-valid');
    }

    return esValido;
}

// Validar email
function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Exportar clientes
function exportarClientes() {
    const clientesFiltrados = obtenerClientesFiltrados();
    
    if (clientesFiltrados.length === 0) {
        mostrarAlerta('No hay clientes para exportar', 'warning');
        return;
    }

    // Crear CSV
    const headers = ['ID', 'Nombre', 'Tipo', 'Documento', 'Correo', 'Teléfono', 'Empresa', 'Ciudad', 'País', 'Dirección', 'Sitio Web', 'Fecha Creación'];
    const csvContent = [
        headers.join(','),
        ...clientesFiltrados.map(cliente => [
            cliente.id,
            `"${cliente.nombre}"`,
            cliente.tipoCliente || '',
            cliente.documentoIdentidad || '',
            cliente.correo || '',
            cliente.telefono || '',
            `"${cliente.empresa || ''}"`,
            cliente.ciudad || '',
            cliente.pais || '',
            `"${cliente.direccion || ''}"`,
            cliente.sitioWeb || '',
            formatearFecha(cliente.creadoEn)
        ].join(','))
    ].join('\n');

    // Descargar archivo
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    const url = URL.createObjectURL(blob);
    link.setAttribute('href', url);
    link.setAttribute('download', `clientes_${new Date().toISOString().split('T')[0]}.csv`);
    link.style.visibility = 'hidden';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}

// Obtener clientes filtrados actuales
function obtenerClientesFiltrados() {
    const busqueda = document.getElementById('txtBuscar').value.toLowerCase();
    const tipoCliente = document.getElementById('ddlTipoCliente').value;
    const ciudad = document.getElementById('ddlCiudad').value;

    return clientesOriginales.filter(cliente => {
        const cumpleBusqueda = !busqueda || cliente.nombre.toLowerCase().includes(busqueda);
        const cumpleTipo = !tipoCliente || cliente.tipoCliente === tipoCliente;
        const cumpleCiudad = !ciudad || cliente.ciudad === ciudad;

        return cumpleBusqueda && cumpleTipo && cumpleCiudad;
    });
}

// Mostrar alerta
function mostrarAlerta(mensaje, tipo) {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${tipo === 'error' ? 'danger' : tipo} alert-dismissible fade show position-fixed`;
    alertDiv.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
    alertDiv.innerHTML = `
        ${mensaje}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    document.body.appendChild(alertDiv);
    
    // Auto-remover después de 5 segundos
    setTimeout(() => {
        if (alertDiv.parentNode) {
            alertDiv.remove();
        }
    }, 5000);
} 