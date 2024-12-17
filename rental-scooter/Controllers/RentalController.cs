using Microsoft.AspNetCore.Mvc;
using rental_scooter.Dtos;
using rental_scooter.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace rental_scooter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService rentalService;

        public RentalController(IRentalService rentalService)
        {
            this.rentalService = rentalService;
        }

        [HttpGet]
        [Route("station-list")]
        [SwaggerOperation(Summary = "Consigue lista de estaciones con monopatines disponibles")]

        public async Task<IActionResult> GetStations()
        {
            var result = await rentalService.GetStationsWithAvailableScooters();
            return Ok(result);
        }
        [HttpGet]
        [Route("rental-history-entry-by-user/{user}")]
        [SwaggerOperation(Summary = "Consigue el historial de movimientos de un usuario")]

        public async Task<IActionResult> GetUserRentalHistoryEntries(string user)
        {
            var result = await rentalService.GetHistoryEntriesByUserIdentifier(user);
            return Ok(result);
        }

        [HttpGet]
        [Route("rental-history-entry-by-user/{user}/{startDate}/{endDate}")]
        [SwaggerOperation(Summary = "Consigue el historial de movimientos de un usuario entre dos fechas")]

        public async Task<IActionResult> GetUserRentalHistoryEntries(string user, DateTime startDate, DateTime endDate)
        {
            var result = await rentalService.GetHistoryEntriesByUserIdentifierFilteredByDate(user, startDate, endDate);
            return Ok(result);
        }

        [HttpPost]
        [Route("rent-scooter")]
        [SwaggerOperation(Summary = "Retira un scooter")]
        public async Task<IActionResult> RentScooter([FromBody] ScooterRentRequest request)
        {
            try
            {
                var result = await rentalService.RentScooter(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("return-scooter")]
        [SwaggerOperation(Summary = "Devuelve un scooter")]
        public async Task<IActionResult> ReturnScooter([FromBody] ScooterReturnRequest request)
        {
            var result = await rentalService.ReturnScooter(request);
            return Ok(result);
        }
    }
}
