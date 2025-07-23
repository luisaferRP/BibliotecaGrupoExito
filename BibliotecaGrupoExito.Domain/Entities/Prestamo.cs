using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Domain.Entities
{
    public class Prestamo
    {
        public Guid Id { get; set; }
        public Guid MaterialId { get; set; }
        public Guid UsuarioId { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public bool Activo { get; set; } // Indica si el préstamo está actualmente activo

        // Propiedades de navegación para Entity Framework
        public Material Material { get; set; } = null!; // Null-forgiving operator, será cargado por EF
        public Usuario Usuario { get; set; } = null!; // Null-forgiving operator, será cargado por EF
    }
}
