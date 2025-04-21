using CovrMe.Models.Vehicles.Request;
using CovrMe.Models.Vehicles.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Services.Contracts
{
    public interface IVehicleService
    {
        Task<GetUserVehiclesResultModel> GetUserVehicles(string userId, HttpClient client, string jwt);
        Task<GetVehicleUsagesResultModel> GetVehicleUsages(HttpClient client, string jwt);
        Task<GetVehicleTypesResultModel> GetVehicleTypes(HttpClient client, string jwt);
        Task<string> GetVehicleModels(string brand, HttpClient client, string jwt);
        Task<VehicleResultModel> AddVehicle(AddVehicleInput addVehicleInput, HttpClient client, string jwt);
        Task<VehicleResultModel> EditVehicle(EditVehicleInput editVehicleInput, HttpClient client, string jwt);
        Task<VehicleResultModel> GetVehicleById(string vehicleId, HttpClient client, string jwt);
        Task<string> GetVehicleBodyTypes(HttpClient client, string jwt);
        Task<string> GetVehicleColors(HttpClient client, string jwt);
        Task<string> GetVehicleEngineTypes(HttpClient client, string jwt);
    }
}
