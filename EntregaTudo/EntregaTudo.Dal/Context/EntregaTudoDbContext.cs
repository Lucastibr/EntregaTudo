using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace EntregaTudo.Dal.Context;

public class EntregaTudoDbContext(DbContextOptions<EntregaTudoDbContext> options) : DbContext(options)
{
    DbSet<Customer> Customers { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntregaTudoDbContext).Assembly);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(80);
            entity.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(80);
            entity.Property(p => p.DocumentNumber)
                .IsRequired()
                .HasMaxLength(14);
            entity.Property(p => p.Email)
                .IsRequired();
            entity.Property(p => p.PhoneNumber)
                .IsRequired();
            entity.Property(p => p.PersonType)
                .IsRequired();

            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
        });

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Server=.\\LUCASAXION;Database=EntregaTudo;Trusted_Connection=true;Encrypt=false;MultipleActiveResultSets=true;",
            options => options.MigrationsAssembly("EntregaTudo.Api"));
    }
}