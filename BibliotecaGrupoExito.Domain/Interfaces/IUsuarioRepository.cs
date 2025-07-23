using BibliotecaGrupoExito.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaGrupoExito.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> GetByIdAsync(Guid id);
        Task<Usuario?> GetByIdentificacionAsync(string identificacion);
        Task AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(Guid id);
    }
}
