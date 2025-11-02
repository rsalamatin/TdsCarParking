using TdsCarParking.Core.Model;
using TdsCarParking.Core.Model.Charge;

namespace TdsCarParking.Core.Contracts;

public interface IParkingChargesRepository
{
    Task<List<ParkingCharge>> FindParkingCharges(VehicleType? vehicleType, ChargeType chargeType);
}