using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class UserPasswordResetCode
{
    public Guid Id { get; set; }

    public string AspNetUserId { get; set; } = null!;

    public string Code { get; set; } = null!;
}
