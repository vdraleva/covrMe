using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models
{
    public class NotificationMessage
    {
        public NotificationMessage()
        {
            this.Users = new List<string>();
        }
        public string? AlertMessage { get; set; }
        public string? TextMessage { get; set; }
        public string? InsuranceId { get; set; }
        public string? PolicyNo { get; set; }
        public int? UserId { get; set; }
        public byte Status { get; set; }
        public string? Header { get; set; }
        public List<string>? Users { get; set; }

    }
}
