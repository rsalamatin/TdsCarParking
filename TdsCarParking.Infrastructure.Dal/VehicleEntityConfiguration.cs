using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TdsCarParking.Core.Model.Parking;

namespace TdsCarParking.Infrastructure.Dal;

public class ParkingSlotEntityConfiguration : IEntityTypeConfiguration<ParkingSlot>
{
    public void Configure(EntityTypeBuilder<ParkingSlot> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder.OwnsOne(ps => ps.Vehicle);
        builder.Navigation(ps => ps.Vehicle).AutoInclude();
    }
}
