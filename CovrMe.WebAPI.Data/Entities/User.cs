using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public int? DrivingExperience { get; set; }

    public string? FirstName { get; set; }

    public string? SurName { get; set; }

    public string? LastName { get; set; }

    public string? LatinFirstName { get; set; }

    public string? LatinLastName { get; set; }

    public string? UiNumber { get; set; }

    public string? VatNumber { get; set; }

    public string? Address { get; set; }

    public string? AspNetUserId { get; set; }

    public string? PhoneNumber { get; set; }

    public Guid? ParentUserId { get; set; }

    public string? Email { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? LatinSurName { get; set; }

    public bool? IsDeleted { get; set; }

    public string? CompanyName { get; set; }

    public string? LatinAddress { get; set; }

    public string? LatinCompanyName { get; set; }

    public string? CountryId { get; set; }

    public string? MuniciplalityId { get; set; }

    public string? CityId { get; set; }

    public string? RegionId { get; set; }

    public string? PostCode { get; set; }

    public virtual ICollection<DeliveryInfo> DeliveryInfos { get; set; } = new List<DeliveryInfo>();

    public virtual ICollection<User> InverseParentUser { get; set; } = new List<User>();

    public virtual User? ParentUser { get; set; }

    public virtual ICollection<UsersInsurance> UsersInsurances { get; set; } = new List<UsersInsurance>();

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
