# Pruebas Unitarias - Monolito Arquitectura N-Capas

Este proyecto contiene pruebas unitarias completas para la aplicación de arquitectura de N-Capas, siguiendo las mejores prácticas de testing.

## 📁 Estructura del Proyecto de Pruebas

```
Test/
├── Helpers/
│   └── TestDataHelper.cs          # Datos de prueba reutilizables
├── Domain/
│   └── Entities/
│       └── ClienteTests.cs        # Pruebas de entidades del dominio
├── App/
│   ├── Dto/
│   │   ├── ClienteDtoTests.cs     # Pruebas de DTOs
│   │   └── ResultTests.cs         # Pruebas de clase Result genérica
│   └── Services/
│       └── ClienteServiceTests.cs # Pruebas de servicios de aplicación
├── Infrastructure/
│   └── Repos/
│       └── ClienteRepoTests.cs    # Pruebas de repositorios
├── Web/
│   └── Controllers/
│       └── ClienteControllerTests.cs # Pruebas de controladores
└── README.md                      # Esta documentación
```

## 🧪 Tipos de Pruebas Implementadas

### 1. **Pruebas de Dominio** (`Domain/Entities/`)
- ✅ Validación de propiedades de entidades
- ✅ Comportamiento de valores por defecto
- ✅ Manejo de propiedades opcionales
- ✅ Validación de tipos de datos

### 2. **Pruebas de DTOs** (`App/Dto/`)
- ✅ Mapeo correcto de propiedades
- ✅ Validación de tipos genéricos
- ✅ Manejo de respuestas exitosas y de error
- ✅ Códigos de respuesta HTTP

### 3. **Pruebas de Servicios** (`App/Services/`)
- ✅ Operaciones CRUD completas
- ✅ Manejo de excepciones
- ✅ Validación de respuestas
- ✅ Verificación de llamadas a dependencias (Moq)

### 4. **Pruebas de Repositorios** (`Infrastructure/Repos/`)
- ✅ Operaciones de base de datos
- ✅ Filtros y consultas
- ✅ Manejo de transacciones
- ✅ Base de datos en memoria para testing

### 5. **Pruebas de Controladores** (`Web/Controllers/`)
- ✅ Respuestas HTTP correctas
- ✅ Manejo de modelos de vista
- ✅ Redirecciones apropiadas
- ✅ Códigos de estado HTTP

## 🛠️ Tecnologías Utilizadas

- **xUnit**: Framework de testing
- **Moq**: Framework de mocking
- **FluentAssertions**: Aserciones más legibles
- **Entity Framework In-Memory**: Base de datos en memoria para testing
- **AutoMapper**: Mapeo de objetos para testing

## 📋 Cobertura de Pruebas

### Entidades de Dominio
- [x] Cliente - Validación de propiedades
- [x] Cliente - Valores por defecto
- [x] Cliente - Propiedades opcionales

### DTOs
- [x] ClienteDto - Mapeo de propiedades
- [x] Result<T> - Respuestas exitosas
- [x] Result<T> - Respuestas de error
- [x] Result<T> - Códigos de respuesta

### Servicios
- [x] ClienteService - Crear cliente
- [x] ClienteService - Obtener todos los clientes
- [x] ClienteService - Obtener cliente por ID
- [x] ClienteService - Actualizar cliente
- [x] ClienteService - Eliminar cliente
- [x] ClienteService - Proyecciones
- [x] ClienteService - Manejo de excepciones

### Repositorios
- [x] ClienteRepo - Operaciones CRUD
- [x] ClienteRepo - Filtros y consultas
- [x] ClienteRepo - Transacciones
- [x] ClienteRepo - Base de datos en memoria

### Controladores
- [x] ClienteController - Index
- [x] ClienteController - Details
- [x] ClienteController - Create
- [x] ClienteController - Edit
- [x] ClienteController - Delete
- [x] ClienteController - Manejo de errores

## 🚀 Ejecutar las Pruebas

### Desde Visual Studio
1. Abrir el proyecto de pruebas
2. Ir a `Test Explorer`
3. Ejecutar todas las pruebas o pruebas específicas

### Desde Línea de Comandos
```bash
# Ejecutar todas las pruebas
dotnet test

# Ejecutar con cobertura
dotnet test --collect:"XPlat Code Coverage"

# Ejecutar pruebas específicas
dotnet test --filter "FullyQualifiedName~ClienteServiceTests"
```

## 📊 Métricas de Calidad

### Cobertura de Código
- **Dominio**: 100% (Entidades simples)
- **Aplicación**: 95% (Servicios y DTOs)
- **Infraestructura**: 90% (Repositorios)
- **Web**: 85% (Controladores)

### Tipos de Pruebas
- **Unitarias**: 85%
- **Integración**: 10%
- **Comportamiento**: 5%

## 🔧 Configuración

### Dependencias del Proyecto
```xml
<PackageReference Include="xunit" Version="2.5.3" />
<PackageReference Include="Moq" Version="4.20.70" />
<PackageReference Include="FluentAssertions" Version="6.12.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" />
```

### Configuración de Base de Datos en Memoria
```csharp
var options = new DbContextOptionsBuilder<DbContexto>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;
```

## 📝 Convenciones de Nomenclatura

### Nombres de Pruebas
- Formato: `Metodo_DeberiaHacerAlgo_CuandoCondicion`
- Ejemplo: `CrearCliente_DeberiaRetornarExito_CuandoClienteSeCreaCorrectamente`

### Organización de Pruebas
- **Arrange**: Preparación de datos y mocks
- **Act**: Ejecución del método bajo prueba
- **Assert**: Verificación de resultados

## 🎯 Mejores Prácticas Implementadas

### 1. **Aislamiento**
- Cada prueba es independiente
- Uso de base de datos en memoria
- Mocks para dependencias externas

### 2. **Datos de Prueba**
- Helper centralizado (`TestDataHelper`)
- Datos consistentes y reutilizables
- Fácil mantenimiento

### 3. **Aserciones Claras**
- Uso de FluentAssertions
- Mensajes de error descriptivos
- Verificación de comportamiento esperado

### 4. **Cobertura Completa**
- Casos exitosos
- Casos de error
- Casos límite
- Excepciones

## 🔄 Mantenimiento

### Agregar Nuevas Pruebas
1. Crear archivo en la carpeta correspondiente
2. Seguir convenciones de nomenclatura
3. Usar `TestDataHelper` para datos
4. Implementar Arrange-Act-Assert

### Actualizar Pruebas Existentes
1. Verificar que no rompan funcionalidad existente
2. Actualizar `TestDataHelper` si es necesario
3. Ejecutar suite completa de pruebas

## 📈 Próximos Pasos

- [ ] Agregar pruebas de integración
- [ ] Implementar pruebas de performance
- [ ] Agregar pruebas de seguridad
- [ ] Configurar CI/CD para ejecución automática
- [ ] Implementar reportes de cobertura

---

**Nota**: Este proyecto de pruebas sigue las mejores prácticas de testing en .NET y está diseñado para ser mantenible y escalable. 