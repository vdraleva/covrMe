using CovrMe.WebAPI.Models.Result.Sirma.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Sirma.Contracts
{
    public interface ISirmaVehicleService
    {
        Task<GetVehicleModelsResultModel> GetVehicleModelsByBrandId(string brandId);
    }
}
