// Variables globales
let clienteActual = null;
let modoEdicion = false;

// Inicialización cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function() {
    inicializarEventos();
    configurarValidaciones();
    cargarTodosLosClientes(); // Cargar datos iniciales
});

// Inicializar eventos
function inicializarEventos() {
    // Evento para guardar cliente
    const btnGuardar = document.getElementById('btnGuardarCliente');
    if (btnGuardar) {
        btnGuardar.addEventListener('click', guardarCliente);
    }
    
    // Eventos para filtros
    const filtroRazonSocial = document.getElementById('filtroRazonSocial');
    if (filtroRazonSocial) {
        filtroRazonSocial.addEventListener('keypress', function(e) {
            if (e.key === 'Enter') buscarPorRazonSocial();
        });
    }
    
    // Botones de filtros
    const btnAplicarFiltros = document.getElementById('btnAplicarFiltros');
    if (btnAplicarFiltros) {
        btnAplicarFiltros.addEventListener('click', aplicarFiltros);
    }
    
    const btnLimpiarFiltros = document.getElementById('btnLimpiarFiltros');
    if (btnLimpiarFiltros) {
        btnLimpiarFiltros.addEventListener('click', limpiarFiltros);
    }
    
    const btnSoloActivos = document.getElementById('btnSoloActivos');
    if (btnSoloActivos) {
        btnSoloActivos.addEventListener('click', cargarClientesActivos);
    }
    
    const btnTodos = document.getElementById('btnTodos');
    if (btnTodos) {
        btnTodos.addEventListener('click', cargarTodosLosClientes);
    }
    
    const btnFiltrarFechas = document.getElementById('btnFiltrarFechas');
    if (btnFiltrarFechas) {
        btnFiltrarFechas.addEventListener('click', filtrarPorFechas);
    }
    
    // Validación en tiempo real del NIT y correo
    const nitInput = document.getElementById('nit');
    if (nitInput) {
        nitInput.addEventListener('blur', validarNitEnTiempoReal);
    }
    
    const correoInput = document.getElementById('correoContacto');
    if (correoInput) {
        correoInput.addEventListener('blur', validarCorreoEnTiempoReal);
    }
}

// Configurar validaciones
function configurarValidaciones() {
    // Validación del formulario
    const form = document.getElementById('formCliente');
    if (form) {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            guardarCliente();
        });
    }
}

// ===== FUNCIONES PRINCIPALES =====

// Abrir modal para nuevo cliente
function ModalNuevoCliente() {
    modoEdicion = false;
    clienteActual = null;
    limpiarFormulario();
    
    const modalTitle = document.getElementById('modalTitle');
    if (modalTitle) modalTitle.textContent = 'Nuevo Cliente';
    
    const clienteId = document.getElementById('clienteId');
    if (clienteId) clienteId.value = '';
    
    // Mostrar modal usando Bootstrap
    const modal = new bootstrap.Modal(document.getElementById('modalCliente'));
    modal.show();
}

// Guardar cliente (crear o actualizar)
async function guardarCliente() {
    if (!validarFormulario()) return;

    const clienteDto = obtenerDatosFormulario();
    const url = modoEdicion ? '/Cliente/Actualizar' : '/Cliente/Crear';

    try {
        mostrarCargando();
        const response = await fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify(clienteDto)
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const resultado = await response.json();
        ocultarCargando();

        if (resultado.isExitoso) {
            mostrarMensaje('success', 'Cliente guardado exitosamente');
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalCliente'));
            if (modal) modal.hide();
            cargarTodosLosClientes();
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al guardar el cliente');
        }
    } catch (error) {
        ocultarCargando();
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al guardar cliente:', error);
    }
}

// Editar cliente
async function editarCliente(id) {
    try {
        mostrarCargando();
        const response = await fetch(`/Cliente/ObtenerPorId?id=${id}`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const resultado = await response.json();

        if (resultado.isExitoso) {
            modoEdicion = true;
            clienteActual = resultado.data;
            cargarDatosEnFormulario(resultado.data);
            
            const modalTitle = document.getElementById('modalTitle');
            if (modalTitle) modalTitle.textContent = 'Editar Cliente';
            
            const modal = new bootstrap.Modal(document.getElementById('modalCliente'));
            modal.show();
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al cargar el cliente');
        }
    } catch (error) {
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al editar cliente:', error);
    } finally {
        ocultarCargando();
    }
}

// Ver detalles del cliente
async function verCliente(id) {
    try {
        mostrarCargando();
        const response = await fetch(`/Cliente/ObtenerPorId?id=${id}`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const resultado = await response.json();

        if (resultado.isExitoso) {
            mostrarDetallesCliente(resultado.data);
            const modal = new bootstrap.Modal(document.getElementById('modalVerCliente'));
            modal.show();
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al cargar los detalles del cliente');
        }
    } catch (error) {
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al ver cliente:', error);
    } finally {
        ocultarCargando();
    }
}

// Eliminar cliente
async function eliminarCliente(id) {
    if (!confirm('¿Está seguro de que desea eliminar este cliente?')) return;

    try {
        mostrarCargando();
        const response = await fetch('/Cliente/Eliminar', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': getAntiForgeryToken()
            },
            body: JSON.stringify({ id: id })
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const resultado = await response.json();
        ocultarCargando();

        if (resultado.isExitoso) {
            mostrarMensaje('success', 'Cliente eliminado exitosamente');
            cargarTodosLosClientes();
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al eliminar el cliente');
        }
    } catch (error) {
        ocultarCargando();
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al eliminar cliente:', error);
    }
}

// ===== FUNCIONES DE FILTRADO Y BÚSQUEDA =====

// Cargar todos los clientes
async function cargarTodosLosClientes() {
    try {
        mostrarCargando();
        const response = await fetch('/Cliente/ObtenerTodos');
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const resultado = await response.json();

        if (resultado.isExitoso) {
            actualizarTablaClientes(resultado.data);
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al cargar los clientes');
        }
    } catch (error) {
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al cargar clientes:', error);
    } finally {
        ocultarCargando();
    }
}

// Cargar solo clientes activos
async function cargarClientesActivos() {
    try {
        mostrarCargando();
        const response = await fetch('/Cliente/ObtenerActivos');
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const resultado = await response.json();

        if (resultado.isExitoso) {
            actualizarTablaClientes(resultado.data);
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al cargar los clientes activos');
        }
    } catch (error) {
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al cargar clientes activos:', error);
    } finally {
        ocultarCargando();
    }
}

// Buscar por razón social
async function buscarPorRazonSocial() {
    const razonSocial = document.getElementById('filtroRazonSocial')?.value.trim();
    if (!razonSocial) {
        mostrarMensaje('warning', 'Por favor ingrese un término de búsqueda');
        return;
    }

    try {
        mostrarCargando();
        const response = await fetch(`/Cliente/BuscarPorRazonSocial?razonSocial=${encodeURIComponent(razonSocial)}`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const resultado = await response.json();

        if (resultado.isExitoso) {
            actualizarTablaClientes(resultado.data);
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al buscar clientes');
        }
    } catch (error) {
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al buscar por razón social:', error);
    } finally {
        ocultarCargando();
    }
}

// Filtrar por fechas
async function filtrarPorFechas() {
    const fechaInicio = document.getElementById('fechaInicio')?.value;
    const fechaFin = document.getElementById('fechaFin')?.value;

    if (!fechaInicio || !fechaFin) {
        mostrarMensaje('warning', 'Por favor seleccione ambas fechas');
        return;
    }

    if (new Date(fechaInicio) > new Date(fechaFin)) {
        mostrarMensaje('warning', 'La fecha de inicio no puede ser mayor a la fecha fin');
        return;
    }

    try {
        mostrarCargando();
        const response = await fetch(`/Cliente/ObtenerPorRangoFechas?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`);
        
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        const resultado = await response.json();

        if (resultado.isExitoso) {
            actualizarTablaClientes(resultado.data);
        } else {
            mostrarMensaje('error', resultado.mensaje || 'Error al filtrar por fechas');
        }
    } catch (error) {
        mostrarMensaje('error', 'Error de conexión: ' + error.message);
        console.error('Error al filtrar por fechas:', error);
    } finally {
        ocultarCargando();
    }
}

// Aplicar filtros combinados
async function aplicarFiltros() {
    const ciudad = document.getElementById('filtroCiudad')?.value.trim();
    const pais = document.getElementById('filtroPais')?.value.trim();
    const tipo = document.getElementById('filtroTipo')?.value;

    if (!ciudad && !pais && !tipo) {
        mostrarMensaje('warning', 'Por favor seleccione al menos un filtro');
        return;
    }

    try {
        mostrarCargando();
        let clientes = [];

        // Aplicar filtros según lo seleccionado
        if (ciudad) {
            const response = await fetch(`/Cliente/ObtenerPorCiudad?ciudad=${encodeURIComponent(ciudad)}`);
            if (response.ok) {
                const resultado = await response.json();
                if (resultado.isExitoso) {
                    clientes = resultado.data;
                }
            }
        } else if (pais) {
            const response = await fetch(`/Cliente/ObtenerPorPais?pais=${encodeURIComponent(pais)}`);
            if (response.ok) {
                const resultado = await response.json();
                if (resultado.isExitoso) {
                    clientes = resultado.data;
                }
            }
        } else if (tipo) {
            const response = await fetch(`/Cliente/ObtenerPorTipo?tipoCliente=${encodeURIComponent(tipo)}`);
            if (response.ok) {
                const resultado = await response.json();
                if (resultado.isExitoso) {
                    clientes = resultado.data;
                }
            }
        }

        if (clientes.length > 0) {
            actualizarTablaClientes(clientes);
        } else {
            mostrarMensaje('info', 'No se encontraron clientes con los filtros aplicados');
            // Limpiar tabla
            const tbody = document.getElementById('tbodyClientes');
            if (tbody) tbody.innerHTML = '<tr><td colspan="10" class="text-center">No se encontraron resultados</td></tr>';
        }
    } catch (error) {
        mostrarMensaje('error', 'Error al aplicar filtros: ' + error.message);
        console.error('Error al aplicar filtros:', error);
    } finally {
        ocultarCargando();
    }
}

// Limpiar filtros
function limpiarFiltros() {
    const elementos = ['filtroRazonSocial', 'filtroCiudad', 'filtroPais', 'filtroTipo', 'fechaInicio', 'fechaFin'];
    elementos.forEach(id => {
        const elemento = document.getElementById(id);
        if (elemento) elemento.value = '';
    });
    cargarTodosLosClientes();
}

// ===== FUNCIONES DE VALIDACIÓN =====

// Validar formulario
function validarFormulario() {
    const razonSocial = document.getElementById('razonSocial')?.value.trim();
    const nit = document.getElementById('nit')?.value.trim();
    const tipoCliente = document.getElementById('tipoCliente')?.value;
    const correoContacto = document.getElementById('correoContacto')?.value.trim();

    if (!razonSocial) {
        mostrarMensaje('warning', 'La razón social es obligatoria');
        document.getElementById('razonSocial')?.focus();
        return false;
    }

    if (!nit) {
        mostrarMensaje('warning', 'El NIT es obligatorio');
        document.getElementById('nit')?.focus();
        return false;
    }

    if (!tipoCliente) {
        mostrarMensaje('warning', 'El tipo de cliente es obligatorio');
        document.getElementById('tipoCliente')?.focus();
        return false;
    }

    if (!correoContacto) {
        mostrarMensaje('warning', 'El correo de contacto es obligatorio');
        document.getElementById('correoContacto')?.focus();
        return false;
    }

    if (!validarEmail(correoContacto)) {
        mostrarMensaje('warning', 'El formato del correo electrónico no es válido');
        document.getElementById('correoContacto')?.focus();
        return false;
    }

    return true;
}

// Validar NIT en tiempo real
async function validarNitEnTiempoReal() {
    const nit = document.getElementById('nit')?.value.trim();
    if (!nit) return;

    try {
        const idExcluir = modoEdicion ? clienteActual?.id : null;
        const response = await fetch(`/Cliente/ValidarNitUnico?nit=${encodeURIComponent(nit)}&idExcluir=${idExcluir || ''}`);
        
        if (response.ok) {
            const resultado = await response.json();
            if (!resultado.isExitoso || !resultado.data) {
                mostrarMensaje('warning', 'Este NIT ya está registrado');
                document.getElementById('nit')?.focus();
            }
        }
    } catch (error) {
        console.error('Error al validar NIT:', error);
    }
}

// Validar correo en tiempo real
async function validarCorreoEnTiempoReal() {
    const correo = document.getElementById('correoContacto')?.value.trim();
    if (!correo) return;

    if (!validarEmail(correo)) {
        mostrarMensaje('warning', 'El formato del correo electrónico no es válido');
        return;
    }

    try {
        const idExcluir = modoEdicion ? clienteActual?.id : null;
        const response = await fetch(`/Cliente/ValidarCorreoUnico?correo=${encodeURIComponent(correo)}&idExcluir=${idExcluir || ''}`);
        
        if (response.ok) {
            const resultado = await response.json();
            if (!resultado.isExitoso || !resultado.data) {
                mostrarMensaje('warning', 'Este correo ya está registrado');
                document.getElementById('correoContacto')?.focus();
            }
        }
    } catch (error) {
        console.error('Error al validar correo:', error);
    }
}

// Validar formato de email
function validarEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// ===== FUNCIONES AUXILIARES =====

// Obtener token antiforgery
function getAntiForgeryToken() {
    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
    return token || '';
}

// Obtener datos del formulario
function obtenerDatosFormulario() {
    return {
        id: modoEdicion ? parseInt(document.getElementById('clienteId')?.value || '0') : 0,
        razonSocial: document.getElementById('razonSocial')?.value.trim() || '',
        nit: document.getElementById('nit')?.value.trim() || '',
        tipoCliente: document.getElementById('tipoCliente')?.value || '',
        representanteLegal: document.getElementById('representanteLegal')?.value.trim() || '',
        correoContacto: document.getElementById('correoContacto')?.value.trim() || '',
        telefonoContacto: document.getElementById('telefonoContacto')?.value.trim() || '',
        direccion: document.getElementById('direccion')?.value.trim() || '',
        ciudad: document.getElementById('ciudad')?.value.trim() || '',
        pais: document.getElementById('pais')?.value.trim() || '',
        paginaWeb: document.getElementById('paginaWeb')?.value.trim() || '',
        notas: document.getElementById('notas')?.value.trim() || '',
        activo: document.getElementById('activo')?.checked || false
    };
}

// Cargar datos en el formulario
function cargarDatosEnFormulario(cliente) {
    if (!cliente) return;
    
    const campos = {
        'clienteId': cliente.id,
        'razonSocial': cliente.razonSocial,
        'nit': cliente.nit,
        'tipoCliente': cliente.tipoCliente,
        'representanteLegal': cliente.representanteLegal,
        'correoContacto': cliente.correoContacto,
        'telefonoContacto': cliente.telefonoContacto,
        'direccion': cliente.direccion,
        'ciudad': cliente.ciudad,
        'pais': cliente.pais,
        'paginaWeb': cliente.paginaWeb,
        'notas': cliente.notas,
        'activo': cliente.activo
    };

    Object.entries(campos).forEach(([id, valor]) => {
        const elemento = document.getElementById(id);
        if (elemento) {
            if (elemento.type === 'checkbox') {
                elemento.checked = valor;
            } else {
                elemento.value = valor || '';
            }
        }
    });
}

// Limpiar formulario
function limpiarFormulario() {
    const form = document.getElementById('formCliente');
    if (form) form.reset();
    
    const clienteId = document.getElementById('clienteId');
    if (clienteId) clienteId.value = '';
}

// Actualizar tabla de clientes
function actualizarTablaClientes(clientes) {
    const tbody = document.getElementById('tbodyClientes');
    if (!tbody) return;
    
    tbody.innerHTML = '';

    if (!clientes || clientes.length === 0) {
        tbody.innerHTML = '<tr><td colspan="10" class="text-center">No se encontraron clientes</td></tr>';
        return;
    }

    clientes.forEach(cliente => {
        const row = document.createElement('tr');
        row.setAttribute('data-id', cliente.id);
        row.innerHTML = `
            <td>${cliente.id || ''}</td>
            <td>${cliente.razonSocial || ''}</td>
            <td>${cliente.nit || ''}</td>
            <td>
                <span class="badge ${cliente.tipoCliente === 'Jurídica' ? 'bg-primary' : 'bg-success'}">
                    ${cliente.tipoCliente || ''}
                </span>
            </td>
            <td>${cliente.correoContacto || ''}</td>
            <td>${cliente.ciudad || ''}</td>
            <td>${cliente.pais || ''}</td>
            <td>
                <span class="badge ${cliente.activo ? 'bg-success' : 'bg-danger'}">
                    ${cliente.activo ? 'Activo' : 'Inactivo'}
                </span>
            </td>
            <td>${formatearFecha(cliente.fechaRegistro)}</td>
            <td>
                <div class="btn-group" role="group">
                    <button class="btn btn-sm btn-info" onclick="verCliente(${cliente.id})" title="Ver">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-warning" onclick="editarCliente(${cliente.id})" title="Editar">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="eliminarCliente(${cliente.id})" title="Eliminar">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </td>
        `;
        tbody.appendChild(row);
    });
}

// Mostrar detalles del cliente
function mostrarDetallesCliente(cliente) {
    const detalles = document.getElementById('detallesCliente');
    if (!detalles || !cliente) return;
    
    detalles.innerHTML = `
        <div class="row">
            <div class="col-md-6">
                <h6>Información General</h6>
                <p><strong>ID:</strong> ${cliente.id || 'N/A'}</p>
                <p><strong>Razón Social:</strong> ${cliente.razonSocial || 'N/A'}</p>
                <p><strong>NIT:</strong> ${cliente.nit || 'N/A'}</p>
                <p><strong>Tipo Cliente:</strong> 
                    <span class="badge ${cliente.tipoCliente === 'Jurídica' ? 'bg-primary' : 'bg-success'}">
                        ${cliente.tipoCliente || 'N/A'}
                    </span>
                </p>
                <p><strong>Representante Legal:</strong> ${cliente.representanteLegal || 'N/A'}</p>
            </div>
            <div class="col-md-6">
                <h6>Información de Contacto</h6>
                <p><strong>Correo:</strong> ${cliente.correoContacto || 'N/A'}</p>
                <p><strong>Teléfono:</strong> ${cliente.telefonoContacto || 'N/A'}</p>
                <p><strong>Dirección:</strong> ${cliente.direccion || 'N/A'}</p>
                <p><strong>Ciudad:</strong> ${cliente.ciudad || 'N/A'}</p>
                <p><strong>País:</strong> ${cliente.pais || 'N/A'}</p>
                <p><strong>Página Web:</strong> ${cliente.paginaWeb || 'N/A'}</p>
            </div>
        </div>
        <div class="row mt-3">
            <div class="col-12">
                <h6>Información Adicional</h6>
                <p><strong>Estado:</strong> 
                    <span class="badge ${cliente.activo ? 'bg-success' : 'bg-danger'}">
                        ${cliente.activo ? 'Activo' : 'Inactivo'}
                    </span>
                </p>
                <p><strong>Fecha de Registro:</strong> ${formatearFecha(cliente.fechaRegistro)}</p>
                <p><strong>Notas:</strong> ${cliente.notas || 'Sin notas'}</p>
            </div>
        </div>
    `;
}

// Formatear fecha
function formatearFecha(fecha) {
    if (!fecha) return 'N/A';
    try {
        const date = new Date(fecha);
        return date.toLocaleDateString('es-ES', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit'
        });
    } catch (error) {
        return 'N/A';
    }
}

// ===== FUNCIONES DE UI =====

// Mostrar mensaje
function mostrarMensaje(tipo, mensaje) {
    const alertClass = tipo === 'success' ? 'alert-success' : 
                      tipo === 'error' ? 'alert-danger' : 
                      tipo === 'warning' ? 'alert-warning' : 'alert-info';

    const alertHtml = `
        <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
            ${mensaje}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;

    // Crear contenedor si no existe
    let alertContainer = document.getElementById('alertContainer');
    if (!alertContainer) {
        alertContainer = document.createElement('div');
        alertContainer.id = 'alertContainer';
        alertContainer.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 9999;
            max-width: 400px;
        `;
        document.body.appendChild(alertContainer);
    }

    // Agregar mensaje
    const alertElement = document.createElement('div');
    alertElement.innerHTML = alertHtml;
    alertContainer.appendChild(alertElement);

    // Auto-remover después de 5 segundos
    setTimeout(() => {
        if (alertElement.parentNode) {
            alertElement.parentNode.removeChild(alertElement);
        }
    }, 5000);
}

// Mostrar indicador de carga
function mostrarCargando() {
    let loading = document.getElementById('loadingOverlay');
    if (!loading) {
        loading = document.createElement('div');
        loading.id = 'loadingOverlay';
        loading.style.cssText = `
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            display: flex;
            justify-content: center;
            align-items: center;
            z-index: 9999;
        `;
        loading.innerHTML = `
            <div class="spinner-border text-light" role="status">
                <span class="visually-hidden">Cargando...</span>
            </div>
        `;
        document.body.appendChild(loading);
    } else {
        loading.style.display = 'flex';
    }
}

// Ocultar indicador de carga
function ocultarCargando() {
    const loading = document.getElementById('loadingOverlay');
    if (loading) {
        loading.style.display = 'none';
    }
}

