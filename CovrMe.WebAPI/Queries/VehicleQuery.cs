
using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Vehicles;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Queries
{
    [ExtendObjectType(typeof(BaseQuery))]
    public class VehicleQuery
    {
        [GraphQLName("vehicles")]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        [Authorize]
        public IQueryable<VehicleResultModel> GetVehicleByUserId([Service] IVehicleService vehicleService, string userId)
        {
            var vehicle = vehicleService.GetVehicleByUserId(userId);

            return vehicle;
        }


        [GraphQLName("vehicleByVehicleId")]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        [Authorize]
        public IQueryable<VehicleResultModel> VehicleByVehicleId([Service] IVehicleService vehicleService, string vehicleId)
        {
            var vehicle = vehicleService.GetVehicleByVehicleId(vehicleId);

            return vehicle;
        }
    }
}
