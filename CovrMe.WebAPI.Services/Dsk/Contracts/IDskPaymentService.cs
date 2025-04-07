using CovrMe.WebAPI.Models.Request.Dsk;
using CovrMe.WebAPI.Models.Result.Dsk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Dsk.Contracts
{
    public interface IDskPaymentService
    {
        Task<DskPaymentResponseModel> DskPayment(DskPaymentRequestModel req);
    }
}
