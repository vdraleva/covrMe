using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Documents
{
    public class CreateGreenCardNumberInput
    {
        public string BatchId { get; set; }
        public string GreenCardNumber { get; set; }
    }
}
