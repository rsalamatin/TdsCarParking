using Microsoft.EntityFrameworkCore;
using TdsCarParking.Core.Contracts;
using TdsCarParking.Core.Model;
using TdsCarParking.Core.Model.Charge;

namespace TdsCarParking.Infrastructure.Dal.Repositories;

public class ParkingChargesRepository(ParkingChargesContext context) : IParkingChargesRepository
{
    public async Task<List<ParkingCharge>> FindParkingCharges(VehicleType? vehicleType, ChargeType chargeType)
    {
        return await context.ParkingCharges.Where(c => c.CarType == vehicleType && c.ChargeType == chargeType).ToListAsync();
    }
}