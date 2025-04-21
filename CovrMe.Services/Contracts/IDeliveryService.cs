using CovrMe.Models.Deliveries;
using CovrMe.Models.Deliveries.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Services.Contracts
{
    public interface IDeliveryService
    {
        Task<List<SpeedyOfficeModel>> FindOffice(string city, string neighborhood, string jwt, HttpClient client);
        Task<SpeedyPriceModel> Calculation(string postalCode, int officeId, string jwt, HttpClient client);
        Task<string> Shipment(InsuranceDeliveryModel req, string jwt, HttpClient client);
    }
}
