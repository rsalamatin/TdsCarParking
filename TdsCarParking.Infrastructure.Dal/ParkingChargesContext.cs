using Microsoft.EntityFrameworkCore;
using TdsCarParking.Core.Model.Charge;
using TdsCarParking.Core.Model.Parking;

namespace TdsCarParking.Infrastructure.Dal;

public class ParkingChargesContext : DbContext
{
    public ParkingChargesContext(DbContextOptions<ParkingChargesContext> options)
        : base(options)
    {
    }
    
    public DbSet<ParkingCharge> ParkingCharges { get; set; }

}