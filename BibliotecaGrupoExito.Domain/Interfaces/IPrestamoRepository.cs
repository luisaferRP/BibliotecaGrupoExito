using BibliotecaGrupoExito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Domain.Interfaces
{
    public interface IPrestamoRepository
    {
        Task<Prestamo?> GetByIdAsync(Guid id);
        Task AddAsync(Prestamo prestamo);
        Task UpdateAsync(Prestamo prestamo);
        Task<IEnumerable<Prestamo>> GetPrestamosActivosByIsbnAsync(long isbn);
    }
}
