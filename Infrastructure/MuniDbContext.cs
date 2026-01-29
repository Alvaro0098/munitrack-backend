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
            modelBuilder.Entity<Operator>().HasData(
                new Operator { DNI = 46502865, Name = "Micaela", LastName = "Ortigoza", NLegajo = 459850, Password = "123abc", Phone = "3416897542", Email = "micaela@example.com", Position = Role.OperatorBasic, Deleted = 0 },
                new Operator { NLegajo = 9999, DNI = 111, Name = "Admin", LastName = "General", Password = "admin", Phone = "12345678", Email = "admin@munitrack.com", Position = Role.SysAdmin, Deleted = 0 },
                new Operator { DNI = 43567210, Name = "Lucas", LastName = "Fernandez", NLegajo = 459851, Password = "abc12345", Phone = "3416549871", Email = "lucas@example.com", Position = Role.OperatorBasic, Deleted = 0 }
            );

            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Name = "Oficina Martin Fierro", Description = "Atiende trámites generales", Deleted = 0 },
                new Area { Id = 2, Name = "Oficina Gender", Description = "Atiende temas de género", Deleted = 0 }
            );

            modelBuilder.Entity<Incidence>().HasData(
                new Incidence { Id = 1, Date = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc), IncidenceType = IncidenceType.FoodBag, Description = "Luz rota en Av. Pellegrini 2000", State = IncidenceState.Started, Deleted = 0, AreaId = 1 },
                new Incidence { Id = 2, Date = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-2), DateTimeKind.Utc), IncidenceType = IncidenceType.Complaint, Description = "Bache en San Martín y Rioja", State = IncidenceState.InProgress, Deleted = 0, AreaId = 2 }
            );

            modelBuilder.Entity<Citizen>().HasData(
                new Citizen { DNI = 46502865, Name = "Micaela", LastName = "Ortigoza", Adress = "Calle Falsa 123", Phone = "3416897542", Email = "micaela@example.com" },
                new Citizen { DNI = 43567210, Name = "Lucas", LastName = "Fernandez", Adress = "Av. San Martín 456", Phone = "3416549871", Email = "lucas@example.com" }
            );

            modelBuilder.Entity<Operator>().HasMany(o => o.Citizens).WithMany();
            modelBuilder.Entity<Citizen>().HasMany(c => c.Incidences).WithMany();
            modelBuilder.Entity<Operator>().HasMany(o => o.Incidences).WithMany();

            modelBuilder.Entity<Incidence>().HasOne(i => i.Area).WithMany().HasForeignKey(i => i.AreaId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Incidence>().Property(i => i.Date).HasColumnType("timestamp with time zone");

            base.OnModelCreating(modelBuilder);
        }
    }
}