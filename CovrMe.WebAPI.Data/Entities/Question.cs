using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class Question
{
    public Guid Id { get; set; }

    public int QuestionId { get; set; }

    public string? Answer { get; set; }

    public Guid MyThingsInsuranceId { get; set; }

    public virtual MyThingsInsurance MyThingsInsurance { get; set; } = null!;
}
