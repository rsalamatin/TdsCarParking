using System.ComponentModel.DataAnnotations;

namespace TdsCarParking.Core.Model.Charge;

public class ParkingCharge
{
    public int Id { get; set; }
    public decimal Value { get; set; }
    public int TimeIntervalMinutes { get; set; }
    public ChargeType ChargeType { get; set; }
    public VehicleType? CarType { get; set; }
}