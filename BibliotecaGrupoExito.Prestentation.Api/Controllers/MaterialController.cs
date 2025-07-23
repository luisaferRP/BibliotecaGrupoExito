using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Application.Interfaces;
using BibliotecaGrupoExito.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaGrupoExito.Prestentation.Api.Controllers
{
    [ApiController] // Indica que es un controlador de API
    [Route("api/[controller]")] 
    public class MaterialController : Controller
    {
        private readonly IMaterialService _materialService;
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registar un material libro/revista",
            Description = "Este endpoint es para listar todos los prestamos"
        )]
        [ProducesResponseType(typeof(MaterialResponse), 200)] 
        [ProducesResponseType(typeof(MaterialResponse), 400)]
        public async Task<IActionResult> RegistrarMaterial([FromBody] MaterialRequest request)
        {
          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _materialService.RegistrarMaterialAsync(request);

            if (result.Exito)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
