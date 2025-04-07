using CovrMe.WebAPI.Models.Request.Payment;
using CovrMe.WebAPI.Models.Result.Payment.Payloads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentInfoAsync(string localOrderNumber, string dskOrderNumber);
        Task<string> UpdatePaymentInfo(string dskOrderNumber, int status, string operation);
        T GetPaymentInfoByOrderNumber<T>(string localOrderNumber);
        Task<CreatePaymentPayload> CreatePayment(CreatePaymentInput input);

        Task<CheckPaymentStatusPayload> CheckPaymentStatus(string localOrderNumberId);
    }
}
