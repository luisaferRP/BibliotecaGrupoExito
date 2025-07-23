using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BibliotecaGrupoExito.Domain.Entities;

namespace BibliotecaGrupoExito.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Material> Materiales { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration for the Material entity
            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.ISBN).IsRequired(); 
                entity.HasIndex(m => m.ISBN).IsUnique(); 
                entity.Property(m => m.Nombre).IsRequired().HasMaxLength(255); 
                entity.Property(m => m.TipoMaterial)
                      .IsRequired()
                      .HasConversion<int>();
            });

            //Configuration for the User entity
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Identificacion).IsRequired();
                entity.HasIndex(u => u.Identificacion).IsUnique();
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(255);
            });

            // Configuration for the Loan entity
            modelBuilder.Entity<Prestamo>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.FechaPrestamo).IsRequired();
                entity.Property(p => p.FechaDevolucionEsperada).IsRequired();
                entity.Property(p => p.Activo).IsRequired();

                //Relationships
                entity.HasOne(p => p.Material)
                      .WithMany(m => m.Prestamos)
                      .HasForeignKey(p => p.MaterialId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Usuario)
                      .WithMany(u => u.Prestamos)
                      .HasForeignKey(p => p.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict); 
            });
        }
    }
}
