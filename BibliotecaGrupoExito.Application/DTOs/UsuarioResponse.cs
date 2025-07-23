using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Application.DTOs
{
    public class UsuarioResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public Guid? Id { get; set; } 
        public string? Identificacion { get; set; }
        public string? NombreCompleto { get; set; }
    }
}
