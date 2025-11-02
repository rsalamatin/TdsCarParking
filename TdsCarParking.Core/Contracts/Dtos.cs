namespace TdsCarParking.Core.Contracts;

public record ParkCarResult(string VehicleReg, int SpaceNumber, DateTime TimeIn);

public record UnparkCarResult(string VehicleReg, int VehileType, DateTime TimeIn, DateTime TimeOut);

public record ParkingStatusResult(int OccupiedSlots, int FreeSlots);

public record ChargeCalculationResult(decimal ChargeAmount);
