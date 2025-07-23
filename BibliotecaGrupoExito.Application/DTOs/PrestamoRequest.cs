using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Application.DTOs
{
    public class PrestamoRequest
    {
        [Required(ErrorMessage = "El ISBN del material es requerido.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "La identificación del usuario es requerida.")]
        [StringLength(50, ErrorMessage = "La identificación del usuario no puede exceder 50 caracteres.")]
        public string IdentificacionUsuario { get; set; } = string.Empty;
    }
}
