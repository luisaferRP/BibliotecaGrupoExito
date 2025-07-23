using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BibliotecaGrupoExito.Domain.Entities;
using BibliotecaGrupoExito.Domain.Enums; 

namespace BibliotecaGrupoExito.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet para cada una de tus entidades
        public DbSet<Material> Materiales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la entidad Material
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(m => m.Id); // Define Id como clave primaria
                entity.Property(m => m.ISBN).IsRequired(); // ISBN es requerido
                entity.HasIndex(m => m.ISBN).IsUnique(); // ISBN debe ser único
                entity.Property(m => m.Nombre).IsRequired().HasMaxLength(255); // Nombre es requerido y con longitud máxima
                entity.Property(m => m.TipoMaterial)
                      .IsRequired()
                      .HasConversion<int>(); // Mapea el enum TipoMaterial a un int en la DB
            });

            // Configuración para la entidad Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Identificacion).IsRequired();
                entity.HasIndex(u => u.Identificacion).IsUnique(); // Identificación debe ser única
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(255);
            });

            // Configuración para la entidad Prestamo
            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FechaPrestamo).IsRequired();
                entity.Property(p => p.FechaDevolucionEsperada).IsRequired();
                entity.Property(p => p.Activo).IsRequired();

                // Relaciones
                entity.HasOne(p => p.Material)
                      .WithMany(m => m.Prestamos)
                      .HasForeignKey(p => p.MaterialId)
                      .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada de Material

                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.Prestamos)
                      .HasForeignKey(p => p.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict); // Evita borrado en cascada de Usuario
            });
        }
    }
}
