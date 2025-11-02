namespace TdsCarParking.Core.Contracts;

public interface IChargeService
{
    Task<Result<ChargeCalculationResult>> GetChargeAmount(int vehicleType, DateTime timeIn, DateTime timeOut);
}