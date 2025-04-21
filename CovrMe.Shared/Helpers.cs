using EGNToolBox;
using EIKToolBox;
using CovrMe.Models.Users;
using CovrMe.Models.Vehicles;
using CovrMe.Shared.Constants;
using CovrMe.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovrMe.Shared
{
    public static class Helpers
    {
        public static string GetEnumDescription(Enum enumVal)
        {
            MemberInfo[] memInfo = enumVal.GetType().GetMember(enumVal.ToString());
            DescriptionAttribute attribute = CustomAttributeExtensions.GetCustomAttribute<DescriptionAttribute>(memInfo[0]);
            return attribute.Description;
        }
        public static string GetImageSrc(string insuranceCompany)
        {
            string imageSrc = string.Empty;

            switch (insuranceCompany)
            {
                case GlobalConstants.Dzi: imageSrc = GlobalConstants.DziImgSrc; break;

                case GlobalConstants.Euroins: imageSrc = GlobalConstants.EuroinsImgSrc; break;

                case GlobalConstants.Generali: imageSrc = GlobalConstants.GeneraliImgSrc; break;

                case GlobalConstants.Groupama: imageSrc = GlobalConstants.GroupamaImgSrc; break;

                case GlobalConstants.Ozk: imageSrc = GlobalConstants.OzkImgSrc; break;

                case GlobalConstants.Bulins: imageSrc = GlobalConstants.BulinsImgSrc; break;

                case GlobalConstants.Bulstrad: imageSrc = GlobalConstants.BulstradImgSrc; break;

                case GlobalConstants.Uniqa: imageSrc = GlobalConstants.UniqaImgSrc; break;
                case GlobalConstants.Levins: imageSrc = GlobalConstants.LevinsImgSrc; break;

                default: break;
            }

            return imageSrc;

        }
        public static bool ValidateInsuranceUserInfo(InsuranceUserInfoModel model)
        {
            bool valid = true;

            if (string.IsNullOrEmpty(model.UserId))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.FirstName))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.SurName))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.Uin) && string.IsNullOrEmpty(model.VatNumber))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.Address))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.RegionCode))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.MunicipalityCode))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.PostalCode))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.CityCode))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.CountryCode))
            {
                valid = false;
            }

            if (model.DrivingExperiance == 0)
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.RegionName))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.MunicipalityName))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.CityName))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.Phone))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.BirthDateString))
            {
                valid = false;
            }

            return valid;
        }
        public static bool ValidateInsuranceVehicleInfo(InsuranceVehicleInfo model)
        {
            bool valid = true;

            if (string.IsNullOrEmpty(model.PlateNumber))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.RegistrationCertificateNumber))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.VehicleBrand))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.VehicleModel))
            {
                valid = false;
            }

            if (model.VehicleModelId == 0)
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.FirstRegistrationDate))
            {
                valid = false;
            }

            if (string.IsNullOrEmpty(model.VehicleUsage))
            {
                valid = false;
            }

            if (model.VehicleTypeId == 0)
            {
                valid = false;
            }

            return valid;
        }
        public static string GetPageTitle(byte insuranceType)
        {
            string title = string.Empty;

            if (insuranceType == (byte)InsuranceTypeEnum.Civil)
            {
                title = GlobalConstants.CivilInsurance;
            }
            else if (insuranceType == (byte)InsuranceTypeEnum.Travel)
            {
                title = GlobalConstants.TravelInsurance;
            }
            else if (insuranceType == (byte)InsuranceTypeEnum.Mountain)
            {
                title = GlobalConstants.MountainInsurance;
            }
            else if (insuranceType == (byte)InsuranceTypeEnum.Health)
            {
                title = GlobalConstants.HealthInsurance;
            }
            else if (insuranceType == (byte)InsuranceTypeEnum.MyThings)
            {
                title = GlobalConstants.MyThingsInsurance;
            }

            return title;
        }
        public static string GetInsuranceNumberText(int number)
        {
            string result = string.Empty;

            switch (number)
            {
                case 1:
                    result = "Първа вноска: ";
                    break;
                case 2:
                    result = "Втора вноска: ";
                    break;
                case 3:
                    result = "Трета вноска: ";
                    break;
                case 4:
                    result = "Четвърта вноска: ";
                    break;
                case 5:
                    result = "Пета вноска: ";
                    break;
                case 6:
                    result = "Шеста вноска: ";
                    break;
                case 7:
                    result = "Седма вноска: ";
                    break;
                case 8:
                    result = "Осма вноска: ";
                    break;
                case 9:
                    result = "Девета вноска: ";
                    break;
                case 10:
                    result = "Десета вноска: ";
                    break;
                case 11:
                    result = "Единадесета вноска: ";
                    break;
                case 12:
                    result = "Дванадесета вноска: ";
                    break;
                default:
                    break;
            }

            return result;
        }
        public static string GetInsuranceNumberShortText(int number)
        {
            string result = string.Empty;

            switch (number)
            {
                case 1:
                    result = "1-ва";
                    break;
                case 2:
                    result = "2-ра";
                    break;
                case 3:
                    result = "3-та";
                    break;
                case 4:
                    result = "4-та";
                    break;
                case 5:
                    result = "5-та";
                    break;
                case 6:
                    result = "6-та";
                    break;
                case 7:
                    result = "7-ма";
                    break;
                case 8:
                    result = "8-ма";
                    break;
                case 9:
                    result = "9-та";
                    break;
                case 10:
                    result = "10-та";
                    break;
                case 11:
                    result = "11-та";
                    break;
                case 12:
                    result = "12-та";
                    break;
                default:
                    break;
            }

            return result;
        }
        public static bool EgnValidation(ulong uiNumber)
        {
            EGN egn = new EGN(uiNumber);
            bool isValid = egn.IsValid();

            return isValid;
        }
        public static DateTime GetDateFromEgn(ulong uiNumber)
        {
            try
            {
                EGN egn = new EGN(uiNumber);
                var day = egn.Day;
                var month = egn.Month;
                var year = egn.Year;

                var date = new DateTime(year, month, day);
                return date;
            }
            catch(Exception e)
            {
                return DateTime.Now;
            }
        }
        public static bool ValidateEgnAndBirthDate(DateTime inputDate, ulong uiNumber)
        {
            EGN egn = new EGN(uiNumber);

            var day = egn.Day;
            var month = egn.Month;
            var year = egn.Year;

            var date = new DateTime(year, month, day);

            if(inputDate.Date == date.Date)
            {
                return true;
            }

            return false;
        }
        public static bool VatValidation(string vat)
        {
            EIK eik = new EIK(vat);
            bool isValid = eik.IsValid();

            return isValid;
        }
        public static string FormatNumbers(decimal number)
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            return number.ToString(number % 1 == 0 ? "#,0" : "#,0.00#", nfi);
        }
        public static string FormatPrice(decimal number)
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            return number.ToString("#,0.00", nfi);
        }
        public static bool ValidateForeignerNumber(string input)
        {
            if (Regex.IsMatch(input, GlobalConstants.ForeignerNumberRegex))
            {
                return true;
            }

            return false;
        }
    }
}
