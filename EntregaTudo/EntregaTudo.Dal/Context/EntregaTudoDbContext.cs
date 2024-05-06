using EntregaTudo.Core.Domain.Business.Delivery;
using EntregaTudo.Core.Domain.Business.Vehicle;
using EntregaTudo.Core.Domain.Infrastructure;
using EntregaTudo.Core.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace EntregaTudo.Dal.Context;

public class EntregaTudoDbContext(DbContextOptions<EntregaTudoDbContext> options) : DbContext(options)
{
    DbSet<Person> Persons { get; set; }
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<Address> Address { get; set; }
    DbSet<Delivery> Deliveries { get; set; }
    DbSet<ItemDelivery> ItemDeliveries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EntregaTudoDbContext).Assembly);

        modelBuilder.Entity<Person>(entity =>
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

            entity.HasOne(x => x.Vehicle)
                .WithOne()
                .HasForeignKey<Vehicle>(d => d.Id);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.StreetAddress)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.NumberAddress)
                .IsRequired()
                .HasMaxLength(20);
            entity.Property(e => e.AddressComplement)
                .HasMaxLength(100);
            entity.Property(e => e.Neighborhood)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.State)
                .IsRequired();
            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(10);
            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Latitude)
                .IsRequired();
            entity.Property(e => e.Longitude)
                .IsRequired();

        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.ToTable("Vehicles");

            entity.HasKey(v => v.Id);

            entity.Property(v => v.Brand)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(v => v.Model)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(v => v.ManufactureYear)
                .IsRequired();

            entity.Property(v => v.LicensePlate)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(v => v.LoadCapacity)
                .IsRequired();

            entity.Property(v => v.VehicleStatus);

            entity.Property(v => v.VehicleType)
                .IsRequired();
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.ToTable("Deliveries");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.DeliveryStatus)
                .IsRequired();

            entity.Property(e => e.ScheduledTime)
                .IsRequired();

            entity.Property(e => e.DeliveryCost)
                .IsRequired();

            entity.Property(e => e.DeliveryNote);

            entity.Property(e => e.DeliveryCode);

            entity.HasMany(d => d.Items)
                .WithOne()
                .HasForeignKey("DeliveryId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.OriginDelivery)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.DestinationDelivery)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ItemDelivery>(entity =>
        {
            entity.ToTable("ItemDeliveries");

            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Description);

            entity.Property(e => e.Weight)
                .IsRequired();
        });

        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseSqlServer("Server=.\\LUCAS;Database=EntregaTudo;Trusted_Connection=true;Encrypt=false;MultipleActiveResultSets=true;",
            options => options.MigrationsAssembly("EntregaTudo.Api"));
    }
}