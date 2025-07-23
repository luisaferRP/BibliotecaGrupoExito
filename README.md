# Sistema de Biblioteca -Grupo Éxito 📚

## Descripción del Proyecto

Este proyecto es una API REST desarrollada en .NET para el manejo de préstamos de materiales bibliográficos (libros y revistas) como parte de una prueba técnica para **Grupo Éxito**. El sistema implementa reglas de negocio específicas para el préstamo de materiales y utiliza una arquitectura basada en Clean Architecture y conceptos de diseño orientado a Dominio (Domain-Driven Desing -DDD) para asegurar una estructura robusta, mantenible y escalable, tambien se cuenta con Entity Framework y SQL Server.

## Características Principales

- **Gestión de Préstamos**: Sistema completo para préstamo de libros y revistas
- **Reglas de Negocio**: 
  - ISBNs palíndromo solo para uso en biblioteca
  - Libros: 10-15 días hábiles según suma de dígitos del ISBN
  - Revistas: 2 días hábiles, no prestables en fines de semana
  - Ajuste automático si fecha de devolución cae en domingo
- **Arquitectura Limpia**: Separación clara entre capas (Domain, Application, Infrastructure, Presentation)
- **Inyección de Dependencias**: Uso de Autofac
- **Base de Datos**: Entity Framework Core con SQL Server
- **Documentación API**: Swagger/OpenAPI
- **Testing**: Pruebas unitarias e integración completas

## Tecnologías Utilizadas

- **.NET 8**
- **Entity Framework Core 9.0.7**
- **SQL Server / PostgreSQL** (configurable)
- **Autofac** (Inyección de dependencias)
- **Swagger/OpenAPI** (Documentación)
- **MSTest** (Pruebas unitarias)
- **NSubstitute** (Mocking)

## Estructura del Proyecto

```
BibliotecaGrupoExito/
├── BibliotecaGrupoExito.Domain/           # Entidades y reglas de negocio
├── BibliotecaGrupoExito.Application/      # Casos de uso y servicios
├── BibliotecaGrupoExito.Infrastructure/   # Acceso a datos y servicios externos
├── BibliotecaGrupoExito.Presentation.Api/ # Controladores y configuración API
├── BibliotecaGrupoExito.Tests.Unit/       # Pruebas unitarias
└── BibliotecaGrupoExito.Tests.Integration/# Pruebas de integración
```
##Arquitectura del Proyecto 🏗️
El proyecto sigue una estructura de Arquitectura Limpia (Clean Architecture), que promueve la separación de responsabilidades y la independencia de las capas. Esto se logra organizando el código en diferentes proyectos que representan capas concéntricas, donde las dependencias fluyen siempre hacia el centro.

##Endpoints Principales de la API 🚀
La API expone los siguientes endpoints principales para la gestión de la biblioteca:

## Requisitos Previos
1. Registrar Usuario
Descripción: Permite crear un nuevo usuario en el sistema de la biblioteca.

Método HTTP: POST

Ruta: /api/usuarios

Cuerpo de la Solicitud (Request Body): UsuarioRequest (Identificación, Nombre)

Respuestas:

200 OK: Usuario creado exitosamente.

400 Bad Request: Datos inválidos o usuario ya existente (identificación duplicado).

2. Registrar Material
Descripción: Permite añadir un nuevo material bibliográfico (libro o revista) al inventario de la biblioteca. Incluye validaciones para evitar ISBN duplicados y asegurar un tipo de material válido.

Método HTTP: POST

Ruta: /api/materiales

Cuerpo de la Solicitud (Request Body): MaterialRequest (ISBN, Nombre, TipoMaterial).

Respuestas:

200 OK: Material registrado exitosamente.

400 Bad Request: Datos inválidos o ISBN ya existente.

3. Registrar Préstamo
Descripción: Permite registrar un nuevo préstamo de un material a un usuario. Este endpoint implementa las reglas de negocio complejas, como la disponibilidad del material, las restricciones de préstamo para ISBN palíndromos, revistas en fin de semana, y el cálculo de la fecha de devolución esperada basado en el tipo de material y el ISBN.
Método HTTP: POST

Ruta: /api/prestamos
Respuestas:

200 OK: Préstamo registrado exitosamente con la fecha de devolución esperada.

400 Bad Request: Préstamo no permitido debido a reglas de negocio (material no disponible, palíndromo, revista en fin de semana, etc.).

Cuerpo de la Solicitud (Request Body): PrestamoRequest (ISBN del material, Identificacion del usuario).
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server/) o [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- Git

## Instalación y Configuración

### 1. Clonar el Repositorio

```bash
git clone https://github.com/luisaferRP/BibliotecaGrupoExito
cd BibliotecaGrupoExito
```

### 2. Configurar la Base de Datos

#### Opción A: SQL Server Local/Express

En esta ocacion se puso la cadena de connection en el appsettings.json, puedes cambiarla de ser requerido

Editar `BibliotecaGrupoExito.Presentation.Api/appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=grupo_exito_db;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=false"
  }
}
```
### 5. Restaurar Dependencias

```bash
dotnet restore
```

### 6. Crear y Aplicar Migraciones

```bash
# Navegar al proyecto API
cd BibliotecaGrupoExito.Presentation.Api

# Crear migración inicial (si no existe)
dotnet ef migrations add InitialCreate -p ../BibliotecaGrupoExito.Infrastructure

# Aplicar migraciones a la base de datos
dotnet ef database update -p ../BibliotecaGrupoExito.Infrastructure
```

### 7. Ejecutar el Proyecto

```bash
# Desde la carpeta del proyecto API
dotnet run

# O para desarrollo con recarga automática
dotnet watch run
```

La API estará disponible en:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `https://localhost:5001/swagger` aunque ya entra por defecto acá


## Uso de la API

### Endpoint Principal: Realizar Préstamo

**POST** `/api/prestamos`

**Request Body:**
```json
{
  "isbn": 1234567890123,
  "identificacionUsuario": "12345678"
}
```

**Response Exitoso (200 OK):**
```json
{
  "exito": true,
  "mensaje": "Préstamo realizado exitosamente.",
  "fechaDevolucionEsperada": "2024-08-15T00:00:00",
  "isbn": 1234567890123
}
```

**Response Error (400 Bad Request):**
```json
{
  "exito": false,
  "mensaje": "El material con ISBN en palíndromo solo es para uso en la biblioteca",
  "fechaDevolucionEsperada": null,
  "isbn": 12321
}
```

### Casos de Ejemplo

#### 1. Préstamo Exitoso de Libro
```bash
curl -X POST "https://localhost:5001/api/prestamos" \
  -H "Content-Type: application/json" \
  -d '{
    "isbn": 1234567890123,
    "identificacionUsuario": "user001"
  }'
```

#### 2. Error: ISBN Palíndromo
```bash
curl -X POST "https://localhost:5001/api/prestamos" \
  -H "Content-Type: application/json" \
  -d '{
    "isbn": 12321,
    "identificacionUsuario": "user001"
  }'
```

## Datos de Prueba

Para probar la aplicación, necesitarás crear datos iniciales. Puedes usar estos scripts SQL:

### Crear Usuarios de Prueba
```sql
INSERT INTO Usuarios (Id, Identificacion, Nombre) VALUES 
(NEWID(), 'user001', 'Juan Pérez'),
(NEWID(), 'user002', 'María García'),
(NEWID(), 'user003', 'Carlos López');
```

### Crear Materiales de Prueba
```sql
INSERT INTO Materiales (Id, ISBN, Nombre, TipoMaterial) VALUES 
(NEWID(), 1234567890123, 'El Quijote', 0), -- Libro (suma dígitos = 51 > 30)
(NEWID(), 12345, 'Cien Años de Soledad', 0), -- Libro (suma dígitos = 15 ≤ 30)
(NEWID(), 98765, 'National Geographic', 1), -- Revista
(NEWID(), 12321, 'Libro Palíndromo', 0); -- ISBN palíndromo
```

## Ejecutar Pruebas

### Pruebas Unitarias
```bash
dotnet test BibliotecaGrupoExito.Tests.Unit/
```

### Pruebas de Integración
```bash
dotnet test BibliotecaGrupoExito.Tests.Integration/
```

### Todas las Pruebas
```bash
dotnet test
```

## Comandos Útiles de Entity Framework

```bash
# Crear nueva migración
dotnet ef migrations add NombreMigration -p BibliotecaGrupoExito.Infrastructure

# Actualizar base de datos
dotnet ef database update -p BibliotecaGrupoExito.Infrastructure

# Eliminar última migración
dotnet ef migrations remove -p BibliotecaGrupoExito.Infrastructure

# Generar script SQL
dotnet ef migrations script -p BibliotecaGrupoExito.Infrastructure

# Ver información de la base de datos
dotnet ef dbcontext info -p BibliotecaGrupoExito.Infrastructure
```

## Solución de Problemas Comunes

### Error de Conexión a Base de Datos
1. Verificar que SQL Server esté ejecutándose
2. Confirmar la cadena de conexión en `appsettings.json`
3. Verificar permisos de usuario en la base de datos

### Error de Migración
```bash
# Limpiar y recrear migraciones
dotnet ef database drop -p BibliotecaGrupoExito.Infrastructure
dotnet ef migrations remove -p BibliotecaGrupoExito.Infrastructure
dotnet ef migrations add InitialCreate -p BibliotecaGrupoExito.Infrastructure
dotnet ef database update -p BibliotecaGrupoExito.Infrastructure
```

## Estructura de Archivos de Configuración

```
BibliotecaGrupoExito.Presentation.Api/
├── .env                          # Variables de entorno (crear)
├── appsettings.json             # Configuración producción
├── appsettings.Development.json # Configuración desarrollo
└── Properties/
    └── launchSettings.json      # Configuración de inicio
```

## Contacto y Soporte

Para preguntas sobre la implementación o el proyecto:

- **Desarrollador**: Luisa Fernanda Ramírez Porras
- **Email**: luisaramiresporras103@gmail.com
- **Proyecto**: Prueba Técnica Grupo Éxito
- **Fecha**: Julio 23 del 2025

## Licencia

Este proyecto fue desarrollado como parte de una prueba técnica para Grupo Éxito.

---
Desarrollado para Grupo Éxito ❤️

**Nota**: Este README proporciona todas las instrucciones necesarias para ejecutar el proyecto localmente. Asegúrate de seguir todos los pasos en orden y verificar que todas las dependencias estén correctamente instaladas.
