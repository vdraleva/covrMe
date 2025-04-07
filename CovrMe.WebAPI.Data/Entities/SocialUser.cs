using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class SocialUser
{
    public string SocialUserId { get; set; } = null!;

    public string AspUserId { get; set; } = null!;

    public string Provider { get; set; } = null!;

    public byte ProviderId { get; set; }
}
