namespace CovrMe.Shared.Constants
{
    public class GlobalConstants
    {
        #region ApiRoutes
        
        public const string GraphQlUrl = "graphql";
        public const string Cache = "api/cache/getValues";
        public const string Policy = "api/insuranceDocuments/getPolicy";
        public const string GetGreenCard = "api/insuranceDocuments/getGreenCard";
        public const string GetReceipt = "api/insuranceDocuments/getReceipt";
        public const string AddMultipleUserToFamilyAndFriends = "api/user/addMultipleUserToFamilyAndFriends";
        public const string EditMultipleUserToFamilyAndFriends = "api/user/editMultipleUserToFamilyAndFriends";
        #endregion

        #region Const
        public const string JsonType = "application/json";
        public const string ArrowImgMore = "arrowmore.png";
        public const string ArrowImgLess = "arrowless.png";
        public const string PlusImg = "plus.png";
        public const string MinusImg = "minus.png";
        public const string WhiteColor = "#fff";
        public const string ActiveColorPurple = "#E8DEF8";
        public const string New = "Нов";
        public const string ItsMe = "Това съм аз";        
        public const string ActiveInsurance = "active";
        public const string ExpiringInsurance = "expiring";
        public const string ExpiredInsurance = "expired";
        public const string EmptyColor = "#ADACAC";
        public const string FilledColor = "#823189";
        public const string CyrillicLabelInfo = "кирилица";
        public const string LatinLabelInfo = "латиница";
        public const decimal EurToBgn = 1.95583m;
        public const string NoName = "Няма име";
        public const int InappropriateAge = 70;
        #endregion

        #region Validation
        //public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        //public const string PhoneRegex = "^[\\+]?[(]?[0-9]{3}[)]?[-\\s\\.]?[0-9]{3}[-\\s\\.]?[0-9]{4,6}$";
        public const string EIKRegex = "^[0-9]{9}$";
        public const string VinRegex = "^[A-HJ-NPR-Z0-9]{17}$";
        public const string CardCvvRegex = @"^[0-9]{3}$";
        public const string CardNumberRegex = @"^[0-9]{16}$";
        public const string CardNamesRegex = @"^([A-Za-z]+)\s([A-Za-z]+)$";
        public const string UinNumberRegex = "^[0-9]{10}$";
        public const string PhoneRegex = "\\d";
        public const string OnlyNumbersRegex = "\\d";
        public const string CardExpireDateRegex = @"^([0-9]{2})\/([0-9]{2})$";
        public const string LatinFullNamesRegex = @"^([a-zA-Z\-]+\s[a-zA-Z\-]+\s[a-zA-Z\-]+)$";
        public const string BulgarianFullNamesRegex = @"^([а-яА-Я\-]+\s[а-яА-Я\-]+\s[а-яА-Я\-]+)$";
        public const string BgCountryCode = "BG";
        public const string BulgarianNameRegex = @"^([а-яА-Я\-]+)$";
        public const string BulgarianCompanyRegex = @"^([а-яА-Я\-\s]+)$";
        public const string AddressRegex = @"^([а-яА-Я\s\d\.\,\-]+)$";
        public const string CarNumberRegex = @"^[ABCEHKMOPTXY]{1,2}\d{4,5}[ABCEHKMOPTXY]{0,2}$";
        public const string ForeignerNumberRegex = "^[0-9]{10}$";

        public const string LatinNameRegex = @"^([a-zA-Z\-]+)$";
        public const string LatinCompanyRegex = @"^([a-zA-Z\-\s]+)$";
        public const string LatinAddressRegex = @"^([a-zA-Z\s\d\.\,\-]+)$";
        public const string EmailRegex = "^[A-Za-z0-9._-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}$";
        #endregion

        #region Images Sources

        public const string BulinsImgSrc = "bbulnig.png";
        public const string BulstradImgSrc = "bbulstrad.png";
        public const string DziImgSrc = "bdzi.png";
        public const string GeneraliImgSrc = "bgenerali.png";
        public const string GroupamaImgSrc = "bgroupama.png";
        public const string LevinsImgSrc = "blevins.png";
        public const string OzkImgSrc = "bozb.png";
        public const string EuroinsImgSrc = "beuroins.png";
        public const string UniqaImgSrc = "buniqa.png";

        #endregion

        #region Insurance Companies

        public const string Dzi = "Dzi";
        public const string Euroins = "Euroins";
        public const string Generali = "Generali";
        public const string Groupama = "Groupama";
        public const string Ozk = "Ozk";
        public const string Bulins = "Bulins";
        public const string Bulstrad = "Bulstrad";
        public const string Uniqa = "Uniqa";
        public const string Levins = "Levins";
        #endregion

        #region Links

        //Insurance Information

        public const string AllianzCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-allianz.pdf";
        public const string ArmeecCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-armeec.pdf";
        public const string BulinsCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-bulins.pdf";
        public const string BulstradCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-bulstrad.pdf";
        public const string DziCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-dzi.pdf";
        public const string EuroinsCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-euroins.pdf";
        public const string GeneraliCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-generali.pdf";
        public const string LevinsCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-levins.pdf";
        public const string UniqaCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-uniqa.pdf";
        public const string GroupamaCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-groupama.pdf";
        public const string OzkCivilInfo = "https://insurance.bg/wp-content/uploads/2024/05/informacionen-dokument-go-ozk.pdf";
        public const string InsuranceContactForm = "https://insurance.bg/kontakti/";

        //Travel
        public const string ObshtiUsloviaTouristURL = "https://insurance.bg/wp-content/uploads/2024/08/Pomosht_pri_patuvane_01.02.2024_37638483.pdf";
        public const string InfoDocumentTouristURL = "https://insurance.bg/wp-content/uploads/2024/05/travel_inf_doc_uniqa.pdf";
        public const string PreddogovorenInfoTouristURL = "https://insurance.bg/wp-content/uploads/2024/05/travel_uniqa_predog.pdf";
        public const string PrivacyPolicyTouristURL = "https://insurance.bg/wp-content/uploads/2024/05/uniqa_poveritelnost.pdf";

        //Mountain
        public const string ObshtiUsloviaMountainURL = "https://insurance.bg/wp-content/uploads/2024/05/mtn_ou_uniqa.pdf";
        public const string InfoDocumentMountainURL = "https://insurance.bg/wp-content/uploads/2024/05/mtn_inf_doc_uniqa.pdf";
        public const string PreddogovorenInfoMountainURL = "https://insurance.bg/wp-content/uploads/2024/05/Preddogovorna-UNIQA-Life.pdf";
        public const string PrivacyPolicyMountainURL = "https://insurance.bg/wp-content/uploads/2024/05/uniqa_poveritelnost.pdf";

        //Health
        public const string ObshtiUsloviaHealthURL = "https://insurance.bg/wp-content/uploads/2024/05/health_calm_ou_uniqa.pdf";
        public const string InfoDocumentHealthURL = "https://insurance.bg/wp-content/uploads/2024/05/health_calm_inf_doc_uniqa.pdf";
        public const string PreddogovorenInfoHealthURL = "https://insurance.bg/wp-content/uploads/2024/05/Preddogovorna-UNIQA-Life.pdf";
        public const string PrivacyPolicyHealthURL = "https://insurance.bg/wp-content/uploads/2024/05/uniqa_poveritelnost.pdf";
        public const string OperationSchema = "https://insurance.bg/wp-content/uploads/2024/05/Skhema_na_UNIKA_za_kategoriite_operacii.pdf";

        //MyThings - Bycicle
        public const string ObshtiUsloviaBycicleURL = "https://insurance.bg/wp-content/uploads/2024/08/OU_Imushtestvo_na_FL_01.02.2024_37638452.pdf";
        public const string InfoDocumentBycicleURL = "https://insurance.bg/wp-content/uploads/2024/05/ipid_velosipedi_i_trorinetki.pdf";
        public const string PreddogovorenInfoBycicleURL = "https://insurance.bg/wp-content/uploads/2024/05/travel_uniqa_predog.pdf";
        public const string SpecialConditionsBycicleURL = "https://insurance.bg/wp-content/uploads/2024/08/Spetsialni_usloviya_velosipedi_01.02.2024_37638450.pdf";
        public const string PrivacyPolicyBycicleURL = "https://insurance.bg/wp-content/uploads/2024/05/uniqa_poveritelnost.pdf";

        //MyThings - Glasses
        public const string ObshtiUsloviaGlassesURL = "https://insurance.bg/wp-content/uploads/2024/08/OU_Imushtestvo_na_FL_01.02.2024_37638452.pdf";
        public const string InfoDocumentGlassesURL = "https://insurance.bg/wp-content/uploads/2024/05/ipid_ochila.pdf";
        public const string PreddogovorenInfoGlassesURL = "https://insurance.bg/wp-content/uploads/2024/05/travel_uniqa_predog.pdf";
        public const string SpecialConditionsGlassesURL = "https://insurance.bg/wp-content/uploads/2024/08/Special_conditions_Eyeglasses_25.08.2020_35348160.pdf";
        public const string PrivacyPolicyGlassesURL = "https://insurance.bg/wp-content/uploads/2024/05/uniqa_poveritelnost.pdf";

        // Common
        public const string ObshtiUsloviaURL = "https://insurance.bg/obsthi-usloviya/";
        public const string PrivacyPolicyURL = "https://insurance.bg/politika-za-poveritelnost/";
        public const string ContractURL = "https://insurance.bg/wp-content/uploads/2024/05/%D0%94%D0%BE%D0%B3%D0%BE%D0%B2%D0%BE%D1%80-%D0%B7%D0%B0-%D0%B2%D1%8A%D0%B7%D0%BB%D0%B0%D0%B3%D0%B0%D0%BD%D0%B5.pdf";
        public const string BankCardURL = "https://insurance.bg/wp-content/uploads/2024/05/saglasie-za-registracziya-na-bankova-karta.pdf";
        public const string ContactUsURL = "https://insurance.bg/kontakti/";
        #endregion

        #region Titles

        public const string CivilInsuranceTitle = "Гражданска отговорност";

        #endregion

        #region Dsk

        public const string DskDepositedOperation = "deposited";
        public const string DskApprovedOperation = "approved";
        public const string DskAwaitingOperation = "awaiting";
        public const string DskFailedperation = "failed";

        #endregion

        #region Insurance Type

        public const string CivilInsurance = "Гражданска отговорност";
        public const string HealthInsurance = "Здраве и спокойствие";
        public const string TravelInsurance = "Помощ при пътуване";
        public const string MountainInsurance = "Планинска застраховка";
        public const string MyThingsInsurance = "Моите вещи";

        #endregion

        #region Assets

        //vehicles
        public const string VehicleBrands = "vehicles\\vehicleMarks.json";
        public const string VehicleTypes = "vehicles\\vehicleTypes.json";
        public const string VehicleUsages = "vehicles\\vehicleUsages.json";
        public const string VehicleBodyTypes = "vehicles\\bodyType.json";
        public const string VehicleColors = "vehicles\\colors.json";
        public const string VehicleEngineTypes = "vehicles\\engineTypes.json";

        //location
        public const string Countries = "location\\countries.json";
        public const string Regions = "location\\regions.json";
        public const string Municipalities = "location\\municipalities\\";
        public const string Cities = "location\\cities\\";

        //user
        public const string GuiltTypes = "user\\guiltTypes.json";

        #endregion
    }
}
