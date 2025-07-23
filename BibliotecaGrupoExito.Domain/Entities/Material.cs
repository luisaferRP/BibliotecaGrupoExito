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
        public long ISBN { get; set; } // ¡Cambiado de string a long!
        public string Nombre { get; set; } = string.Empty;
        public TipoMaterial TipoMaterial { get; set; }

        // Propiedad de navegación para Entity Framework (opcional pero útil para acceder a préstamos)
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
