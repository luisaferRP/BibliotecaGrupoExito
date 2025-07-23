using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaGrupoExito.Application.DTOs;

namespace BibliotecaGrupoExito.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<UsuarioResponse> RegistrarUsuarioAsync(UsuarioRequest request);
    }
}
