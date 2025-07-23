using Microsoft.AspNetCore.Mvc;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace PrestamosBiblioteca.Presentation.Api.Controllers
{
    [ApiController] 
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoService _prestamoService;
        public PrestamosController(IPrestamoService prestamoService)
        {
            _prestamoService = prestamoService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Realizar un prestamo",
            Description = "Este endpoint es para realizar un prestamo"
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PrestamoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(PrestamoResponse))]
        public async Task<IActionResult> RealizarPrestamo([FromBody] PrestamoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new PrestamoResponse { Exito = false, Mensaje = "Los datos de ingresados son inválidos." });
            }

            var response = await _prestamoService.RealizarPrestamoAsync(request);

            if (response.Exito)
            {
                return Ok(response); 
            }
            else
            {
                return BadRequest(response);
            }
        }



        [HttpGet("material")]
        [SwaggerOperation(
            Summary = "Buscar materiales prestados",
            Description = "Este endpoint es para realizar la busqueda de materiales prestados"
        )]
        [ProducesResponseType(typeof(IEnumerable<PrestamoResponse>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> ObtenerPrestamosPorMaterial(string isbnMaterial)
        {
            if (string.IsNullOrWhiteSpace(isbnMaterial))
            {
                return BadRequest("El ISBN del material es requerido.");
            }
          
            var prestamos = await _prestamoService.ObtenerPrestamosPorMaterialAsync(isbnMaterial);

          
            return Ok(prestamos);
        }

        [HttpGet("todos")]
        [SwaggerOperation(
            Summary = "Listar todos los prestamos",
            Description = "Este endpoint es para listar todos los prestamos"
        )]
        [ProducesResponseType(typeof(IEnumerable<PrestamoResponse>), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<IActionResult> ObtenerTodosLosPrestamos()
        {
            var prestamos = await _prestamoService.ObtenerTodosLosPrestamosAsync();

            if (prestamos == null || !prestamos.Any())
            {
                return NoContent(); 
            }

            return Ok(prestamos);
        }

    }
}