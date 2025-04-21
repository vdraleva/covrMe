using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries
{
    public class InsuranceDeliveryModel
    {
        public string? PolicyNo { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? Neighbourhood { get; set; }
        public int OfficeId { get; set; }
        public string? Email { get; set; }
        public decimal DeliveryPrice { get; set; }
        public string? PostalCode { get; set; }
        public string? Street { get; set; }
        public string? BlockNo { get; set; }
        public string? Entrance { get; set; }
        public string? Floor { get; set; }
        public string? Appartment { get; set; }
        public string? Info { get; set; }

    }
}
