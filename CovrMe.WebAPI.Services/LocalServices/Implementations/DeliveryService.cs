using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Data;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CovrMe.WebAPI.Services.Speedy.Contracts;
using CovrMe.WebAPI.Models.Result.Speedy.Payloads;
using CovrMe.WebAPI.Models.Request.Speedy;
using CovrMe.WebAPI.Models.Result.Speedy;
using CovrMe.WebAPI.Models.Result.Users;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private ISpeedyShipmentService _speedyShipmentService;
        private IErrorService _errorService;

        public DeliveryService(ILogger<DeliveryService> logger, IConfiguration configuration,
            ISpeedyShipmentService speedyShipmentService, IErrorService errorService)
        {
            this._speedyShipmentService = speedyShipmentService;
            this._logger = logger;
            this._configuration = configuration;
            _errorService = errorService;
        }

        public async Task<FindOfficePayload> FindOffice(FindOfficeInput req)
        {
            var result = new FindOfficePayload();
            try
            {
                var offices = await this._speedyShipmentService.FindOffice(req);

                result.Offices = offices;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                this._logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<CalculationPayload> Calculation(CalculationInput req)
        {
            var result = new CalculationPayload();
            try
            {
                var calcResponse = new SpeedyCalculatedPriceResultModel();

                if (!string.IsNullOrEmpty(req.PostalCode))
                {
                    int code = int.Parse(req.PostalCode);
                    calcResponse = await this._speedyShipmentService.AddressCalculation(code);
                }
                else
                {
                    calcResponse = await this._speedyShipmentService.OfficeCalculation(req.OfficeId);
                }

                result.Calculations = calcResponse.Calculations;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                this._logger.LogError(ex.Message);
                return result;
            }
        }

        public async Task<ShipmentPayload> Shipment(ShipmentInput req)
        {
            var result = new ShipmentPayload();
            try
            {
                var shipResponse = new CreateShipmentResultModel();

                if (req.OfficeId != 0)
                {
                    shipResponse = await this._speedyShipmentService.ShipmentToOffice(req.Phone, req.Names, req.Email, req.OfficeId);
                }
                else
                {
                    shipResponse = await this._speedyShipmentService.ShipmentToAddress(req.Phone, req.Names, req.Email,
                        req.PostalCode, req.Street, req.BlockNo, req.Entrance, req.Floor, req.Appartment, req.Info);
                }

                if (string.IsNullOrEmpty(shipResponse.Id))
                {
                    Task.Run(async () => { await this.SendSpeedyErrorEmail(req.Names, req.Phone, req.PolicyNo); });
                }

                result.DeliveryDeadline = shipResponse.DeliveryDeadline;
                result.Id = shipResponse.Id;
                return result;
            }
            catch (Exception ex)
            {
                this.LogError(ex.Message, $"{ex.StackTrace}");
                this._logger.LogError(ex.Message);
                Task.Run(async () => { await this.SendSpeedyErrorEmail(req.Names, req.Phone, req.PolicyNo); });
                return result;
            }
        }

        private async Task SendSpeedyErrorEmail(string names,string phone, string policyNo)
        {
            this._errorService.SendSpeedyErrorEmail(policyNo,names, phone);
        }

        private void LogError(string exMessage, string trace)
        {
            string message = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm")} {trace} - {exMessage}";
            string fileName = $"{DateTime.Now.ToString("dd-MM-yyyy")}.txt";

            this._errorService.LogError(message, fileName);
        }
    }
}
