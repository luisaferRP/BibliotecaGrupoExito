using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Application.DTOs
{
    public class MaterialResponse
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string TipoMaterial { get; set; } = string.Empty;
    }
}
