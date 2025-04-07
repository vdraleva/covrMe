﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models
{
    public class DeviceInformation
    {
        public string? InstallationId { get; set; }

        public string? Platform { get; set; }

        public string? PushChannel { get; set; }
    }
}
