using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaGrupoExito.Infrastructure.Repositories

{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly ApplicationDbContext _context;

        public MaterialRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Material material)
        {
            await _context.Materiales.AddAsync(material);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var material = await _context.Materiales.FindAsync(id);
            if (material != null)
            {
                _context.Materiales.Remove(material);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Material?> GetByIdAsync(Guid id)
        {
            return await _context.Materiales.FindAsync(id);
        }

        public async Task<Material?> GetByIsbnAsync(string isbn)
        {
            return await _context.Materiales.FirstOrDefaultAsync(m => m.ISBN.ToLower() == isbn.ToLower());
        }

        public async Task<bool> IsMaterialAvailableAsync(string isbn)
        {
            return await _context.Prestamos
                                 .AnyAsync(p => p.Material.ISBN.ToLower() == isbn.ToLower() && p.Activo);
        }
    }
}