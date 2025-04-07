using CovrMe.WebAPI.Models.Result.Sirma.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Sirma.Contracts
{
    public interface ISirmaLocationService
    {
        Task<GetCitiesResultModel> GetCitiesByMunicipalityId(string municipalityId);
        Task<GetPostCodeResultModel> GetPostCodeByCityId(string cityId);
    }
}
