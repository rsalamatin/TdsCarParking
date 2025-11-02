using NSubstitute;
using TdsCarParking.Core.Contracts;
using TdsCarParking.Core.Model;
using TdsCarParking.Core.Model.Charge;
using TdsCarParking.Core.Services;

namespace TdsCarParking.UnitTests.Services;

[TestFixture]
public class ChargeServiceTests
{
    private IChargeService _chargeService;
    private IParkingChargesRepository _parkingChargesRepository;

    [SetUp]
    public void Setup()
    {
        _parkingChargesRepository = Substitute.For<IParkingChargesRepository>();
        _chargeService = new ChargeService(_parkingChargesRepository);
    }

    [Test]
    public async Task GetChargeAmount_WithInvalidVehicleType_ReturnsFailedResult()
    {
        // Arrange
        var invalidVehicleType = 999;
        var timeIn = DateTime.Now;
        var timeOut = timeIn.AddHours(2);

        // Act
        var result = await _chargeService.GetChargeAmount(invalidVehicleType, timeIn, timeOut);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
    }

    [Test]
    public async Task GetChargeAmount_WithValidInput_CalculatesCorrectCharge()
    {
        // Arrange
        var vehicleType = (int)VehicleType.Small;
        var timeIn = DateTime.Now;
        var timeOut = timeIn.AddHours(2);

        var regularCharges = new List<ParkingCharge>
        {
            new() { Value = 50m, TimeIntervalMinutes = 60, ChargeType = ChargeType.Regular }
        };

        var additionalCharges = new List<ParkingCharge>
        {
            new() { Value = 10m, TimeIntervalMinutes = 60, ChargeType = ChargeType.Additional }
        };

        _parkingChargesRepository
            .FindParkingCharges(VehicleType.Small, ChargeType.Regular)
            .Returns(regularCharges);

        _parkingChargesRepository
            .FindParkingCharges(null, ChargeType.Additional)
            .Returns(additionalCharges);

        // Act
        var result = await _chargeService.GetChargeAmount(vehicleType, timeIn, timeOut);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Payload.ChargeAmount, Is.EqualTo(120m)); // 2 hours * (50 + 10)
    }

    [Test]
    public async Task GetChargeAmount_WithPartialInterval_CalculatesFlooredCharge()
    {
        // Arrange
        var vehicleType = (int)VehicleType.Medium;
        var timeIn = DateTime.Now;
        var timeOut = timeIn.AddMinutes(90); // 1.5 hours

        var regularCharges = new List<ParkingCharge>
        {
            new() { Value = 60m, TimeIntervalMinutes = 60, ChargeType = ChargeType.Regular }
        };

        var additionalCharges = new List<ParkingCharge>
        {
            new() { Value = 20m, TimeIntervalMinutes = 60, ChargeType = ChargeType.Additional }
        };

        _parkingChargesRepository
            .FindParkingCharges(VehicleType.Medium, ChargeType.Regular)
            .Returns(regularCharges);

        _parkingChargesRepository
            .FindParkingCharges(null, ChargeType.Additional)
            .Returns(additionalCharges);

        // Act
        var result = await _chargeService.GetChargeAmount(vehicleType, timeIn, timeOut);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Payload.ChargeAmount, Is.EqualTo(80m));
    }

    [Test]
    public async Task GetChargeAmount_WithMultipleChargeRules_CalculatesCorrectTotal()
    {
        // Arrange
        var vehicleType = (int)VehicleType.Large;
        var timeIn = DateTime.Now;
        var timeOut = timeIn.AddHours(3);

        var regularCharges = new List<ParkingCharge>
        {
            new() { Value = 100m, TimeIntervalMinutes = 60, ChargeType = ChargeType.Regular },
            new() { Value = 150m, TimeIntervalMinutes = 120, ChargeType = ChargeType.Regular }
        };

        var additionalCharges = new List<ParkingCharge>
        {
            new() { Value = 30m, TimeIntervalMinutes = 60, ChargeType = ChargeType.Additional }
        };

        _parkingChargesRepository
            .FindParkingCharges(VehicleType.Large, ChargeType.Regular)
            .Returns(regularCharges);

        _parkingChargesRepository
            .FindParkingCharges(null, ChargeType.Additional)
            .Returns(additionalCharges);

        // Act
        var result = await _chargeService.GetChargeAmount(vehicleType, timeIn, timeOut);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
        Assert.That(result.Payload.ChargeAmount, Is.EqualTo(540m));
    }
}
