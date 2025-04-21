using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class InsuranceDocumentModel
    {
        public string? StickerId { get; set; }
        public string? GreenCardId { get; set; }
        public string? LocalOrderNumber { get; set; }
        public string? DocumentBatchId { get; set; }
        public string? PaymentFormUrl { get; set; }
    }
}
