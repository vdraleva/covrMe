using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class Setting
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public string Value { get; set; } = null!;
}
