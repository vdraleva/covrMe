using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Shared.Constants
{
    public class GlobalConstants
    {
        #region Roles

        public const string AdministratorRoleName = "Administrator";
        public const string UserRoleName = "User";

        #endregion

        #region Emails

        public const string PasswordRecoveryHeader = "Password recovery code";
        public const string PasswordRecoveryEmail = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n<html>\r\n  <head>\r\n    <!-- Compiled with Bootstrap Email version: 1.4.0 --><meta http-equiv=\"x-ua-compatible\" content=\"ie=edge\">\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n    <meta name=\"format-detection\" content=\"telephone=no, date=no, address=no, email=no\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n    <style type=\"text/css\">\r\n      body,table,td{font-family:Helvetica,Arial,sans-serif !important}.ExternalClass{width:100%}.ExternalClass,.ExternalClass p,.ExternalClass span,.ExternalClass font,.ExternalClass td,.ExternalClass div{line-height:150%}a{text-decoration:none}*{color:inherit}a[x-apple-data-detectors],u+#body a,#MessageViewBody a{color:inherit;text-decoration:none;font-size:inherit;font-family:inherit;font-weight:inherit;line-height:inherit}img{-ms-interpolation-mode:bicubic}table:not([class^=s-]){font-family:Helvetica,Arial,sans-serif;mso-table-lspace:0pt;mso-table-rspace:0pt;border-spacing:0px;border-collapse:collapse}table:not([class^=s-]) td{border-spacing:0px;border-collapse:collapse}@media screen and (max-width: 600px){.w-full,.w-full>tbody>tr>td{width:100% !important}.p-lg-10:not(table),.p-lg-10:not(.btn)>tbody>tr>td,.p-lg-10.btn td a{padding:0 !important}.p-6:not(table),.p-6:not(.btn)>tbody>tr>td,.p-6.btn td a{padding:24px !important}*[class*=s-lg-]>tbody>tr>td{font-size:0 !important;line-height:0 !important;height:0 !important}.s-4>tbody>tr>td{font-size:16px !important;line-height:16px !important;height:16px !important}.s-10>tbody>tr>td{font-size:40px !important;line-height:40px !important;height:40px !important}}\r\n    </style>\r\n  </head>\r\n  <body class=\"bg-light\" style=\"outline: 0; width: 100%; min-width: 100%; height: 100%; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; font-family: Helvetica, Arial, sans-serif; line-height: 24px; font-weight: normal; font-size: 16px; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; box-sizing: border-box; color: #000000; margin: 0; padding: 0; border-width: 0;\" bgcolor=\"#f7fafc\">\r\n    <table class=\"bg-light body\" valign=\"top\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"outline: 0; width: 100%; min-width: 100%; height: 100%; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; font-family: Helvetica, Arial, sans-serif; line-height: 24px; font-weight: normal; font-size: 16px; -moz-box-sizing: border-box; -webkit-box-sizing: border-box; box-sizing: border-box; color: #000000; margin: 0; padding: 0; border-width: 0;\" bgcolor=\"#f7fafc\">\r\n      <tbody>\r\n        <tr>\r\n          <td valign=\"top\" style=\"line-height: 24px; font-size: 16px; margin: 0;\" align=\"left\" bgcolor=\"#f7fafc\">\r\n            <table class=\"container\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\">\r\n              <tbody>\r\n                <tr>\r\n                  <td align=\"center\" style=\"line-height: 24px; font-size: 16px; margin: 0; padding: 0 16px;\">\r\n                    <!--[if (gte mso 9)|(IE)]>\r\n                      <table align=\"center\" role=\"presentation\">\r\n                        <tbody>\r\n                          <tr>\r\n                            <td width=\"600\">\r\n                    <![endif]-->\r\n                    <table align=\"center\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%; max-width: 600px; margin: 0 auto;\">\r\n                      <tbody>\r\n                        <tr>\r\n                          <td style=\"line-height: 24px; font-size: 16px; margin: 0;\" align=\"left\">\r\n                            <table class=\"s-10 w-full\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\" width=\"100%\">\r\n                              <tbody>\r\n                                <tr>\r\n                                  <td style=\"line-height: 40px; font-size: 40px; width: 100%; height: 40px; margin: 0;\" align=\"left\" width=\"100%\" height=\"40\">\r\n                                    &#160;\r\n                                  </td>\r\n                                </tr>\r\n                              </tbody>\r\n                            </table>\r\n                            <table class=\"ax-center\" role=\"presentation\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin: 0 auto;\">\r\n                              <tbody>\r\n                                <tr>\r\n                                  <td style=\"line-height: 24px; font-size: 16px; margin: 0;\" align=\"left\">\r\n                                    <img class=\"\" src=\"https://insurance.bg/wp-content/uploads/2023/11/CovrMe-logo.png\" style=\"height: auto; line-height: 100%; outline: none; text-decoration: none; display: block; border-style: none; border-width: 0;\">\r\n                                  </td>\r\n                                </tr>\r\n                              </tbody>\r\n                            </table>\r\n                            <table class=\"s-10 w-full\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\" width=\"100%\">\r\n                              <tbody>\r\n                                <tr>\r\n                                  <td style=\"line-height: 40px; font-size: 40px; width: 100%; height: 40px; margin: 0;\" align=\"left\" width=\"100%\" height=\"40\">\r\n                                    &#160;\r\n                                  </td>\r\n                                </tr>\r\n                              </tbody>\r\n                            </table>\r\n                            <table class=\"card p-6 p-lg-10 space-y-4\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-radius: 6px; border-collapse: separate !important; width: 100%; overflow: hidden; border: 1px solid #e2e8f0;\" bgcolor=\"#ffffff\">\r\n                              <tbody>\r\n                                <tr>\r\n                                  <td style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0; padding: 40px;\" align=\"left\" bgcolor=\"#ffffff\">\r\n                                    <div class=\"card-header text-black text-center\" style=\"color: #000000;\" align=\"center\">\r\n                                      <h3 class=\"text-center\" style=\"padding-top: 0; padding-bottom: 0; font-weight: 500; vertical-align: baseline; font-size: 28px; line-height: 33.6px; margin: 0;\" align=\"center\">Password Reset Confirmation</h3>\r\n                                    </div>\r\n                                    <table class=\"s-4 w-full\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\" width=\"100%\">\r\n                                      <tbody>\r\n                                        <tr>\r\n                                          <td style=\"line-height: 16px; font-size: 16px; width: 100%; height: 16px; margin: 0;\" align=\"left\" width=\"100%\" height=\"16\">\r\n                                            &#160;\r\n                                          </td>\r\n                                        </tr>\r\n                                      </tbody>\r\n                                    </table>\r\n                                    <table class=\"card-body\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\">\r\n                                      <tbody>\r\n                                        <tr>\r\n                                          <td style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0; padding: 20px;\" align=\"left\">\r\n                                            <p style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0;\" align=\"left\">We have received a request to reset your password for your account.</p>\r\n                                            <p style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0;\" align=\"left\">Please use the following confirmation code to reset your password:</p>\r\n <br>\r\n                                            <p class=\"h4 text-success\" style=\"line-height: 28.8px; font-size: 24px; color: #823189; padding-top: 0; padding-bottom: 0; font-weight: 500; vertical-align: baseline; width: 100%; margin: 0;\" align=\"left\">Confirmation Code: $code$</p>\r\n <br>\r\n                                            <p style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0;\" align=\"left\">Enter this code on our mobile app to verify your identity and reset your password.</p>\r\n <br>\r\n                                            <p style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0;\" align=\"left\">If you did not request a password reset, please contact our support team immediately at <strong>hello@insurance.bg</strong>.</p>\r\n                                            <p style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0;\" align=\"left\">If you require any assistance or have questions regarding your password reset, please don't hesitate to reach out to our dedicated support team at <strong>hello@insurance.bg</strong> or <a href=\"https://insurance.bg/kontakti/\" style=\"color: #0d6efd;\">Contact Us</a>.</p>\r\n                                            <br>\r\n                                            <p style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0;\" align=\"left\">Thank you for choosing CovrMe. We're here to assist you!</p>\r\n                                          </td>\r\n                                        </tr>\r\n                                      </tbody>\r\n                                    </table>\r\n                                    <table class=\"s-4 w-full\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"width: 100%;\" width=\"100%\">\r\n                                      <tbody>\r\n                                        <tr>\r\n                                          <td style=\"line-height: 16px; font-size: 16px; width: 100%; height: 16px; margin: 0;\" align=\"left\" width=\"100%\" height=\"16\">\r\n                                            &#160;\r\n                                          </td>\r\n                                        </tr>\r\n                                      </tbody>\r\n                                    </table>\r\n                                    <div class=\"card-footer text-center\" style=\"\" align=\"center\">\r\n                                      <p style=\"line-height: 24px; font-size: 16px; width: 100%; margin: 0;\" align=\"left\">&#169; 2023 Insurance.bg</p>\r\n                                    </div>\r\n                                  </td>\r\n                                </tr>\r\n                              </tbody>\r\n                            </table>\r\n                          </td>\r\n                        </tr>\r\n                      </tbody>\r\n                    </table>\r\n                    <!--[if (gte mso 9)|(IE)]>\r\n                    </td>\r\n                  </tr>\r\n                </tbody>\r\n              </table>\r\n                    <![endif]-->\r\n                  </td>\r\n                </tr>\r\n              </tbody>\r\n            </table>\r\n          </td>\r\n        </tr>\r\n      </tbody>\r\n    </table>\r\n  </body>\r\n</html>\r\n";

        public const string InstallmentInsuranceEmailHeader = "Платена вноска по застраховка от CovrMe";
        public const string InstallmentInsuranceEmаil = "Успешно заплатихте всноска по вашата застраховка \"$type$\" с полица $policy$.";

        public const string SpeedyErrorEmailHeader = "Грешка при създаване на товарителница";
        public const string SpeedyErrorEmаil = "Възникна грешка при създаване на товарителница към Спиди за застраховка тип ГО с полица $policyNo$. Данни за клиента: $names$, $phone$.";

        public const string InsuranceErrorHeader = "Възникна грешка при създаване на застраховка";
        public const string InsuranceErrorEmаil = "Възникна грешка при създаване на застраховка \"$type$\". Данни за клиента: $names$, $phone$.";

        public const string CivilInsuranceHeaderClient = "Нова застраховка Гражданска отговорност";
        public const string CivilInsuranceHeaderBroker = "Нова застраховка Гражданска отговорност";

        public const string TravelInsuranceHeaderClient = "Успешно създадена полица Помощ при пътуване";
        public const string TravelInsuranceHeaderBroker = "Нова застрановка “Помощ при пътуване“: $policyNo$";

        public const string MountainInsuranceHeaderClient = "Успешно създадена полица Планинска застраховка";
        public const string MountainInsuranceHeaderBroker = "Нова застрановка “Планинска застраховка“: $policyNo$";

        public const string HealthInsuranceHeaderClient = "Успешно създадена полица Здраве и спокойствие";
        public const string HealthInsuranceHeaderBroker = "Нова застрановка “Здраве и спокойствие“: $policyNo$";

        public const string MyThingsInsuranceHeaderClient = "Успешно създадена полица “Моите вещи”";
        public const string MyThingsInsuranceHeaderBroker = "Нова застрановка “Моите вещи“: $policyNo$";

        public const string CascoRequestEmailHeader = "Запитване от клиент за Застраховка Автокаско";
        public const string CascoRequestEmаil = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>New Customer Request</title>\r\n    <link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css\">\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">\r\n            <h2>Ново Запитване от Клиент</h2>\r\n        </div>\r\n        <div>\r\n            <h4>Водач:</h4>\r\n            <p><span class=\"field-label\">Име:</span>$firstName$</p>\r\n            <p><span class=\"field-label\">Презиме:</span>$surName$</p>\r\n            <p><span class=\"field-label\">Фамилия:</span>$lastName$</p>\r\n            <p><span class=\"field-label\">Телефон:</span>$phone$</p>\r\n            <p><span class=\"field-label\">E-mail:</span>$email$</p>\r\n            <p><span class=\"field-label\">Шофьорски стаж:</span>$drivingExperiance$</p>\r\n            <p><span class=\"field-label\">ЕГН:</span>$uin$</p>\r\n        </div>\r\n        <div>\r\n            <h4>Автомобил:</h4>\r\n            <p><span class=\"field-label\">Марка:</span>$brand$</p>\r\n            <p><span class=\"field-label\">Модел:</span>$model$</p>\r\n            <p><span class=\"field-label\">Дата на първа регистрация:</span>$year$</p>\r\n            <p><span class=\"field-label\">Тип:</span>$engineType$</p>\r\n            <p><span class=\"field-label\">Предназначение:</span>$vehicleUsage$</p>\r\n            <p><span class=\"field-label\">Кубатура/Капацитет на батерията:</span>$volumeOrKilowatts$</p>\r\n        <p><span class=\"field-label\">Мощност kW:</span>$vehicleKilowatts$</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n";
        
        #endregion

        #region Patterns

        public const string PolicyNoPattern = "$policyNo$";
        public const string FirstNamePattern = "$firstName$";
        public const string InfoDocPattern = "$infoDoc$";
        public const string InsuranceTypePattern = "$insuranceType$";
        public const string SurNamePattern = "$surName$";
        public const string LastNamePattern = "$lastName$";
        public const string PhonePattern = "$phone$";
        public const string EmailPattern = "$email$";
        public const string DrivingExperiencePattern = "$drivingExperiance$";
        public const string UinPattern = "$uin$";
        public const string BrandPattern = "$brand$";
        public const string ModelPattern = "$model$";
        public const string YearPattern = "$year$";
        public const string EngineTypePattern = "$engineType$";
        public const string VehicleUsagePattern = "$vehicleUsage$";
        public const string VolumeOrBatteryCapacity = "$volumeOrKilowatts$";
        public const string VehicleKilowatts = "$vehicleKilowatts$";


        #endregion

        #region Routes

        //Sirma
        public const string SirmaAuthUrl = "security/authenticate";
        public const string SirmaGetCities = "locations/cities";
        public const string SirmaGetCitiesPostCode = "locations/zip";

        //Vehicles
        public const string GetVehicleModels = "objects/vehicle-models";
        public const string GetVehicleBrands = "objects/vehicle-marks";
        public const string GetVehicleModelsApi = "api/objects/vehicle-models";
        public const string GetVehicleBrandsApi = "api/objects/vehicle-marks";
        public const string SirmaCivilInsuranceSearch = "liability/short";
        public const string SirmaCivilInsuranceLongSearch = "liability/calc";
        public const string SirmaCivilInsurancePolicy = "liability/policy-and-payment-short";
        public const string SirmaCivilInsuranceLongPolicy = "liability/issue";
        public const string SirmaCivilInsuranceNote = "policy/note";
        public const string SirmaCivilInsurancePdf = "policy/print";
        public const string GetMobileAppCacheValues = "api/cache/getValues";
        public const string TestSubscription = "api/test/testSubscription";
        public const string TestApi = "api/test/testApi";
        public const string TestAuth = "api/test/testAuth";
        public const string CheckPaymentStatus = "api/payment/checkPaymentStatus";
        public const string TestUniqa = "api/test/testUniqa";
        public const string TestPdf = "api/test/testPdf";
        public const string GetPolicy = "api/insuranceDocuments/getPolicy";
        public const string GetGreenCard = "api/insuranceDocuments/getGreenCard";
        public const string GetReceipt = "api/insuranceDocuments/getReceipt";
        public const string GetLocationsData = "api/getData/getLocations";
        public const string GetVehicleData = "api/getData/vetVehicleData";

        //Speedy
        public const string SpeedyFindOfficeUrl = "location/office";
        public const string SpeedyCalculation = "calculate";
        public const string SpeedyShipment = "shipment";

        //Users

        public const string AddMultipleUserToFamilyAndFriends = "api/user/addMultipleUserToFamilyAndFriends";
        public const string EditMultipleUserToFamilyAndFriends = "api/user/editMultipleUserToFamilyAndFriends";

        #endregion

        #region Auth

        public const string AuthenticationSchemes = "Bearer";

        #endregion

        #region Settings

        //Jwt
        public const string JwtIssuer = "Jwt:Issuer";
        public const string JwtAudience = "Jwt:Audience";
        public const string JwtKey = "Jwt:Key";

        //Google Credentials
        public const string GoogleClientId = "GoogleCredentials:ClientId";
        public const string GoogleClientSecret = "GoogleCredentials:ClientSecret";

        //Facebook Credentials
        public const string FacebookAppId = "FacebookCredentials:AppId";
        public const string FacebookAppSecret = "FacebookCredentials:AppSecret";

        #endregion

        #region Const

        public const int SqlUniqueErrorCode = 2627;

        public const string Jwt = "jwt";
        public const string BgCountryCode = "BG";
        public const string BrokerName = "Иншуранс БГ";

        //Speedy
        public const string Language = "bg";
        public const int CountryId = 100;
        public const int ServiceId = 505;
        public const bool AutoAdjustPickupDate = true;
        public const string ReturnShipmentPayer = "SENDER";
        public const string AdditionalServicesOption = "OPEN";
        public const bool Documents = true;
        public const double TotalWeight = 0.5;
        public const int ParcelsCount = 1;
        public const string CourierServicePayer = "RECIPIENT";
        public const string DeclaredValuePayer = "RECIPIENT";
        public const bool PrivatePerson = true;
        public const bool SaturdayDelivery = true;
        public const string Contents = "Documents";
        public const string Package = "ENVELOPE A4";
        public const string Bgn = "BGN";
        public const string Eur = "Eur";
        public const string New = "Нов";
        public const decimal EurToBgn = 1.95583m;

        //Travel
        public const int TravelPolicyType = 9;
        public const int UniqaEuroCurrencyCode = 20;
        public const int UniqaInsuredType = 16;
        public const string  UniqaPolicyIssueLocation = "68134";

        //MyThings
        public const string UniqaMyThingsCoverId = "21";

        //Uniqa InsuranceTypes
        public const string HealthInsuranceType = "Ж1";
        public const string TravelInsuranceType = "2C";
        public const string MyThingsInsuranceType = "2C";
        public const string MountainInsuranceType = "Ж1";

        //Uniqa InsuranceGroupes
        public const bool HealthInsuranceGroup = false;
        public const bool TravelInsuranceGroup = true;
        public const bool MyThingsInsuranceGroup = true;
        public const bool MountainInsuranceGroup = false;


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

        #endregion

        public const int BgnCode = 975;
        public const int EurCode = 978;
        public const string LanguageCode = "bg";
        public const string OrderNumbers = "OrderNumbers";

        public const string DuplicateEmailCode = "DuplicateEmail";
        public const string OnlyCyrilicRegex = @"^([а-яА-Я\s\-\.\,\!]+)$";
        public const string SqlWord = "SQL";
        public const string ExceptionWord = "Exception";
        public const string ErrorWord = "Грешка";
    }
}
