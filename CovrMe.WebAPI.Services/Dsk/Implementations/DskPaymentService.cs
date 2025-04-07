using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Models.Request.Dsk;
using CovrMe.WebAPI.Models.Request.Sirma;
using CovrMe.WebAPI.Models.Result.Dsk;
using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search;
using CovrMe.WebAPI.Services.Caching.Implementations;
using CovrMe.WebAPI.Services.Dsk.Contracts;
using CovrMe.WebAPI.Services.Sirma.Implementations;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Dsk.Implementations
{
    public class DskPaymentService : IDskPaymentService
    {
        private IConfiguration _configuration;
        private readonly DskAuthenticationSettings _authSettings;
        private readonly ILogger<DskPaymentService> _logger;
        private readonly string _baseUrl;
        public DskPaymentService(IOptions<DskAuthenticationSettings> dskAuthOptions, ILogger<DskPaymentService> logger,
            IConfiguration configuration)
        {
            _authSettings = dskAuthOptions.Value;
            _logger = logger;
            _configuration = configuration;
            _baseUrl = _configuration["Routing:DskPaymentUrl"];
        }

        public async Task<DskPaymentResponseModel> DskPayment(DskPaymentRequestModel input)
        {
            var url = this._baseUrl;
            var result = new DskPaymentResponseModel();

            var req = new Dictionary<string, string>();
            req.Add("amount", input.Amount.ToString());
            req.Add("currency", input.Currency.ToString());
            req.Add("language", input.Language);
            req.Add("orderNumber", input.OrderNumber);
            req.Add("returnUrl", this._authSettings.ReturnUrl);
            req.Add("userName", this._authSettings.UserName);
            req.Add("password", this._authSettings.Password);
            req.Add("description", input.Description);
            req.Add("email", input.Email);
            req.Add("phone", input.Phone);
            req.Add("clientId", input.UserId);

            ServicePointManager.SecurityProtocol =  SecurityProtocolType.SystemDefault;

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(req);
                var content = new FormUrlEncodedContent(req);

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<DskPaymentResponseModel>(apiResponse);

                        if (!string.IsNullOrEmpty(result.OrderId))
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
    }
}
