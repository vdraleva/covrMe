using CovrMe.WebAPI.Models.Result;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface ILocationService
    {
        string GetRegions();
        Task<BaseResultModel> GetCitiesFromSirma();
    }
}
