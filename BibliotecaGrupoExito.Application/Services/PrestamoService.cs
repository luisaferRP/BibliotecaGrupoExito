using BibliotecaGrupoExito.Application.Interfaces;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Domain.Enums;
using BibliotecaGrupoExito.Domain.Extensions; // Para IsPalindromo y SumarDigitos
using BibliotecaGrupoExito.Infrastructure.Common; // Para DateCalculator

namespace BibliotecaGrupoExito.Application.Services
{
    public class PrestamoService : IPrestamoService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        public PrestamoService(
        IMaterialRepository materialRepository,
        IUsuarioRepository usuarioRepository,
            IPrestamoRepository prestamoRepository)
        {
            _materialRepository = materialRepository;
            _usuarioRepository = usuarioRepository;
            _prestamoRepository = prestamoRepository;
        }

        public async Task<PrestamoResponse> RealizarPrestamoAsync(PrestamoRequest request)
        {
            // 1. Obtener Material y Usuario
            var material = await _materialRepository.GetByIsbnAsync(request.ISBN);
            if (material == null)
            {
                return new PrestamoResponse { Exito = false, Mensaje = "Material no encontrado.", ISBN = request.ISBN };
            }

            var usuario = await _usuarioRepository.GetByIdentificacionAsync(request.IdentificacionUsuario);
            if (usuario == null)
            {
                return new PrestamoResponse { Exito = false, Mensaje = "Usuario no encontrado.", ISBN = request.ISBN };
            }

            // 2. Regla: ISBN palíndromo
            if (material.ISBN.IsPalindromo())
            {
                return new PrestamoResponse { Exito = false, Mensaje = "El material con ISBN en palíndromo solo es para uso en la biblioteca", ISBN = request.ISBN };
            }

            // 3. Regla: Material ya prestado
            if (await _materialRepository.IsMaterialAvailableAsync(material.ISBN))
            {
                return new PrestamoResponse { Exito = false, Mensaje = "El material no puede ser prestado: Ese ISBN ya está prestado", ISBN = request.ISBN };
            }

            DateTime fechaPrestamo = DateTime.Today; // Asumimos la fecha de préstamo es hoy

            // 4. Calcular Fecha de Devolución según tipo de material y reglas
            DateTime fechaDevolucionEsperada;
            if (material.TipoMaterial == TipoMaterial.Revista)
            {
                // Regla: Revistas no pueden ser prestadas para el fin de semana y solo por dos días
                if (fechaPrestamo.DayOfWeek == DayOfWeek.Saturday || fechaPrestamo.DayOfWeek == DayOfWeek.Sunday)
                {
                    return new PrestamoResponse { Exito = false, Mensaje = "La revista no puede ser prestada para el fin de semana", ISBN = request.ISBN };
                }
                fechaDevolucionEsperada = DateCalculator.AddBusinessDays(fechaPrestamo, 2); // 2 días hábiles
            }
            else // TipoMaterial.Libro
            {
                // Regla: Libros se prestan por máximo 15 días hábiles si suma de dígitos > 30, sino 10 días hábiles
                int sumaDigitosISBN = material.ISBN.SumarDigitos();
                int diasPrestamo = (sumaDigitosISBN > 30) ? 15 : 10;
                fechaDevolucionEsperada = DateCalculator.AddBusinessDays(fechaPrestamo, diasPrestamo);
            }

            // 5. Regla: Si la fecha de entrega cae un domingo se debe entregar el siguiente día hábil.
            fechaDevolucionEsperada = DateCalculator.AdjustIfSunday(fechaDevolucionEsperada);

            // 6. Registrar el Préstamo
            var nuevoPrestamo = new Prestamo
            {
                Id = Guid.NewGuid(),
                MaterialId = material.Id,
                UsuarioId = usuario.Id,
                FechaPrestamo = fechaPrestamo,
                FechaDevolucionEsperada = fechaDevolucionEsperada,
                Activo = true
            };

            await _prestamoRepository.AddAsync(nuevoPrestamo);

            return new PrestamoResponse
            {
                Exito = true,
                Mensaje = "Préstamo realizado exitosamente.",
                FechaDevolucionEsperada = fechaDevolucionEsperada,
                ISBN = material.ISBN
            };
        }
    }
}