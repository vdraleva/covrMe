﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries.Request
{
    public class CalculationInput
    {
        public int OfficeId { get; set; }
        public string? PostalCode { get; set; }
    }
}
