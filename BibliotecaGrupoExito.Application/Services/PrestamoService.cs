using BibliotecaGrupoExito.Application.Interfaces;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Domain.Enums;
using BibliotecaGrupoExito.Domain.Extensions;

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
            var material = await _materialRepository.GetByIsbnAsync(request.ISBN);
            if (material == null)
            {
                return new PrestamoResponse { Exito = false, Mensaje = "Material no encontrado.", ISBN = request.ISBN };
            }

            var usuario = await _usuarioRepository.GetByIdentificationAsync(request.IdentificacionUsuario);
            if (usuario == null)
            {
                return new PrestamoResponse { Exito = false, Mensaje = "Usuario no encontrado.", ISBN = request.ISBN };
            }

            if (material.ISBN.IsPalindrome())
            {
                return new PrestamoResponse { Exito = false, Mensaje = "El material con ISBN en palíndromo solo es para uso en la biblioteca", ISBN = request.ISBN };
            }

            if (await _materialRepository.IsMaterialAvailableAsync(material.ISBN))
            {
                return new PrestamoResponse { Exito = false, Mensaje = "El material no puede ser prestado: Ese ISBN ya está prestado", ISBN = request.ISBN };
            }

            //Today's date for the loan
            DateTime fechaPrestamo = DateTime.Today;

            //Return date
            DateTime fechaDevolucionEsperada;
            if (material.TipoMaterial == TipoMaterial.Revista)
            {

                if (fechaPrestamo.DayOfWeek == DayOfWeek.Saturday || fechaPrestamo.DayOfWeek == DayOfWeek.Sunday)
                {
                    return new PrestamoResponse { Exito = false, Mensaje = "La revista no puede ser prestada para el fin de semana", ISBN = request.ISBN };
                }
                fechaDevolucionEsperada = DateCalculator.CalculaterReturnDate(fechaPrestamo, 2);
            }
            else
            {
                int sumaDigitosISBN = material.ISBN.SumDigits();
                int diasPrestamo = (sumaDigitosISBN > 30) ? 15 : 10;
                fechaDevolucionEsperada = DateCalculator.CalculaterReturnDate(fechaPrestamo, diasPrestamo);
            }

            fechaDevolucionEsperada = DateCalculator.AdjustIfSunday(fechaDevolucionEsperada);

       
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
                FechaPrestamo = fechaPrestamo,
                ISBN = material.ISBN
            };
        }

        public async Task<IEnumerable<PrestamoResponse>> ObtenerPrestamosPorMaterialAsync(string isbnMaterial)
        {
            var material = await _materialRepository.GetByIsbnAsync(isbnMaterial);
            if (material == null)
            {
                return Enumerable.Empty<PrestamoResponse>(); 
            }

            var prestamos = await _prestamoRepository.GetActiveLoansByIsbnAsync(isbnMaterial);

            var responses = new List<PrestamoResponse>();
            foreach (var prestamo in prestamos)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(prestamo.UsuarioId);
                responses.Add(new PrestamoResponse
                {
                    Exito = true,
                    ISBN = material.ISBN,
                    FechaPrestamo = prestamo.FechaPrestamo,
                    FechaDevolucionEsperada = prestamo.FechaDevolucionEsperada,
                });
            }
            return responses;
        }

    }
}