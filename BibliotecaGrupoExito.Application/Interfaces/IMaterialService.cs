using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaGrupoExito.Application.DTOs;

namespace BibliotecaGrupoExito.Application.Interfaces
{
    public interface IMaterialService
    {
        Task<MaterialResponse> RegistrarMaterialAsync(MaterialRequest request);
        Task<MaterialResponse> EliminarMaterialAsync(Guid id);
    }
}
