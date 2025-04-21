using CovrMe.Models;
using CovrMe.Models.Payments.Request;
using CovrMe.Models.Payments.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Services.Contracts
{
    public interface IPaymentService
    {
        Task<CreatePaymentResultModel> Payment(CreatePaymentInput createPaymentInput, string jwt, HttpClient client);

        Task CheckPaymentStatusSubscription(string localOrderId, string jwt, HttpClient client);

        Task<CheckPaymentStatusResultModel> CheckPaymentStatus(string localOrderId, string jwt, HttpClient client);
    }
}
