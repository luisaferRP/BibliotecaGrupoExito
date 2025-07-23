using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaGrupoExito.Application.DTOs;
using BibliotecaGrupoExito.Application.Interfaces;
using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Enums;
using BibliotecaGrupoExito.Domain.Interfaces;

namespace BibliotecaGrupoExito.Application.Services
{
    public class MaterialService : IMaterialService
    {

        private readonly IMaterialRepository _materialRepository;
        private readonly IPrestamoRepository _prestamoRepository;
        public MaterialService(
        IMaterialRepository materialRepository,
        IPrestamoRepository prestamoRepository)
        {
            _materialRepository = materialRepository;
            _prestamoRepository = prestamoRepository;
        }
        public async Task<MaterialResponse> EliminarMaterialAsync(Guid id)
        {
            var material = await _materialRepository.GetByIdAsync(id);
            if (material == null)
            {
                return new MaterialResponse { Exito = false, Mensaje = "El Material que desea eliminar no existe." };
            }

            var activeLoans = await _prestamoRepository.GetActiveLoansByIsbnAsync(material.ISBN);
            if (activeLoans != null && activeLoans.Any())
            {
                return new MaterialResponse { Exito = false, Mensaje = "No se puede eliminar el material porque actualmente está prestado." };
            }

            await _materialRepository.DeleteAsync(id);

            return new MaterialResponse { Exito = true, Mensaje = "Material eliminado con éxito." };
        }

        public async Task<MaterialResponse> RegistrarMaterialAsync(MaterialRequest request)
        {
            var existingMaterial = await _materialRepository.GetByIsbnAsync(request.ISBN);
            if (existingMaterial != null)
            {
                return new MaterialResponse { Exito = false, Mensaje = $"Ya existe un material con el ISBN: {request.ISBN}" };
            }

            if (!Enum.TryParse(request.TipoMaterial, true, out TipoMaterial tipoMaterialEnum))
            {
                return new MaterialResponse { Exito = false, Mensaje = "Tipo de material no válido. Debe ser 'Libro' o 'Revista'." };
            }

            var newMaterial = new Material
            {
                ISBN = request.ISBN,
                Nombre = request.Nombre,
                TipoMaterial = tipoMaterialEnum,
            };

            await _materialRepository.AddAsync(newMaterial);

            return new MaterialResponse
            {
                Exito = true,
                Mensaje = "Material registrado exitosamente.",
                ISBN = newMaterial.ISBN,
                Nombre = newMaterial.Nombre,
                TipoMaterial = newMaterial.TipoMaterial.ToString()
            }; 

        }
    }
}
