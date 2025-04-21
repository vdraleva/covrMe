using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries.Request
{
    public class ShipmentInput
    {
        public string? PolicyNo { get; set; }
        public int OfficeId { get; set; }
        public string? Names { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? BlockNo { get; set; }
        public string? Entrance { get; set; }
        public string? Floor { get; set; }
        public string? Appartment { get; set; }
        public string? Info { get; set; }
    }
}
