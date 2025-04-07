using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Models.Request.Sirma;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Sirma;
using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.InstallmentPay;
using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy;
using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search;
using CovrMe.WebAPI.Models.Result.Sirma.Vehicle;
using CovrMe.WebAPI.Services.Caching.Contracts;
using CovrMe.WebAPI.Services.Caching.Implementations;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Sirma.Implementations
{
    public class SirmaCivilInsuranceService : ISirmaCivilInsuranceService
    {
        private readonly ILogger<SirmaCivilInsuranceService> _logger;
        private IConfiguration _configuration;
        private readonly string _baseUrl = string.Empty;
        private IMemoryCacheService _memoryCacheService;
        private readonly string _certificatePFXPath = string.Empty;
        private ISirmaAuthenticationService _sirmaAuthentication;

        public SirmaCivilInsuranceService(ILogger<SirmaCivilInsuranceService> logger,IMemoryCacheService memoryCacheService,
            IConfiguration configuration, ISirmaAuthenticationService sirmaAuthentication)
        {           
            _logger = logger;
            _configuration = configuration;
            _baseUrl = _configuration["Routing:SirmaBaseUrl"];
            _certificatePFXPath = _configuration["SirmaCertificatesSettingsPaths:CertificatePFXPath"];
            _memoryCacheService = memoryCacheService;
            _sirmaAuthentication = sirmaAuthentication;
        }

        public async Task<CivilInsuranceSearchResponse> CivilInsuranceSearch(CivilInsuranceSearchRequestModel req, int? installments)
        {
            var url = this._baseUrl + GlobalConstants.SirmaCivilInsuranceSearch;
            var result = new CivilInsuranceSearchResponse();

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }

            using (client)
            {
                var jsonObject = JObject.FromObject(req);

                if (installments.HasValue)
                {
                    jsonObject["installments"] = installments.Value;
                }

                var json = jsonObject.ToString(Formatting.None);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<CivilInsuranceSearchResponse>(apiResponse);

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

        public async Task<CivilInsuranceSearchResponse> CivilInsuranceLongSearch(CivilInsuranceLongSearchRequestModel req, int? installments)
        {
            var url = this._baseUrl + GlobalConstants.SirmaCivilInsuranceLongSearch;
            var result = new CivilInsuranceSearchResponse();

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }

            using (client)
            {
                var jsonObject = JObject.FromObject(req);

                if (installments.HasValue)
                {
                    jsonObject["installments"] = installments.Value;
                }

                var json = jsonObject.ToString(Formatting.None);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<CivilInsuranceSearchResponse>(apiResponse);

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

        public async Task<CivilInsurancePolicyResponse> CivilInsurancePolicy(CivilInsurancePolicyRequestModel req)
        {
            var url = this._baseUrl + GlobalConstants.SirmaCivilInsurancePolicy;
            var result = new CivilInsurancePolicyResponse();

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<CivilInsurancePolicyResponse>(apiResponse);

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

        public async Task<CivilInsurancePolicyResponse> CivilInsuranceLongPolicy(CivilInsuranceLongPolicyRequestModel req)
        {
            var url = this._baseUrl + GlobalConstants.SirmaCivilInsuranceLongPolicy;
            var result = new CivilInsurancePolicyResponse();

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<CivilInsurancePolicyResponse>(apiResponse);

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

        public async Task<CivilInsuranceInstallmentPayResponse> CivilInsuranceInstallmentPay(CivilInsuranceInstallmentRequestModel req)
        {
            var url = this._baseUrl + GlobalConstants.SirmaCivilInsuranceNote;
            var result = new CivilInsuranceInstallmentPayResponse();

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<CivilInsuranceInstallmentPayResponse>(apiResponse);

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

        public async Task<byte[]> CivilInsuranceGetPdf(string hash)
        {           
            byte[] result = null;

            var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
            var client = new HttpClient(handler);

            var jwt = _memoryCacheService[GlobalConstants.Jwt];

            if (string.IsNullOrEmpty(jwt))
            {
                var authResult = await this._sirmaAuthentication.Authenticate();
                jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
            }

            var url = this._baseUrl + GlobalConstants.SirmaCivilInsurancePdf;
            url = $"{url}/{hash}/?token={jwt}";

            using (client)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);

                using (var response = await client.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var stream = await response.Content.ReadAsStreamAsync();

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            await stream.CopyToAsync(memoryStream);
                            result = memoryStream.ToArray();

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


        //public async Task<CivilInsuranceSearchResponse> PerformTest(CivilInsuranceSearchRequestModel req)
        //{
        //    var url = this._baseUrl + GlobalConstants.SirmaCivilInsuranceSearch;
        //    var result = new CivilInsuranceSearchResponse();

        //    var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
        //    var client = new HttpClient(handler);

        //    var jwt = _memoryCacheService[GlobalConstants.Jwt];

        //    if (string.IsNullOrEmpty(jwt))
        //    {
        //        var authResult = await this._sirmaAuthentication.Authenticate();
        //        jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
        //    }

        //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        //    var results = new List<TestResult>();
        //    Stopwatch stopwatch = new Stopwatch();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var json = JsonConvert.SerializeObject(req);
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        //        stopwatch.Start();
        //        using (var response = await client.PostAsync(url, content))
        //        {
        //            stopwatch.Stop();
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            result = JsonConvert.DeserializeObject<CivilInsuranceSearchResponse>(apiResponse);

        //            results.Add(new TestResult
        //            {
        //                TestNumber = i + 1,
        //                Success = response.StatusCode == System.Net.HttpStatusCode.OK && result.Success == 1,
        //                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
        //                ResponseCode = (int)response.StatusCode,
        //                ApiResponse = apiResponse
        //            });
        //            stopwatch.Reset();
        //        }
        //    }
        //    return result;
        //}

        //public async Task<CivilInsuranceSearchResponse> PerformLongSearchTest(CivilInsuranceLongSearchRequestModel req)
        //{
        //    var url = this._baseUrl + GlobalConstants.SirmaCivilInsuranceLongSearch;
        //    var result = new CivilInsuranceSearchResponse();

        //    var handler = Helpers.GenerateHttpClientHandler(_certificatePFXPath);
        //    var client = new HttpClient(handler);

        //    var jwt = _memoryCacheService[GlobalConstants.Jwt];

        //    if (string.IsNullOrEmpty(jwt))
        //    {
        //        var authResult = await this._sirmaAuthentication.Authenticate();
        //        jwt = string.IsNullOrEmpty(authResult.Token) ? authResult.Info.Token : authResult.Token;
        //    }

        //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        //    var results = new List<TestResult>();
        //    Stopwatch stopwatch = new Stopwatch();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var json = JsonConvert.SerializeObject(req);
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        //        stopwatch.Start();
        //        using (var response = await client.PostAsync(url, content))
        //        {
        //            stopwatch.Stop();
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            result = JsonConvert.DeserializeObject<CivilInsuranceSearchResponse>(apiResponse);

        //            results.Add(new TestResult
        //            {
        //                TestNumber = i + 1,
        //                Success = response.StatusCode == System.Net.HttpStatusCode.OK && result.Success == 1,
        //                ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
        //                ResponseCode = (int)response.StatusCode,
        //                ApiResponse = apiResponse
        //            });
        //            stopwatch.Reset();
        //        }
        //    }
        //    return result;
        //}
    }
}
