namespace TdsCarParking.Core.Contracts;

public interface IParkingService
{
    Task<Result<ParkCarResult>> ParkCar(string vehicleReg, int vehicleType);
    Task<Result<UnparkCarResult>> UnparkCar(string vehicleReg);
    Task<Result<ParkingStatusResult>> GetParkingStatus();
}