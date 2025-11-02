using TdsCarParking.Core.Contracts;
using TdsCarParking.Core.Model;

namespace TdsCarParking.Core.Services;

public class ParkingService(IParkingSlotRepository parkingSlotRepository) : IParkingService
{
    public async Task<Result<ParkCarResult>> ParkCar(string vehicleReg, int vehicleType)
    {
        if (string.IsNullOrEmpty(vehicleReg)|| !Enum.IsDefined(typeof(VehicleType), vehicleType))
        {
            return Result<ParkCarResult>.Failed();
        }
        
        var slot = await parkingSlotRepository.GetFirstEmpty();
        if (slot == null)
        {
            return Result<ParkCarResult>.Failed();
        }

        slot.SetVehicle(vehicleReg, (VehicleType)vehicleType);
        
        await parkingSlotRepository.UpdateParkingSlot(slot);
        
        return Result<ParkCarResult>.Succeeded(new ParkCarResult(vehicleReg, slot.SpaceNumber, slot.Vehicle!.TimeIn));
    }
    
    public async Task<Result<UnparkCarResult>> UnparkCar(string vehicleReg)
    {
        if (string.IsNullOrEmpty(vehicleReg))
        {
            return Result<UnparkCarResult>.Failed();
        }
        
        var slot = await parkingSlotRepository.GetSlotByVehicleReg(vehicleReg);
        if (slot?.Vehicle == null)
        {
            return Result<UnparkCarResult>.Failed();
        }

        var response = new UnparkCarResult(vehicleReg, (int)slot.Vehicle.Type, slot.Vehicle.TimeIn, DateTime.UtcNow);

        slot.RemoveVehicle();
        await parkingSlotRepository.UpdateParkingSlot(slot);
        return Result<UnparkCarResult>.Succeeded(response);
    }
    
    public async Task<Result<ParkingStatusResult>> GetParkingStatus()
    {
        var occupiedSlots = await parkingSlotRepository.GetOccupiedCount();
        var freeSlots = await parkingSlotRepository.GetFreeCount();
        
        return Result<ParkingStatusResult>.Succeeded(new ParkingStatusResult(occupiedSlots, freeSlots));
    }
}