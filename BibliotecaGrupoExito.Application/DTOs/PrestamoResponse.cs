using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Application.DTOs
{
    public class PrestamoResponse
    {
       
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;

        public DateTime? FechaPrestamo { get; set; }
        public DateTime? FechaDevolucionEsperada { get; set; }
        public string ISBN { get; set; } 
        public string? NombreUsuario { get; set; }
        public string? IdentificacionUsuario { get; set; }
    }
}
