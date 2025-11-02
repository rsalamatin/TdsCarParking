namespace TdsCarParking.Core.Model.Parking;

public class Vehicle
{
    public string VehicleReg { get; set; }
    public DateTime TimeIn { get; set; }
    public VehicleType Type { get; set; }
}