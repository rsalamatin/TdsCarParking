namespace TdsCarParking.WebApi.Controllers;

public class ParkVehicleRequest
{
    public string VehicleReg { get; set; } = string.Empty;
    public int VehicleType { get; set; }
}