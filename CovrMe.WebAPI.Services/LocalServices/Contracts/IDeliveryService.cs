using CovrMe.WebAPI.Models.Request.Speedy;
using CovrMe.WebAPI.Models.Result.Speedy.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IDeliveryService
    {
        Task<FindOfficePayload> FindOffice(FindOfficeInput req);
        Task<CalculationPayload> Calculation(CalculationInput req);
        Task<ShipmentPayload> Shipment(ShipmentInput req);
    }
}
