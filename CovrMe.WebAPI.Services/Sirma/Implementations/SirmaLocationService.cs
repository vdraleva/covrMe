using CovrMe.WebAPI.Models.Result.Sirma.City;
using CovrMe.WebAPI.Services.Caching.Contracts;
using CovrMe.WebAPI.Services.Caching.Implementations;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Sirma.Implementations
{
    public class SirmaLocationService : ISirmaLocationService
    {
        private readonly ILogger<SirmaLocationService> _logger;
        private IConfiguration _configuration;
        private readonly string _baseUrl = string.Empty;
        private readonly string _certificatePFXPath = string.Empty;
        private ISirmaAuthenticationService _sirmaAuthentication;
        private IMemoryCacheService _memoryCacheService;

        public SirmaLocationService(ILogger<SirmaLocationService> logger,IConfiguration configuration, ISirmaAuthenticationService sirmaAuthentication,
            IMemoryCacheService memoryCacheService)
        {
            _logger = logger;
            _configuration = configuration;
            _baseUrl = _configuration["Routing:SirmaBaseUrl"];
            _certificatePFXPath = _configuration["SirmaCertificatesSettingsPaths:CertificatePFXPath"];
            _sirmaAuthentication = sirmaAuthentication;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<GetCitiesResultModel> GetCitiesByMunicipalityId(string municipalityId)
        {
            var result = new GetCitiesResultModel();

            var url = $"{this._baseUrl}{GlobalConstants.SirmaGetCities}?municipality_id={municipalityId}";

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }
            try
            {
                using (client)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                    using (var response = await client.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            result = JsonConvert.DeserializeObject<GetCitiesResultModel>(apiResponse);
                            if (result.Success == 1)
                            { 
                                return result;
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<GetPostCodeResultModel> GetPostCodeByCityId(string cityId)
        {
            var result = new GetPostCodeResultModel();

            var url = $"{this._baseUrl}{GlobalConstants.SirmaGetCitiesPostCode}?city_id={cityId}";

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }

            try
            {
                using (client)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                    using (var response = await client.GetAsync(url))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            result = JsonConvert.DeserializeObject<GetPostCodeResultModel>(apiResponse);
                            if (result.Success == 1)
                            {
                                return result;
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                return result;
            }
        }
    }
}
