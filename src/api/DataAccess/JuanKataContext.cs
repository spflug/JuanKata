using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess;

public class JuanKataContext : DbContext
{
    public JuanKataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Specialization> Specializations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(et =>
        {
            et.HasKey(e => e.Id);
            et.HasIndex(e => e.Name);
            et.HasOne(e => e.Team);
            et.HasMany(e => e.Specializations);
        });
        modelBuilder.Entity<Specialization>(et =>
        {
            et.HasKey(e => e.Id);
            et.HasIndex(e => e.Name);
        });
        modelBuilder.Entity<Team>(et =>
        {
            et.HasKey(e => e.Id);
            et.HasIndex(e => e.Name);
        });
        base.OnModelCreating(modelBuilder);
    }
}