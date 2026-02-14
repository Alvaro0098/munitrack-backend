using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MuniDbContext : DbContext
    {
        public DbSet<Operator> Operators { get; set; }
        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<Incidence> Incidences { get; set; }
        public DbSet<Area> Areas { get; set; }

        public MuniDbContext() { }
        public MuniDbContext(DbContextOptions<MuniDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Seed de Áreas (Usa "Id" porque así está en tu clase Area)
            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Name = "Oficina Martin Fierro", Description = "Atiende trámites generales", Deleted = 0 },
                new Area { Id = 2, Name = "Oficina Gender", Description = "Atiende temas de género", Deleted = 0 }
            );

            // 2. Seed de Operadores (Usa "NLegajo" porque es tu [Key])
            modelBuilder.Entity<Operator>().HasData(
                new Operator { NLegajo = 9999, DNI = 111, Name = "Admin", LastName = "General", Password = "adminpassword", Phone = "12345678", Email = "admin@munitrack.com", Position = Role.SysAdmin, Deleted = 0 },
                new Operator { NLegajo = 459850, DNI = 46502865, Name = "Micaela", LastName = "Ortigoza", Password = "123abc123", Phone = "3416897542", Email = "micaela@example.com", Position = Role.OperatorBasic, Deleted = 0 }
            );

            // 3. Seed de Ciudadanos (Usa "DNI" porque es tu [Key])
            modelBuilder.Entity<Citizen>().HasData(
                new Citizen { DNI = 46502865, Name = "Micaela", LastName = "Ortigoza", Adress = "Calle Falsa 123", Phone = "3416897542", Email = "micaela@example.com" }
            );

            // --- Configuraciones de Relaciones ---

            // Relación Operador -> Citizens (CreatedByOperatorId)
            modelBuilder.Entity<Operator>()
                .HasMany(o => o.Citizens)
                .WithOne() 
                .HasForeignKey("CreatedByOperatorId"); 

            // Relación Citizen -> Incidences
            modelBuilder.Entity<Citizen>()
                .HasMany(c => c.Incidences)
                .WithOne()
                .HasForeignKey("CitizenId");

            // Relación Incidence -> Area (Evita borrado en cascada)
            modelBuilder.Entity<Incidence>()
                .HasOne(i => i.Area)
                .WithMany()
                .HasForeignKey(i => i.AreaId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relación Incidence -> Operator
            modelBuilder.Entity<Incidence>()
                .HasOne(i => i.Operator)
                .WithMany(o => o.Incidences)
                .HasForeignKey(i => i.OperatorId);

            base.OnModelCreating(modelBuilder);
        }
    }
}