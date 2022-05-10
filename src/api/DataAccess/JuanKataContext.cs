using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess;

public class JuanKataContext : DbContext
{
    protected JuanKataContext() : this(new DbContextOptions<JuanKataContext>())
    {
    }

    public JuanKataContext(DbContextOptions options) : base(options)
    {
    }

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