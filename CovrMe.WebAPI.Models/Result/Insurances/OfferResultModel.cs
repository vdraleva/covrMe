using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances
{
    public class OfferResultModel
    {
        public string? OfferId { get; set; }
        public string? ErrorId { get; set; }
        public string? Message { get; set; }
    }
}
