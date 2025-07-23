using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public bool Activo { get; set; }

        [ForeignKey("MaterialId")]
        public Material Material { get; set; } = null!;
        public Usuario Usuario { get; set; } = null!;
    }
}
