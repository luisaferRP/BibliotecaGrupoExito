using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Application.DTOs
{
    public class MaterialRequest
    {
        [Required(ErrorMessage = "El ISBN es requerido.")]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del material es requerido.")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de material es requerido (0. Libro/ 1. Revista).")]
        [StringLength(50, ErrorMessage = "El tipo de material no puede exceder 50 caracteres.")]
        public string TipoMaterial { get; set; } = string.Empty; 
    }
}
