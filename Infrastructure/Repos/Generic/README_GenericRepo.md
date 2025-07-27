# Documentación del GenericRepo

## Descripción General

El `GenericRepo<T>` es una implementación del patrón Repository que proporciona operaciones CRUD genéricas y gestión de transacciones para entidades de Entity Framework Core.

## Características Principales

- ✅ Operaciones CRUD completas
- ✅ Gestión de transacciones
- ✅ Proyecciones optimizadas
- ✅ Control de tracking de entidades
- ✅ Filtros con expresiones lambda
- ✅ Operaciones asíncronas

## Configuración Inicial

### 1. Registro en el contenedor de dependencias

```csharp
// En Program.cs o Startup.cs
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
```

### 2. Inyección de dependencias

```csharp
public class MiServicio
{
    private readonly IGenericRepo<Cliente> _clienteRepo;
    
    public MiServicio(IGenericRepo<Cliente> clienteRepo)
    {
        _clienteRepo = clienteRepo;
    }
}
```

## Operaciones CRUD Básicas

### Crear una entidad

```csharp
// Crear un nuevo cliente
var nuevoCliente = new Cliente 
{ 
    Nombre = "Juan Pérez", 
    Email = "juan@email.com" 
};

await _clienteRepo.Crear(nuevoCliente);
```

### Obtener una entidad

```csharp
// Obtener por ID (asumiendo que Cliente tiene una propiedad Id)
var cliente = await _clienteRepo.Obtener(c => c.Id == 1);

// Obtener por email
var clientePorEmail = await _clienteRepo.Obtener(c => c.Email == "juan@email.com");

// Obtener sin tracking (para operaciones de solo lectura)
var clienteSinTracking = await _clienteRepo.Obtener(c => c.Id == 1, tracked: false);
```

### Obtener múltiples entidades

```csharp
// Obtener todos los clientes
var todosLosClientes = await _clienteRepo.ObtenerTodos();

// Obtener clientes con filtro
var clientesActivos = await _clienteRepo.ObtenerTodos(c => c.Activo == true);

// Obtener clientes por nombre que contenga "Juan"
var clientesJuan = await _clienteRepo.ObtenerTodos(c => c.Nombre.Contains("Juan"));
```

### Actualizar una entidad

```csharp
// Obtener la entidad
var cliente = await _clienteRepo.Obtener(c => c.Id == 1);

// Modificar propiedades
cliente.Nombre = "Juan Carlos Pérez";
cliente.Email = "juancarlos@email.com";

// Guardar cambios
await _clienteRepo.Grabar();
```

### Eliminar una entidad

```csharp
// Obtener la entidad a eliminar
var clienteAEliminar = await _clienteRepo.Obtener(c => c.Id == 1);

// Eliminar
await _clienteRepo.Remover(clienteAEliminar);
```

## Gestión de Transacciones

### Transacción simple

```csharp
try
{
    await _clienteRepo.IniciarTransaccion();
    
    // Realizar múltiples operaciones
    await _clienteRepo.Crear(nuevoCliente);
    await _clienteRepo.Crear(otroCliente);
    
    await _clienteRepo.ConfirmarTransaccion();
}
catch (Exception)
{
    await _clienteRepo.RevertirTransaccion();
    throw;
}
```

### Transacción con múltiples repositorios

```csharp
// Asumiendo que tienes múltiples repositorios inyectados
try
{
    await _clienteRepo.IniciarTransaccion();
    
    // Operaciones en cliente
    await _clienteRepo.Crear(nuevoCliente);
    
    // Operaciones en otro repositorio (usando la misma transacción)
    await _pedidoRepo.Crear(nuevoPedido);
    
    await _clienteRepo.ConfirmarTransaccion();
}
catch (Exception)
{
    await _clienteRepo.RevertirTransaccion();
    throw;
}
```

## Proyecciones Optimizadas

### Obtener proyección de una entidad

```csharp
// Obtener solo el nombre y email de un cliente
var clienteInfo = await _clienteRepo.GetProy(
    filtro: c => c.Id == 1,
    selector: c => new { c.Nombre, c.Email }
);

// Obtener solo el nombre
var nombreCliente = await _clienteRepo.GetProy(
    filtro: c => c.Id == 1,
    selector: c => c.Nombre
);
```

### Obtener lista proyectada

```csharp
// Obtener lista de nombres de clientes activos
var nombresClientes = await _clienteRepo.GetAllProy(
    filtro: c => c.Activo == true,
    selector: c => c.Nombre
);

// Obtener lista de objetos con múltiples propiedades
var clientesResumen = await _clienteRepo.GetAllProy(
    filtro: c => c.FechaRegistro >= DateTime.Today.AddDays(-30),
    selector: c => new 
    { 
        c.Id, 
        c.Nombre, 
        c.Email,
        FechaRegistro = c.FechaRegistro.ToString("dd/MM/yyyy")
    }
);
```

## Ejemplos de Uso Avanzado

### Consultas complejas con múltiples condiciones

```csharp
// Clientes activos registrados en el último mes con email válido
var clientesFiltrados = await _clienteRepo.ObtenerTodos(c => 
    c.Activo == true && 
    c.FechaRegistro >= DateTime.Today.AddMonths(-1) &&
    c.Email.Contains("@")
);
```

### Ordenamiento (requiere extensión del repositorio)

```csharp
// Para ordenamiento, puedes usar LINQ después de obtener los datos
var clientesOrdenados = (await _clienteRepo.ObtenerTodos())
    .OrderBy(c => c.Nombre)
    .ToList();
```

### Paginación básica

```csharp
// Obtener todos y aplicar paginación en memoria
var todosClientes = await _clienteRepo.ObtenerTodos();
var pagina = todosClientes
    .Skip((paginaActual - 1) * elementosPorPagina)
    .Take(elementosPorPagina)
    .ToList();
```

## Mejores Prácticas

### 1. Uso de tracking

```csharp
// ✅ Para operaciones de solo lectura
var cliente = await _clienteRepo.Obtener(c => c.Id == 1, tracked: false);

// ✅ Para operaciones que requieren modificación
var clienteParaModificar = await _clienteRepo.Obtener(c => c.Id == 1, tracked: true);
```

### 2. Manejo de transacciones

```csharp
// ✅ Siempre usar try-catch con transacciones
try
{
    await _repo.IniciarTransaccion();
    // Operaciones...
    await _repo.ConfirmarTransaccion();
}
catch
{
    await _repo.RevertirTransaccion();
    throw;
}
```

### 3. Uso de proyecciones

```csharp
// ✅ Usar proyecciones para optimizar consultas
var nombres = await _repo.GetAllProy(
    filtro: c => c.Activo == true,
    selector: c => c.Nombre
);

// ❌ Evitar traer toda la entidad cuando solo necesitas una propiedad
var todosLosClientes = await _repo.ObtenerTodos(c => c.Activo == true);
var nombres = todosLosClientes.Select(c => c.Nombre).ToList();
```

### 4. Manejo de errores

```csharp
// ✅ Validar antes de operaciones
var cliente = await _clienteRepo.Obtener(c => c.Id == id);
if (cliente == null)
{
    throw new NotFoundException($"Cliente con ID {id} no encontrado");
}

// ✅ Usar operaciones asíncronas correctamente
await _clienteRepo.Crear(cliente); // Siempre usar await
```

## Consideraciones de Rendimiento

1. **Usar `AsNoTracking()`** para consultas de solo lectura
2. **Aplicar filtros** en la base de datos, no en memoria
3. **Usar proyecciones** para reducir la cantidad de datos transferidos
4. **Evitar N+1 queries** usando `Include()` cuando sea necesario
5. **Manejar transacciones** de manera eficiente

## Limitaciones Actuales

- No incluye ordenamiento nativo
- No incluye paginación nativa
- No incluye operaciones de inclusión (Include)
- No incluye operaciones de agregación

## Extensiones Recomendadas

Para funcionalidades adicionales, considera extender el repositorio con:

```csharp
// Ordenamiento
public async Task<List<T>> ObtenerTodosOrdenados<TKey>(
    Expression<Func<T, bool>>? filtro = null,
    Expression<Func<T, TKey>>? ordenarPor = null,
    bool ascendente = true
)

// Paginación
public async Task<(List<T> Items, int Total)> ObtenerPaginados(
    Expression<Func<T, bool>>? filtro = null,
    int pagina = 1,
    int elementosPorPagina = 10
)

// Inclusión de entidades relacionadas
public async Task<T> ObtenerConInclude(
    Expression<Func<T, bool>> filtro,
    params Expression<Func<T, object>>[] includes
)
``` 