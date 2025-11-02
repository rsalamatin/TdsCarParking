using System.ComponentModel.DataAnnotations;

namespace TdsCarParking.Core.Model.Parking;

public class ParkingSlot
{
    public int Id { get; set; }
    public int SpaceNumber { get; set; }
    public Vehicle? Vehicle { get; set; }
    public ParkingSlotStatus Status { get; set; } = ParkingSlotStatus.Empty;

    public void SetVehicle(string vehicleReg, VehicleType type)
    {
        Vehicle = new Vehicle
        {
            VehicleReg = vehicleReg,
            TimeIn = DateTime.UtcNow,
            Type = type,
        };
        Status = ParkingSlotStatus.Occupied;
    }
    
    public void RemoveVehicle()
    {
        Status = ParkingSlotStatus.Empty;
        Vehicle = null;
    }

}