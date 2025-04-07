using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Dsk
{
    public class DskPaymentRequestModel
    {
        public long Amount { get; set; }
        public int Currency { get; set; }
        public string? Language { get; set; }
        public string? OrderNumber { get; set; }
        public string? ReturnUrl { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Description { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? UserId { get; set; }
    }
}
