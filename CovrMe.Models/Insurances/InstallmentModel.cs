using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class InstallmentModel
    {
        public int Number { get; set; }
        public int Index { get; set; }
        public DateTime? Date { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal GovernmentFunds { get; set; }
        public string? DateFormatted { get; set; }
        public string? PriceFormatted { get; set; }
        public string? Text { get; set; }
        public string? ShortText { get; set; }
    }
}
