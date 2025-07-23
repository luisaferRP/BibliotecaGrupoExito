using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Application.Interfaces;
using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Infrastructure.Repositories;

namespace BibliotecaGrupoExito.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<UsuarioResponse> RegistrarUsuarioAsync(UsuarioRequest request)
        {

            var existingUserByIdentification = await _usuarioRepository.GetByIdentificationAsync(request.Identificacion);
            if (existingUserByIdentification != null)
            {
                return new UsuarioResponse
                {
                    Exito = false,
                    Mensaje = $"Ya existe un usuario con la identificación: {request.Identificacion}"
                };
            }

            var nuevoUsuario = new Usuario 
            {
                Id = Guid.NewGuid(),
                Identificacion = request.Identificacion,
                Nombre = request.Nombre,
            };
           
            await _usuarioRepository.AddAsync(nuevoUsuario);

            
            return new UsuarioResponse
            {
                Exito = true,
                Mensaje = "Usuario registrado exitosamente.",
                Id = nuevoUsuario.Id,
                Identificacion = nuevoUsuario.Identificacion,
                NombreCompleto = nuevoUsuario.Nombre,
            };
        }
    }
}

