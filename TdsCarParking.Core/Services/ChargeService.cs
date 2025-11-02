using TdsCarParking.Core.Contracts;
using TdsCarParking.Core.Model;
using TdsCarParking.Core.Model.Charge;

namespace TdsCarParking.Core.Services;

public class ChargeService(IParkingChargesRepository parkingChargesRepository) : IChargeService
{
    public async Task<Result<ChargeCalculationResult>> GetChargeAmount(int vehicleType, DateTime timeIn, DateTime timeOut)
    {
        if (!Enum.IsDefined(typeof(VehicleType), vehicleType))
        {
            return Result<ChargeCalculationResult>.Failed();
        }

        var vehicleCharges = await parkingChargesRepository.FindParkingCharges((VehicleType)vehicleType, ChargeType.Regular);
        var additionalCharges = await parkingChargesRepository.FindParkingCharges(null, ChargeType.Additional);

        var numberOfParkingMinutes = Math.Floor((timeOut - timeIn).TotalMinutes);
        var regularCharge = calculateChargeAmount(vehicleCharges, numberOfParkingMinutes);
        var additionalCharge = calculateChargeAmount(additionalCharges, numberOfParkingMinutes);
        var totalCharge = regularCharge + additionalCharge;
        return Result<ChargeCalculationResult>.Succeeded(new ChargeCalculationResult(totalCharge));
    }
    
    private decimal calculateChargeAmount(List<ParkingCharge> charges, double numberOfParkingMinutes)
    {
        return charges.Select(c => Math.Floor((decimal)(numberOfParkingMinutes / c.TimeIntervalMinutes)) * c.Value).Sum();
    }
}