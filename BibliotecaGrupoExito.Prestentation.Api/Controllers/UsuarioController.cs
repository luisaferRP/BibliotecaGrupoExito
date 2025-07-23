using Microsoft.AspNetCore.Mvc;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Application.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace BibliotecaGrupoExito.Prestentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registar un usuario",
            Description = "Este endpoint es para registrar un usuario,para porder realizar el prestamo"
        )]
        [ProducesResponseType(typeof(UsuarioResponse), 200)] 
        [ProducesResponseType(typeof(UsuarioResponse), 400)] 
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioRequest request)
        {

            if (!ModelState.IsValid)
            {
     
                return BadRequest(ModelState);
            }


            var result = await _usuarioService.RegistrarUsuarioAsync(request);

     
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

