using NSubstitute;
using TdsCarParking.Core.Contracts;
using TdsCarParking.Core.Model;
using TdsCarParking.Core.Model.Parking;
using TdsCarParking.Core.Services;

namespace TdsCarParking.UnitTests.Services;

[TestFixture]
public class ParkingServiceTests
{
    private IParkingService _parkingService;
    private IParkingSlotRepository _parkingSlotRepository;

    [SetUp]
    public void Setup()
    {
        _parkingSlotRepository = Substitute.For<IParkingSlotRepository>();
        _parkingService = new ParkingService(_parkingSlotRepository);
    }

    [Test]
    public async Task ParkCar_WithValidInput_ReturnsSuccessResult()
    {
        // Arrange
        var vehicleReg = "ABC123";
        var vehicleType = (int)VehicleType.Small;
        var emptySlot = new ParkingSlot { SpaceNumber = 1 };
        
        _parkingSlotRepository.GetFirstEmpty().Returns(emptySlot);

        // Act
        var result = await _parkingService.ParkCar(vehicleReg, vehicleType);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Payload.VehicleReg, Is.EqualTo(vehicleReg));
            Assert.That(result.Payload.SpaceNumber, Is.EqualTo(1));
        });
        
        await _parkingSlotRepository.Received(1).UpdateParkingSlot(Arg.Is<ParkingSlot>(
            slot => slot.Vehicle != null && 
                   slot.Vehicle.VehicleReg == vehicleReg && 
                   slot.Vehicle.Type == (VehicleType)vehicleType));
    }

    [Test]
    public async Task ParkCar_WithInvalidVehicleType_ReturnsFailedResult()
    {
        // Arrange
        var vehicleReg = "ABC123";
        var invalidVehicleType = 999;

        // Act
        var result = await _parkingService.ParkCar(vehicleReg, invalidVehicleType);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        await _parkingSlotRepository.DidNotReceive().UpdateParkingSlot(Arg.Any<ParkingSlot>());
    }

    [Test]
    public async Task ParkCar_WithEmptyRegistration_ReturnsFailedResult()
    {
        // Arrange
        var vehicleReg = string.Empty;
        var vehicleType = (int)VehicleType.Small;

        // Act
        var result = await _parkingService.ParkCar(vehicleReg, vehicleType);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        await _parkingSlotRepository.DidNotReceive().UpdateParkingSlot(Arg.Any<ParkingSlot>());
    }

    [Test]
    public async Task ParkCar_WhenNoEmptySlots_ReturnsFailedResult()
    {
        // Arrange
        var vehicleReg = "ABC123";
        var vehicleType = (int)VehicleType.Small;
        
        _parkingSlotRepository.GetFirstEmpty().Returns((ParkingSlot?)null);

        // Act
        var result = await _parkingService.ParkCar(vehicleReg, vehicleType);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        await _parkingSlotRepository.DidNotReceive().UpdateParkingSlot(Arg.Any<ParkingSlot>());
    }

    [Test]
    public async Task UnparkCar_WithValidRegistration_ReturnsSuccessResult()
    {
        // Arrange
        var vehicleReg = "ABC123";
        var vehicleType = VehicleType.Small;
        var timeIn = DateTime.UtcNow.AddHours(-2);
        var slot = new ParkingSlot 
        { 
            SpaceNumber = 1,
            Vehicle = new Vehicle
            {
                VehicleReg = vehicleReg,
                Type = vehicleType,
                TimeIn = timeIn
            },
            Status = ParkingSlotStatus.Occupied
        };

        _parkingSlotRepository.GetSlotByVehicleReg(vehicleReg).Returns(slot);

        // Act
        var result = await _parkingService.UnparkCar(vehicleReg);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Payload.VehicleReg, Is.EqualTo(vehicleReg));
            Assert.That(result.Payload.VehileType, Is.EqualTo((int)vehicleType));
            Assert.That(result.Payload.TimeIn, Is.EqualTo(timeIn));
            Assert.That(result.Payload.TimeOut, Is.GreaterThan(timeIn));
        });

        await _parkingSlotRepository.Received(1).UpdateParkingSlot(Arg.Is<ParkingSlot>(
            s => s.Vehicle == null));
    }

    [Test]
    public async Task UnparkCar_WithEmptyRegistration_ReturnsFailedResult()
    {
        // Arrange
        var vehicleReg = string.Empty;

        // Act
        var result = await _parkingService.UnparkCar(vehicleReg);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        await _parkingSlotRepository.DidNotReceive().GetSlotByVehicleReg(Arg.Any<string>());
        await _parkingSlotRepository.DidNotReceive().UpdateParkingSlot(Arg.Any<ParkingSlot>());
    }

    [Test]
    public async Task UnparkCar_WithNonExistentVehicle_ReturnsFailedResult()
    {
        // Arrange
        var vehicleReg = "ABC123";
        _parkingSlotRepository.GetSlotByVehicleReg(vehicleReg).Returns((ParkingSlot?)null);

        // Act
        var result = await _parkingService.UnparkCar(vehicleReg);

        // Assert
        Assert.That(result.IsSuccess, Is.False);
        await _parkingSlotRepository.DidNotReceive().UpdateParkingSlot(Arg.Any<ParkingSlot>());
    }

    [Test]
    public async Task GetParkingStatus_ReturnsCorrectCounts()
    {
        // Arrange
        var occupiedCount = 5;
        var freeCount = 10;

        _parkingSlotRepository.GetOccupiedCount().Returns(occupiedCount);
        _parkingSlotRepository.GetFreeCount().Returns(freeCount);

        // Act
        var result = await _parkingService.GetParkingStatus();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Payload.OccupiedSlots, Is.EqualTo(occupiedCount));
            Assert.That(result.Payload.FreeSlots, Is.EqualTo(freeCount));
        });
    }
}
