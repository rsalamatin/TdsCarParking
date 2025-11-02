using Microsoft.AspNetCore.Mvc;
using TdsCarParking.Core.Contracts;


namespace TdsCarParking.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ParkingController(IParkingService parkingService, IChargeService chargeService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ParkVehicle([FromBody] ParkVehicleRequest request)
    {
        var result = await parkingService.ParkCar(request.VehicleReg, request.VehicleType);
        if (!result.IsSuccess)
        {
            return BadRequest("Could not park vehicle");
        }

        return Ok(new
        {
            result.Payload.VehicleReg,
            result.Payload.SpaceNumber,
            result.Payload.TimeIn
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetParkingStatus()
    {
        var status = await parkingService.GetParkingStatus();
        return Ok(new
        {
            AvailableSpaces = status.Payload.FreeSlots,
            OccupiedSpaces = status.Payload.OccupiedSlots
        });
    }

    [HttpPost("exit")]
    public async Task<IActionResult> ExitParking([FromBody] ExitParkingRequest request)
    {
        var unparkResult = await parkingService.UnparkCar(request.VehicleReg);
        if (!unparkResult.IsSuccess)
        {
            return BadRequest("Vehicle not found");
        }

        var chargeResult = await chargeService.GetChargeAmount(
            unparkResult.Payload.VehileType,
            unparkResult.Payload.TimeIn,
            unparkResult.Payload.TimeOut);

        if (!chargeResult.IsSuccess)
        {
            return BadRequest("Could not calculate charge");
        }

        return Ok(new
        {
            unparkResult.Payload.VehicleReg,
            VehicleCharge = decimal.ToDouble(chargeResult.Payload.ChargeAmount),
            unparkResult.Payload.TimeIn,
            unparkResult.Payload.TimeOut
        });
    }
}