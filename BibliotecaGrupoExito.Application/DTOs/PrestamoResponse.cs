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
        public DateTime? FechaDevolucionEsperada { get; set; }
        public long? ISBN { get; set; } // Añadir ISBN al response para contexto
    }
}
