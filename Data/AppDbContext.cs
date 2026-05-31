using Microsoft.EntityFrameworkCore;
using PolicyStreetAssessment.Models;

namespace PolicyStreetAssessment.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>()
            .HasIndex(d => d.DepartmentCode).IsUnique();

        modelBuilder.Entity<Position>()
            .HasIndex(p => p.PositionCode).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.EmployeeCode).IsUnique();

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email).IsUnique();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Salary)
            .HasColumnType("decimal(7,2)");
    }
}
