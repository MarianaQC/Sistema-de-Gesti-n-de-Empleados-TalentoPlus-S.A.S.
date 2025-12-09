using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TalentoPlus.Domain.Entities;

namespace TalentoPlus.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }
    
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
    public DbSet<EducationLevel> EducationLevels { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Document).IsRequired().HasMaxLength(20);
            entity.HasIndex(e => e.Document).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Salary).HasPrecision(18, 2);
            
            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);
                
            entity.HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId);
                
            entity.HasOne(e => e.EmployeeStatus)
                .WithMany(s => s.Employees)
                .HasForeignKey(e => e.EmployeeStatusId);
                
            entity.HasOne(e => e.EducationLevel)
                .WithMany(l => l.Employees)
                .HasForeignKey(e => e.EducationLevelId);
        });
        
        // Department configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(50);
        });
        
        // Position configuration
        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
        });
        
        // EmployeeStatus configuration
        modelBuilder.Entity<EmployeeStatus>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Name).IsRequired().HasMaxLength(30);
        });
        
        // EducationLevel configuration
        modelBuilder.Entity<EducationLevel>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Name).IsRequired().HasMaxLength(50);
        });
        
        // Seed initial data
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Tecnología" },
            new Department { Id = 2, Name = "Recursos Humanos" },
            new Department { Id = 3, Name = "Finanzas" },
            new Department { Id = 4, Name = "Marketing" },
            new Department { Id = 5, Name = "Operaciones" }
        );
        
        // Seed Positions
        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Name = "Gerente" },
            new Position { Id = 2, Name = "Coordinador" },
            new Position { Id = 3, Name = "Analista" },
            new Position { Id = 4, Name = "Auxiliar" },
            new Position { Id = 5, Name = "Director" }
        );
        
        // Seed Employee Statuses
        modelBuilder.Entity<EmployeeStatus>().HasData(
            new EmployeeStatus { Id = 1, Name = "Activo" },
            new EmployeeStatus { Id = 2, Name = "Inactivo" },
            new EmployeeStatus { Id = 3, Name = "Vacaciones" },
            new EmployeeStatus { Id = 4, Name = "Licencia" }
        );
        
        // Seed Education Levels
        modelBuilder.Entity<EducationLevel>().HasData(
            new EducationLevel { Id = 1, Name = "Bachiller" },
            new EducationLevel { Id = 2, Name = "Técnico" },
            new EducationLevel { Id = 3, Name = "Tecnólogo" },
            new EducationLevel { Id = 4, Name = "Profesional" },
            new EducationLevel { Id = 5, Name = "Especialización" },
            new EducationLevel { Id = 6, Name = "Maestría" },
            new EducationLevel { Id = 7, Name = "Doctorado" }
        );
    }
}