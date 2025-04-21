
namespace CovrMe.ApplicationHelpers;

using CovrMe.Models.Insurances;
using CovrMe.Models.Insurances.Result;
using CovrMe.Shared;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using System.Collections.ObjectModel;
public static class InsuranceHelper
{
    public static Dictionary<string, ObservableCollection<MyInsurancesModel>> PopulateInsuranceCollections(List<InsuranceModel> insurances)
    {
        var active = new ObservableCollection<MyInsurancesModel>();
        var expired = new ObservableCollection<MyInsurancesModel>();

        var result = new Dictionary<string, ObservableCollection<MyInsurancesModel>>();
        ;
        foreach (var insurance in insurances.OrderByDescending(x => x.CurrentEndDate))
        {
            var current = new MyInsurancesModel();

            current.ExpireDate = insurance.CurrentEndDate.ToString("dd.MM.yyyy");
            current.InsuranceId = insurance.Id;
            current.InsuredUserId = insurance.InsuredUsers.Where(x => x.IsInsurer == true).Select(x => x.UserId).FirstOrDefault();
            current.PolicyNo = insurance.PolicyNo;

            var insuranceTypeEnum = (InsuranceTypeEnum)((int)insurance.Type);
            current.InsuranceType = Helpers.GetEnumDescription(insuranceTypeEnum);

            InsuranceCompanyEnum insuranceCompanyEnum = (InsuranceCompanyEnum)Enum.Parse(typeof(InsuranceCompanyEnum), insurance.InsuranceCompany.CompanyName);
            current.InsuranceCompany = Helpers.GetEnumDescription(insuranceCompanyEnum);
            current.InsuranceCompanyCode = insurance.InsuranceCompany.CompanyName;

            if (insurance.Type == (byte)InsuranceTypeEnum.Civil)
            {
                current.IsCivil = true;
                if (insurance.Vehicle != null)
                {
                    current.Name = $"{insurance.Vehicle.Brand} {insurance.Vehicle.Model} / {insurance.Vehicle.PlateNumber}";
                }
                if (insurance.CivilInsurance != null)
                {
                    current.GreenCardNo = insurance.CivilInsurance.GreenCardNo;
                }
            }
            else if (insurance.Type == (byte)InsuranceTypeEnum.MyThings)
            {
                current.Name = $"{insurance.MyThingsInsurance.Brand} {insurance.MyThingsInsurance.Model}";
                current.IsCivil = false;
            }
            else
            {
                var insurer = insurance.InsuredUsers.FirstOrDefault(x => x.IsInsurer);
                current.IsCivil = false;

                if (insurer != null)
                {
                    current.Name = $"{insurer.FirstName} {insurer.LastName}";
                }
            }

            current.InstallmentToPay = insurance.InstallmentToPay;

            if (insurance.CurrentEndDate < DateTime.Now)
            {
                current.ShowBtn = true;
                current.BtnSource = "btnbuyaccord.png";
                expired.Add(current);

            }
            else
            {
                current.StatusImgSource = "dotgreen.png";

                if (current.IsCivil == true)
                {
                    if (insurance.CurrentEndDate.AddDays(-20) <= DateTime.Now)
                    {
                        current.ShowBtn = true;
                        current.BtnSource = current.InstallmentToPay == 0 ? "btnbuyaccord.png" : "btncashaccord.png";
                        current.IsExpiring = true;
                        current.StatusImgSource = "dotyellow.png";
                    }
                }
                else
                {
                    if (insurance.CurrentEndDate.AddDays(-1) <= DateTime.Now)
                    {
                        current.ShowBtn = true;
                        current.BtnSource = current.InstallmentToPay == 0 ? "btnbuyaccord.png" : "btncashaccord.png";
                        current.IsExpiring = true;
                        current.StatusImgSource = "dotyellow.png";
                    }
                }

                active.Add(current);
            }
        }

        result.Add(GlobalConstants.ActiveInsurance, active);
        result.Add(GlobalConstants.ExpiredInsurance, expired);

        return result;
    }
}
