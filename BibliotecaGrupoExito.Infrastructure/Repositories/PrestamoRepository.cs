using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Interfaces;
using BibliotecaGrupoExito.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BibliotecaGrupoExito.Infrastructure.Repositories
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly ApplicationDbContext _context;

        public PrestamoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Prestamo prestamo)
        {
            await _context.Prestamos.AddAsync(prestamo);
            await _context.SaveChangesAsync();
        }

        public async Task<Prestamo?> GetByIdAsync(Guid id)
        {
            return await _context.Prestamos.FindAsync(id);
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosActivosByIsbnAsync(long isbn)
        {
            // Incluye el material para poder filtrar por ISBN
            return await _context.Prestamos
                                 .Include(p => p.Material) // Asegura que Material esté cargado
                                 .Where(p => p.Material.ISBN == isbn && p.Activo)
                                 .ToListAsync();
        }

        public async Task UpdateAsync(Prestamo prestamo)
        {
            _context.Prestamos.Update(prestamo);
            await _context.SaveChangesAsync();
        }
    }
}