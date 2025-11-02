using TdsCarParking.Core.Model;
using TdsCarParking.Core.Model.Charge;
using TdsCarParking.Core.Model.Parking;

namespace TdsCarParking.WebApi;

public static class InitialDataProvider
{
    public static IList<ParkingCharge> GetCharges()
    {
        return new List<ParkingCharge>
        {
            new ParkingCharge
            {
                Value = 0.10m,
                TimeIntervalMinutes = 1,
                ChargeType = ChargeType.Regular,
                CarType = VehicleType.Small
            },
            new ParkingCharge
            {
                Value = 0.20m,
                TimeIntervalMinutes = 1,
                ChargeType = ChargeType.Regular,
                CarType = VehicleType.Medium
            },
            new ParkingCharge
            {
                Value = 0.40m,
                TimeIntervalMinutes = 1,
                ChargeType = ChargeType.Regular,
                CarType = VehicleType.Large
            },
            new ParkingCharge
            {
                Value = 1.00m,
                TimeIntervalMinutes = 5,
                ChargeType = ChargeType.Additional
            }
        };
    }
    
    public static IList<ParkingSlot> GetParkingSlots()
    {
        return new List<ParkingSlot>
        {
            new ParkingSlot
            {
                SpaceNumber = 1,
            },
            new ParkingSlot
            {
                SpaceNumber = 2,
            },
            new ParkingSlot
            {
                SpaceNumber = 3,
            },
            new ParkingSlot
            {
                SpaceNumber = 4,
            },
            new ParkingSlot
            {
                SpaceNumber = 5,
            }
        };
    }
}