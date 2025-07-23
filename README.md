# Sistema de Biblioteca -Grupo Éxito 📚

## Descripción del Proyecto

Este proyecto es una API REST desarrollada en .NET para el manejo de préstamos de materiales bibliográficos (libros y revistas) como parte de una prueba técnica para **Grupo Éxito**. El sistema implementa reglas de negocio específicas para el préstamo de materiales y utiliza una arquitectura basada en Clean Architecture y conceptos de diseño orientado a Dominio (Domain-Driven Design (DDD)) para asegurar una estructura robusta, mantenible y escalable. Se cuenta con Entity Framework y SQL Server como motor de base de datos.

## Características Principales

- **Gestión de Préstamos**: Sistema para préstamo de libros y revistas.
- **Reglas de Negocio**: 
  - ISBNs palíndromo solo para uso en biblioteca,no se pueden prestar (validado mediante método de extensión sobre `string`)
  - Libros: 10-15 días hábiles según suma de dígitos del ISBN
  - Revistas: solo se prestan 2 días hábiles y no pueden ser prestadas los fines de semana
  - Ajuste automático si fecha de devolución cae en domingo
- **Arquitectura Limpia**: Separación entre capas (Domain, Application, Infrastructure, Presentation)
- **Inyección de Dependencias**: Uso de Autofac
- **Base de Datos**: Entity Framework Core con SQL Server
- **Documentación API**: Swagger
- **Testing**: Pruebas unitarias e integración

## Tecnologías Utilizadas

- **.NET 8**
- **Entity Framework Core 9.0.7**
- **SQL Server**
- **Autofac** (Inyección de dependencias)
- **Swagger** (Documentación)
- **MSTest** (Pruebas unitarias)
- **NSubstitute** (Mocking)

---
## Patrones y Principios Aplicados 🧠

- **Principios SOLID**: Implementados en las capas de dominio y aplicación, especialmente en la separación de responsabilidades y la inversión de dependencias.
- **DDD (Domain-Driven Design)**: Modelo de dominio claro y representativo del problema.
- **Patrón Repository**: Para encapsular el acceso a datos.
- **Patrón DTO (Data Transfer Object)**: Para el transporte de datos entre capas.

---


## Supuestos Asumidos 📌

- **Días Hábiles**: Se consideran de lunes a sábado (domingo es no hábil).
- **Fechas de Devolución**: Si cae domingo, se ajusta automáticamente al siguiente lunes.
- **Un material no puede ser prestado más de una vez a la vez.**
- **Si el usuario o material no existen, se retorna error de validación.**
- **No se manejan reservas futuras en esta versión.**

---

## Limitaciones y Justificación del Alcance 🚧

Por limitaciones de tiempo y familiaridad técnica, **no se incluyeron las siguientes funcionalidades opcionales solicitadas en esta versión**

- **CI/CD Pipeline (Azure DevOps o GitHub Actions)**: Por enfoque en completar la lógica de negocio lo más rapido posible. Esta integración puede realizarse en futuras fases utilizando GitHub Actions o Azure Pipelines.
- **Despliegue en la nube (Azure u otra)**: No se realizó despliegue ya que se requería configuración de entornos y recursos que excedían el tiempo disponible.
- **Revisión de código estático con SonarQube**:Sin embargo, el código fue escrito bajo estándares de calidad, principios SOLID y arquitectura modular para facilitar su análisis posterior si se desea integrar.

---

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
## Arquitectura del Proyecto 🏗️
El proyecto sigue una estructura de Arquitectura Limpia (Clean Architecture), que promueve la separación de responsabilidades y la independencia de las capas. Esto se logra organizando el código en diferentes proyectos que representan capas concéntricas, donde las dependencias fluyen siempre hacia el centro.

## Endpoints de la API 🚀
La API expone los siguientes endpoints principales para la gestión de la biblioteca, se espera en la siguiente version implementar endpoints faltantes

### 1. Registrar Usuario
Descripción: Permite crear un nuevo usuario en el sistema de la biblioteca.

- **Método HTTP**: `POST`
- **Ruta**: `/api/usuario`
- **Request Body**:
Cuerpo de la Solicitud (Request Body): UsuarioRequest (Identificación, Nombre)

Respuestas:
- 200 OK: Usuario creado exitosamente.
- 400 Bad Request: Datos inválidos o usuario ya existente (identificación duplicado).

### 2. Registrar Material
Descripción: Permite añadir un nuevo material bibliográfico (libro o revista) al inventario de la biblioteca. Incluye validaciones para evitar ISBN duplicados y asegurar un tipo de material válido.

- **Método HTTP**: `POST`
- **Ruta**: `/api/Material`
- **Request Body**:
Cuerpo de la Solicitud (Request Body): MaterialRequest (ISBN, Nombre, TipoMaterial).

Respuestas:
- 200 OK: Material registrado exitosamente.
- 400 Bad Request: Datos inválidos o ISBN ya existente.

### 3. Registrar Préstamo
Descripción: Permite registrar un nuevo préstamo de un material a un usuario. Este endpoint implementa las reglas de negocio complejas, como la disponibilidad del material, las restricciones de préstamo para ISBN palíndromos, revistas en fin de semana, y el cálculo de la fecha de devolución esperada basado en el tipo de material y el ISBN.

- **Método HTTP**: `POST`
- **Ruta**: `/api/prestamos`
- **Request Body**:
Cuerpo de la Solicitud (Request Body): PrestamoRequest (ISBN del material, Identificacion del usuario).

Respuestas:
- 200 OK: Préstamo registrado exitosamente con la fecha de devolución esperada.
- 400 Bad Request: Préstamo no permitido debido a reglas de negocio (material no disponible, palíndromo, revista en fin de semana, etc.).


## Requisitos Previos
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
Editar el archivo BibliotecaGrupoExito.Presentation.Api/appsettings.json

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

### 3. Crear y Aplicar Migraciones

```bash
# Navegar al proyecto API
cd BibliotecaGrupoExito.Presentation.Api

# Crear migración inicial (si no existe)
dotnet ef migrations add InitialCreate -p ../BibliotecaGrupoExito.Infrastructure

# Aplicar migraciones a la base de datos
dotnet ef database update -p ../BibliotecaGrupoExito.Infrastructure
```

### 4. Ejecutar el Proyecto

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

## Datos de Prueba

Para probar la aplicación, necesitarás crear datos iniciales desde Sql si quieres . Puedes usar estos scripts SQL:

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
