using BibliotecaGrupoExito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Domain.Interfaces
{
    public interface IMaterialRepository
    {
        Task<Material?> GetByIdAsync(Guid id);
        Task<Material?> GetByIsbnAsync(long isbn);
        Task<bool> IsMaterialAvailableAsync(long isbn); 
        Task AddAsync(Material material);
        Task UpdateAsync(Material material);
        Task DeleteAsync(Guid id);

    }
}
