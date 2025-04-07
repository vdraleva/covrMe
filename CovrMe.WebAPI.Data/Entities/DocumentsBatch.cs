using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class DocumentsBatch
{
    public Guid Id { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsActive { get; set; }

    public int DocsCount { get; set; }

    public Guid InsuranceCompanyId { get; set; }

    public bool IsCompleted { get; set; }

    public virtual ICollection<GreenCard> GreenCards { get; set; } = new List<GreenCard>();

    public virtual InsuranceCompany InsuranceCompany { get; set; } = null!;

    public virtual ICollection<Sticker> Stickers { get; set; } = new List<Sticker>();
}
