using Microsoft.EntityFrameworkCore;
using TdsCarParking.Core.Model.Parking;

namespace TdsCarParking.Infrastructure.Dal;

public class ParkingSlotContext : DbContext
{
    public ParkingSlotContext(DbContextOptions<ParkingSlotContext> options)
        : base(options)
    {
    }
    
    public DbSet<ParkingSlot> ParkingSlots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ParkingSlotEntityConfiguration());
    }
}