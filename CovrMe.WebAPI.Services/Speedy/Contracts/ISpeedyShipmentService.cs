using CovrMe.WebAPI.Models.Request.Speedy;
using CovrMe.WebAPI.Models.Result.Speedy;
using CovrMe.WebAPI.Models.Result.Speedy.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Speedy.Contracts
{
    public interface ISpeedyShipmentService
    {
        Task<List<SpeedyOfficeModel>> FindOffice(FindOfficeInput input);
        Task<SpeedyCalculatedPriceResultModel> AddressCalculation(int postalCode);

        Task<SpeedyCalculatedPriceResultModel> OfficeCalculation(int officeId);
        Task<CreateShipmentResultModel> ShipmentToOffice(string phone, string names, string email, int officeId);
        Task<CreateShipmentResultModel> ShipmentToAddress(string phone, string names, string email, string postalCode, string street, string blockNo, string entrance, string floor, string app, string info);
    }
}
