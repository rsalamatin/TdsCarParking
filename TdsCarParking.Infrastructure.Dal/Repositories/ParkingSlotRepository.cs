using Microsoft.EntityFrameworkCore;
using TdsCarParking.Core.Contracts;
using TdsCarParking.Core.Model.Parking;

namespace TdsCarParking.Infrastructure.Dal.Repositories;

public class ParkingSlotRepository(ParkingSlotContext context) : IParkingSlotRepository
{
    public async Task<ParkingSlot?> GetFirstEmpty()
    {
        return await context.ParkingSlots.Where(s => s.Status == ParkingSlotStatus.Empty).FirstOrDefaultAsync();
    }
    
    public async Task<ParkingSlot> UpdateParkingSlot(ParkingSlot slot)
    {
        context.ParkingSlots.Update(slot);
        await context.SaveChangesAsync();
        return slot;
    }
    
    public async Task<int> GetOccupiedCount()
    {
        return await context.ParkingSlots.Where(s => s.Status == ParkingSlotStatus.Occupied).CountAsync();
    }
    
    public async Task<int> GetFreeCount()
    {
        return await context.ParkingSlots.Where(s => s.Status == ParkingSlotStatus.Empty).CountAsync();
    }
    
    public async Task<ParkingSlot?> GetSlotByVehicleReg(string vehicleReg)
    {
        return await context.ParkingSlots.Where(s => s.Vehicle != null && s.Vehicle.VehicleReg == vehicleReg).FirstOrDefaultAsync();
    }
}