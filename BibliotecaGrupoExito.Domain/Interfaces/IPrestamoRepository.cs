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
        Task DeleteAsync(Guid id); 
        Task<IEnumerable<Prestamo>> GetActiveLoansByIsbnAsync(string isbn);
        Task<IEnumerable<Prestamo>> GetLoansByUserIdAsync(Guid userId);
        Task<IEnumerable<Prestamo>> GetAllLoansAsync();

    }
    
}