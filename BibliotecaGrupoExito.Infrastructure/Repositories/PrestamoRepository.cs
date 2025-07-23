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

        public async Task<IEnumerable<Prestamo>> GetLoansByUserIdAsync(Guid userId)
        {
            return await _context.Prestamos.Where(p => p.UsuarioId == userId).ToListAsync();
        }

        public async Task<IEnumerable<Prestamo>> GetActiveLoansByIsbnAsync(string isbn)
        {
            return await _context.Prestamos.Include(p => p.Material)
                .Where(p => p.Material.ISBN.ToLower() == isbn.ToLower() && p.Activo).ToListAsync();
        }

        public async Task UpdateAsync(Prestamo prestamo)
        {
            _context.Prestamos.Update(prestamo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
            }
        }
    }
}