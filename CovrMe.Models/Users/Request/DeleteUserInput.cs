﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Users.Request
{
    public class DeleteUserInput
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
    }
}
