using Microsoft.AspNetCore.Mvc;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Application.Interfaces;

namespace PrestamosBiblioteca.Presentation.Api.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("api/[controller]")] // Define la ruta base para este controlador (e.g., /api/prestamos)
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;

        // Inyección de dependencia del servicio de préstamo
        public PrestamosController(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        /// <summary>
        /// Permite a un usuario solicitar el préstamo de un material bibliográfico.
        /// </summary>
        /// <param name="request">Datos de la solicitud de préstamo (ISBN del material, identificación del usuario).</param>
        /// <returns>Resultado del préstamo, incluyendo mensaje de éxito/error y fecha de devolución esperada.</returns>
        [HttpPost] // Define que este método responde a solicitudes POST
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrestamoResponse))] // Documentación de la respuesta exitosa
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PrestamoResponse))] // Documentación de la respuesta de error
        public async Task<IActionResult> RealizarPrestamo([FromBody] PrestamoRequest request)
        {
            // Validar si el request es nulo o inválido
            if (request == null)
            {
                return BadRequest(new PrestamoResponse { Exito = false, Mensaje = "La solicitud no puede ser nula." });
            }

            // Llamar al servicio de aplicación para procesar el préstamo
            var response = await _prestamoService.RealizarPrestamoAsync(request);

            // Devolver la respuesta adecuada según el resultado del servicio
            if (response.Exito)
            {
                return Ok(response); // HTTP 200 OK si el préstamo fue exitoso
            }
            else
            {
                // Para errores de negocio específicos (palíndromo, ya prestado, fin de semana),
                // se retorna un BadRequest (HTTP 400) con el mensaje de error.
                return BadRequest(response);
            }
        }

        // Podrías añadir otros endpoints aquí (e.g., GET para ver préstamos, PUT para devolver, etc.)
        // La prueba se centra en el préstamo, así que nos limitamos a este por ahora.
    }
}