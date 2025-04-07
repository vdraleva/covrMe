using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Request.Payment;
using CovrMe.WebAPI.Models.Request.Speedy;
using CovrMe.WebAPI.Models.Result.Payment.Payloads;
using CovrMe.WebAPI.Models.Result.Speedy.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Mutations
{
    [ExtendObjectType(typeof(BaseMutation))]
    public class DeliveryMutations
    {
        [Authorize]
        public async Task<FindOfficePayload> FindOffice([Service] IDeliveryService deliveryService, FindOfficeInput input)
        {
            var result = await deliveryService.FindOffice(input);

            return result;
        }

        [Authorize]
        public async Task<CalculationPayload> Calculation([Service] IDeliveryService deliveryService, CalculationInput input)
        {
            var result = await deliveryService.Calculation(input);

            return result;
        }

        [Authorize]
        public async Task<ShipmentPayload> Shipment([Service] IDeliveryService deliveryService, ShipmentInput input)
        {
            var result = await deliveryService.Shipment(input);

            return result;
        }
    }
}
