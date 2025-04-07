using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Models.Request.Dsk;
using CovrMe.WebAPI.Models.Request.Speedy;
using CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels;
using CovrMe.WebAPI.Models.Result.Dsk;
using CovrMe.WebAPI.Models.Result.Speedy;
using CovrMe.WebAPI.Models.Result.Speedy.Payloads;
using CovrMe.WebAPI.Services.Dsk.Implementations;
using CovrMe.WebAPI.Services.Speedy.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Speedy.Implementations
{
    public class SpeedyShipmentService : ISpeedyShipmentService
    {
        private IConfiguration _configuration;
        private readonly SpeedyAuthenticationSettings _authSettings;
        private readonly ILogger<SpeedyShipmentService> _logger;
        private readonly string _baseUrl;

        public SpeedyShipmentService(IOptions<SpeedyAuthenticationSettings> dskAuthOptions, ILogger<SpeedyShipmentService> logger,
            IConfiguration configuration)
        {
            _authSettings = dskAuthOptions.Value;
            _logger = logger;
            _configuration = configuration;
            _baseUrl = _configuration["Routing:SpeedyBaseUrlUrl"];
        }

        public async Task<List<SpeedyOfficeModel>> FindOffice(FindOfficeInput input)
        {
            var result = new FindOfficeResultModel();

            var url = this._baseUrl + GlobalConstants.SpeedyFindOfficeUrl;

            var req = new SpeedyFindOfficeRequestModel();

            req.Language = GlobalConstants.Language;
            req.UserName = this._authSettings.UserName;
            req.Password = this._authSettings.Password;
            req.CountryId = GlobalConstants.CountryId;

            if (string.IsNullOrEmpty(input.Neighborhood))
            {
                req.KeyWord = input.City;
            }
            else
            {
                req.KeyWord = $"{input.City} - {input.Neighborhood}";
            }
            

            var client = new HttpClient();

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<FindOfficeResultModel>(apiResponse);

                        return result.Offices;
                    }
                    else
                    {
                        return result.Offices;
                    }
                }
            }
        }

        public async Task<SpeedyCalculatedPriceResultModel> AddressCalculation(int postalCode)
        {
            var result = new SpeedyCalculatedPriceResultModel();

            var url = this._baseUrl + GlobalConstants.SpeedyCalculation;

            var req = new AddressCalculationRequestModel();

            var recipient = new SpeedyCalculationAddressRecipientModel();
            var addressLocation = new SpeedyAddressLocationModel();
            var service = new SpeedyServiceModel();
            var obpd = new SpeedyObpdModel();
            var additionalService = new SpeedyAdditionalServicesModel();
            var calcContent = new SpeedyContentModel();
            var payment = new SpeedyPaymentModel();
                      
            addressLocation.PostCode = postalCode;
            recipient.AddressLocation = addressLocation;
            additionalService.Obpd = obpd;
            service.AdditionalServices = additionalService;

            req.UserName = this._authSettings.UserName;
            req.Password = this._authSettings.Password;
            req.Recipient = recipient;
            req.Service = service;
            req.Content = calcContent;
            req.Payment = payment;

            var client = new HttpClient();

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<SpeedyCalculatedPriceResultModel>(apiResponse);
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
            }
        }

        public async Task<SpeedyCalculatedPriceResultModel> OfficeCalculation(int officeId)
        {
            var result = new SpeedyCalculatedPriceResultModel();

            var url = this._baseUrl + GlobalConstants.SpeedyCalculation;

            var req = new OfficeCalculationRequestModel();

            var recipient = new SpeedyCalculationOfficeRecipientModel();
            var service = new SpeedyServiceModel();
            var obpd = new SpeedyObpdModel();
            var additionalService = new SpeedyAdditionalServicesModel();
            var calcContent = new SpeedyContentModel();
            var payment = new SpeedyPaymentModel();
            
            additionalService.Obpd = obpd;
            service.AdditionalServices = additionalService;

            recipient.PickupOfficeId = officeId;
            req.UserName = this._authSettings.UserName;
            req.Password = this._authSettings.Password;
            req.Recipient = recipient;
            req.Service = service;
            req.Content = calcContent;
            req.Payment = payment;

            var client = new HttpClient();

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<SpeedyCalculatedPriceResultModel>(apiResponse);
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
            }
        }

        public async Task<CreateShipmentResultModel> ShipmentToOffice(string phone, string names, string email, int officeId)
        {
            var result = new CreateShipmentResultModel();

            var url = this._baseUrl + GlobalConstants.SpeedyShipment;

            var req = new OfficeShipmentRequestModel();

            var service = new SpeedyServiceModel();
            var obpd = new SpeedyObpdModel();
            var additionalService = new SpeedyAdditionalServicesModel();
            var shipmentContent = new SpeedyContentModel();
            var payment = new SpeedyPaymentModel();
            var phoneModel = new SpeedyPhoneModel();
            var recipient = new SpeedyOfficeShipmentRecipientModel();

            additionalService.Obpd = obpd;
            service.AdditionalServices = additionalService;           
            phoneModel.Phone = phone;
            
            recipient.PhoneNumber = phoneModel;
            recipient.Names = names;
            recipient.Email = email;
            recipient.PickupOfficeId = officeId;

            req.UserName = this._authSettings.UserName;
            req.Password = this._authSettings.Password;
            req.Recipient = recipient;
            req.Service = service;
            req.Content = shipmentContent;
            req.Payment = payment;

            var client = new HttpClient();

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<CreateShipmentResultModel>(apiResponse);
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
            }
        }

        public async Task<CreateShipmentResultModel> ShipmentToAddress(string phone, string names, string email,string postalCode,string street, string blockNo,string entrance, string floor,  string app, string info)
        {
            var result = new CreateShipmentResultModel();

            var url = this._baseUrl + GlobalConstants.SpeedyShipment;

            var req = new AddressShipmentRequestModel();

            var recipient = new SpeedyAddressShipmentRecipientModel();
            var service = new SpeedyServiceModel();
            var obpd = new SpeedyObpdModel();
            var additionalService = new SpeedyAdditionalServicesModel();
            var shipmentContent = new SpeedyContentModel();
            var payment = new SpeedyPaymentModel();
            var phoneModel = new SpeedyPhoneModel();
            var address = new SpeedyShipmentAddressModel();

            additionalService.Obpd = obpd;
            service.AdditionalServices = additionalService;
            phoneModel.Phone = phone;

            address.PostCode = postalCode;
            address.StreetNo = street;
            address.BlockNo = blockNo;
            address.EntranceNo = entrance;
            address.FloorNo = floor;
            address.ApartmentNo = app;
            address.AddressNote = info;

            recipient.PhoneNumber = phoneModel;
            recipient.Names = names;
            recipient.Email = email;
            recipient.Address = address;

            req.UserName = this._authSettings.UserName;
            req.Password = this._authSettings.Password;
            req.Recipient = recipient;
            req.Service = service;
            req.Content = shipmentContent;
            req.Payment = payment;

            var client = new HttpClient();

            using (client)
            {
                var json = JsonConvert.SerializeObject(req);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        result = JsonConvert.DeserializeObject<CreateShipmentResultModel>(apiResponse);
                        return result;
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
