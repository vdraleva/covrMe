
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CovrMe.WebAPI.Models.Result.Sirma.Municipalities;
using CovrMe.WebAPI.Models.Result.Sirma.Region;
using System.Runtime;
using Microsoft.Extensions.Configuration;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using UniqaSoapService;
using CovrMe.WebAPI.Shared.Enums;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ILogger _logger;
        private IConfiguration _configuration;
        private IReadWriteService _readWriteService;
        private ISirmaLocationService _sirmaLocationService;

        public LocationService(ILogger<LocationService> logger, IConfiguration configuration,
            IReadWriteService readWriteService, ISirmaLocationService sirmaLocationService)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._readWriteService = readWriteService;
            this._sirmaLocationService = sirmaLocationService;
        }

        public string GetRegions()
        {
            string path = this._configuration.GetSection("ReadWriteOptions").GetSection("Regions").Value;

            string regions = this._readWriteService.ReadFile(path);

            return regions;
        }

        public async Task<BaseResultModel> GetCitiesFromSirma()
        {
            var result = new BaseResultModel();

            try
            {
                var regions = this.GetRegions();

                if(!string.IsNullOrEmpty(regions))
                {
                    var regionsData = JsonConvert.DeserializeObject<GetRegionsResultModel>(regions);

                    foreach (var region in regionsData.Regions)
                    {
                        string municipalities = this.GetMunicipalityByRegionId(region.Id);

                        if (!string.IsNullOrEmpty(municipalities))
                        {
                            var municipalitiesData = JsonConvert.DeserializeObject<GetMunicipalitiesResultModel>(municipalities);

                            foreach (var municipality in municipalitiesData.Municipalities)
                            {
                                var cities = await this._sirmaLocationService.GetCitiesByMunicipalityId(municipality.Id);

                                foreach (var city in cities.Cities)
                                {
                                    var postCodeResult = await this._sirmaLocationService.GetPostCodeByCityId(city.Id);

                                    var postCode = postCodeResult.PostCodes.FirstOrDefault();

                                    if(postCode != null)
                                    {
                                        city.PostCode = postCode.Name;
                                    }
                                }

                                var citiesAsString = JsonConvert.SerializeObject(cities);
                                string cityBasePath = this._configuration.GetSection("ReadWriteOptions").GetSection("Cities").Value;

                                string cityPath = $"{cityBasePath}{municipality.Id}.json";

                                this._readWriteService.WriteTextToFile(cityPath, citiesAsString);
                            }
                        }
                    }
                }

                result.Code = (int)GeneralStatusEnum.Success;
                
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            return result;
        }

        private string GetMunicipalityByRegionId(string regionId)
        {
            string baseMunicipalityUrl = this._configuration.GetSection("ReadWriteOptions").GetSection("Municipalities").Value;

            string path = $"{baseMunicipalityUrl}{regionId}.json";

            string municipalities = this._readWriteService.ReadFile(path);

            return municipalities;
        }
    }
}
