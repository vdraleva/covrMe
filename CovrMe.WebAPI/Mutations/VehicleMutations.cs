using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Request.Vehicles;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Vehicles.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Mutations
{
    [ExtendObjectType(typeof(BaseMutation))]
    public class VehicleMutations
    {
        [Authorize]
        public async Task<AddVehiclePayload> AddVehicle([Service] IVehicleService vehicleService, AddVehicleInput input)
        {
            var result = new AddVehiclePayload();
            var vehicle = await vehicleService.CreateVehicleAsync(input);

            result.Vehicle = vehicle;
            return result;
        }

        [Authorize]
        public async Task<EditVehiclePayload> EditVehicle([Service] IVehicleService vehicleService, EditVehicleInput input)
        {
            var result = new EditVehiclePayload();
            var vehicle = await vehicleService.EditUserVehicle(input);

            result.Vehicle = vehicle;
            return result;
        }

        [Authorize]
        public VehicleModelsPayload VehicleModels([Service] IVehicleService vehicleService, VehicleModelsInput input)
        {
            var result = new VehicleModelsPayload();
            var models = vehicleService.GetVehicleModelsByBrandName(input.Brand);

            result.Models = models;
            return result;
        }

    }
}
