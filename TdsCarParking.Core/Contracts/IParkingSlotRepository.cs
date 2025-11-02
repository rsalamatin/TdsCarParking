using TdsCarParking.Core.Model.Parking;

namespace TdsCarParking.Core.Contracts;

public interface IParkingSlotRepository
{
    Task<ParkingSlot?> GetFirstEmpty();
    Task<ParkingSlot> UpdateParkingSlot(ParkingSlot slot);
    Task<ParkingSlot?> GetSlotByVehicleReg(string vehicleReg);
    Task<int> GetOccupiedCount();
    Task<int> GetFreeCount();
}