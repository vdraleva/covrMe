using CovrMe.Models.Locations.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Services.Contracts
{
    public interface ILocationService
    {
        Task<GetCountriesResultModel> GetCountries(HttpClient client, string jwt);
        Task<GetRegionsByCountryResultModel> GetRegionsByCountry(string countryId, HttpClient client, string jwt);
        Task<GetMunicipalityResultModel> GetMunicipalityByRegionId(string regionId, HttpClient client, string jwt);
        Task<GetCityResultModel> GetCityByMunicipalityId(string municipalityId, HttpClient client, string jwt);
    }
}
