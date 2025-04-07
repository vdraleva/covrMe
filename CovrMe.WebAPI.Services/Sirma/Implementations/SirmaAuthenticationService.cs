using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Models.Request.Sirma;
using CovrMe.WebAPI.Models.Result.Sirma;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X509;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Services.Caching.Contracts;
using Newtonsoft.Json.Linq;

namespace CovrMe.WebAPI.Services.Sirma.Implementations
{
    public class SirmaAuthenticationService : ISirmaAuthenticationService
    {
        private readonly SirmaAuthenticationSettings _authSettings;
        private readonly ILogger<SirmaAuthenticationService> _logger;
        private IConfiguration _configuration;
        private readonly string _baseUrl = string.Empty;
        private readonly string _certificatePFXPath = string.Empty;
        private IMemoryCacheService _memoryCacheService;
        public SirmaAuthenticationService(IOptions<SirmaAuthenticationSettings> sirmaAuthOptions, ILogger<SirmaAuthenticationService> logger,
            IConfiguration configuration, IMemoryCacheService memoryCacheService)
        {
            _authSettings = sirmaAuthOptions.Value;
            _logger = logger;
            _configuration = configuration;
            _baseUrl = _configuration["Routing:SirmaBaseUrl"];
            _certificatePFXPath = _configuration["SirmaCertificatesSettingsPaths:CertificatePFXPath"];
            _memoryCacheService = memoryCacheService;
        }

        public async Task<SirmaAuthenticationResultModel> Authenticate()
        {
            var url = this._baseUrl + GlobalConstants.SirmaAuthUrl;
            var result = new SirmaAuthenticationResultModel();

            var req = new SirmaAuthenticateRequestModel();
            req.Email = this._authSettings.Email;
            req.Password = this._authSettings.Password;

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);

            var client = new HttpClient(handler);

            try
            {
                using(client)
                {
                    var json = JsonConvert.SerializeObject(req);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            result = JsonConvert.DeserializeObject<SirmaAuthenticationResultModel>(apiResponse);

                            var jtoken = JToken.Parse(apiResponse);

                            var responseResult = jtoken["result"];

                            if (responseResult is JObject)
                            {
                                result.Info = responseResult.ToObject<SirmaAuthenticationInfoResultModel>();
                            }
                            else
                            {
                                result.Token = responseResult.ToObject<string>();
                            }

                            if (result.Success == 1)
                            {
                                string token = string.IsNullOrEmpty(result.Token) ? result.Info.Token : result.Token;
                                _memoryCacheService.Set(GlobalConstants.Jwt, token);
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
