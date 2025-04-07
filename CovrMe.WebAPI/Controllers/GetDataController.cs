using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CovrMe.WebAPI.Controllers
{
    public class GetDataController : Controller
    {
        private IConfiguration _configuration;
        private ILocationService _locationService;
        private IVehicleService _vehicleService;

        public GetDataController(IConfiguration configuration, IVehicleService vehicleService,
            ILocationService locationService, IUserService userService)
        {
            this._configuration = configuration;
            this._vehicleService = vehicleService;
            this._locationService = locationService;
        }

        [HttpGet]
        [RequireHttps]
        [Route(GlobalConstants.GetLocationsData)]
        public async Task<IActionResult> GetLocationsData()
        {
            var result = await this._locationService.GetCitiesFromSirma();

            if (result != null)
            {
                return Ok(result);
            }

            else
            {
                return BadRequest();
            }

        }

        [HttpGet]
        [RequireHttps]
        [Route(GlobalConstants.GetVehicleData)]
        public async Task<IActionResult> GetVehicleData()
        {
            var result = await this._vehicleService.GetVehicleModelsFromSirma();

            if (result != null)
            {
                return Ok(result);
            }

            else
            {
                return BadRequest();
            }

        }
    }
}
