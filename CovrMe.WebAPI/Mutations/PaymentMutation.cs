using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Request.Payment;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result.Payment.Payloads;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Mutations
{
    [ExtendObjectType(typeof(BaseMutation))]
    public class PaymentMutation
    {
        [Authorize]
        public async Task<CreatePaymentPayload> CreatePayment([Service] IPaymentService paymentService, CreatePaymentInput input)
        {
            var result = await paymentService.CreatePayment(input);

            return result;
        }

        [Authorize]
        public async Task<CheckPaymentStatusPayload> CheckPaymentStatus([Service] IPaymentService paymentService, CheckPaymentStatusInput input)
        {
            var result = await paymentService.CheckPaymentStatus(input.LocalOrderId);

            return result;
        }
    }
}
