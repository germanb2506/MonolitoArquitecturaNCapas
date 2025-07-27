# Sistema de Gestión Empresarial - .NET 8 + Arquitectura N-Capas Limpia (Razor Pages + PostgreSQL)

![.NET](https://img.shields.io/badge/.NET%208-blueviolet?logo=dotnet)
![Razor Pages](https://img.shields.io/badge/Razor%20Pages-Informational?logo=razor)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-336791?logo=postgresql)
![Arquitectura N-Capas](https://img.shields.io/badge/Arquitectura-N--Capas-blue)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-green?logo=entity-framework)

---

## Descripción General
Este proyecto es un sistema de gestión desarrollado en .NET 8, implementando una arquitectura N-Capas. Utiliza Razor Pages para la capa de presentación y Entity Framework Core con PostgreSQL para la persistencia de datos. El objetivo es ofrecer una base robusta, escalable y mantenible para aplicaciones empresariales.

---

## Arquitectura del Proyecto
El sistema sigue la **arquitectura N-Capas**, separando responsabilidades en diferentes proyectos:

- **Web**: Capa de presentación. Implementa Razor Pages y controla la interacción con el usuario.
- **App**: Capa de aplicación. Orquesta la lógica de negocio y coordina operaciones entre las demás capas. Incluye reglas de negocio, contratos (interfaces) y DTOs.
- **Domain**: Capa de dominio. Define las entidades limpias y reutilizables en caso de que sea necesario realizar migración de la base de datos a otro gestor, por ejemplo: PostgreSQL ➝ OracleDB.
- **Infrastructure**: Capa de infraestructura. Implementa detalles técnicos como acceso a datos, repositorios y servicios externos.

### Diagrama Simplificado
       ┌────────────────────────────────────────────────────────┐
       │                    Web (Razor Pages)                   │
       └────────────────────────────────────────────────────────┘
                          ↓                        ↑
       ┌────────────────────────────────────────────────────────┐
       │        App (Servicios, DTOs, Interfaces de Repos)      │
       └────────────────────────────────────────────────────────┘
                          ↓                        ↑
       ┌────────────────────────────────────────────────────────┐
       │ Infrastructure (Repositorios, EF Core, PostgreSQL,     │
       │           API Fluently, Servicios Externos)            │
       └────────────────────────────────────────────────────────┘
                          ↓                        ↑
       ┌────────────────────────────────────────────────────────┐
       │              Domain (Entidades puras)                  │
       └────────────────────────────────────────────────────────┘



---

## Principios y Patrones Aplicados
- **Separación de Responsabilidades (SRP)**: Cada capa tiene una función clara y aislada.
- **Inversión de Dependencias (DIP)**: Las capas superiores dependen de abstracciones, no de implementaciones concretas.
- **Abstracción y Desacoplamiento**: Uso de interfaces para desacoplar lógica de negocio y persistencia.
- **Patrón Repositorio**: Acceso a datos centralizado y desacoplado mediante repositorios genéricos y específicos.
- **Inyección de Dependencias**: Configurada en la capa Web para facilitar pruebas y mantenimiento.
- **Principios SOLID**: Aplicados en la estructura y diseño de clases y servicios.

---

## Tecnologías Utilizadas
- **.NET 8**
- **Razor Pages** (Presentación)
- **Entity Framework Core** (ORM)
- **PostgreSQL** (Base de datos relacional)
- **Inyección de dependencias nativa**
- **Automatización de migraciones y configuración por entorno**

---

## Estructura de Carpetas/Proyectos
- `/Web` - Presentación, configuración de servicios, controladores y vistas.
- `/App` - Lógica de aplicación, servicios, DTOs, interfaces de repositorios.
- `/Domain` - Entidades de dominio, interfaces y reglas de negocio.
- `/Infrastructure` - Implementación de repositorios, DbContext, mapeos y acceso a datos.

---

## Capa de Datos y Patrón Repositorio
- **DbContext**: Configura el acceso a la base de datos y mapea entidades.
- **GenericRepo<T>**: Implementa operaciones CRUD y manejo de transacciones de forma genérica.
- **Repositorios Específicos**: Ejemplo: `ClienteRepo` para operaciones particulares sobre la entidad Cliente.
- **Proyecciones y consultas optimizadas**: Métodos para obtener solo los datos necesarios.

---

## Configuración y Puesta en Marcha
1. **Requisitos**: .NET 8 SDK, PostgreSQL.
2. **Configura la cadena de conexión** en `Web/appsettings.json`.
3. **Restaura paquetes y compila**:
   ```bash
   dotnet restore
   dotnet build
Sientase libre de usar el codigo en sus proyectos, pero no olvide dar créditos al autor original.
Att : German Andres Bautista
https://github.com/germanb2506
   ```
  