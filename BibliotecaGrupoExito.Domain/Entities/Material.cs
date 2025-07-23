using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaGrupoExito.Domain.Enums;

namespace BibliotecaGrupoExito.Domain.Entities
{
    public class Material
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public TipoMaterial TipoMaterial { get; set; }
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
