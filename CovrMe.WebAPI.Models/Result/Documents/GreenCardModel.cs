using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Documents
{
    public class GreenCardModel
    {
        public string Id { get; set; }
        public string DocumentsBatchId { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string GreenCardNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsUsed { get; set; }
        public int UsedGreenCards{ get; set; }
        public int TotalGreenCards { get; set; }
    }
}
