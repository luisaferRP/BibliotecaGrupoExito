using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BibliotecaGrupoExito.Domain.Enums;

namespace BibliotecaGrupoExito.Domain.Entities
{
    public class Material
    {
        [Key]
        public Guid Id { get; set; }
        public string ISBN { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public TipoMaterial TipoMaterial { get; set; }
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
