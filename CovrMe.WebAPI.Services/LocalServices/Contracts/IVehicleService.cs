using CovrMe.WebAPI.Models.Request.Vehicles;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Vehicles;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IVehicleService
    {
        IQueryable<T> GetVehicles<T>();
        Task<VehicleResultModel> CreateVehicleAsync(AddVehicleInput req);
        IQueryable<VehicleResultModel> GetVehicleByUserId(string userId);
        Task<VehicleResultModel> EditUserVehicle(EditVehicleInput req);
        T GetVehicleByPlateNumber<T>(string vehiclePlateNumber, string userId);
        VehicleResultModel GetVehicleById(string id);
        IQueryable<VehicleResultModel> GetVehicleByVehicleId(string vehicleId);
        string GetVehiclesBodyTypes(int hashCode);
        string GetVehiclesBodyTypes();
        string GetVehiclesEngineTypes(int hashCode);
        string GetVehiclesEngineTypes();
        string GetVehiclesColors(int hashCode);
        string GetVehiclesColors();
        Task<BaseResultModel> GetVehicleModelsFromSirma();
        string GetVehicleModelsByBrandName(string brand);
        Task DeleteUserVehicles(string userId);
    }
}
