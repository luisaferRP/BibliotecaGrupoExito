using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Application.DTOs
{
    public class PrestamoRequest
    {
        public long ISBN { get; set; }
        public string IdentificacionUsuario { get; set; } = string.Empty;
    }
}
