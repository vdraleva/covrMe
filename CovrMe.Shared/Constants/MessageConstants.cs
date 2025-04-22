namespace CovrMe.Shared.Constants
{
    public class MessageConstants
    {
        #region Success

        public const string EditProfilSuccess = "Успешна редакция на профила";
        public const string FamilyFriendsSuccess = "Успешно добавихте потребител към семейство и приятели";
        public const string ChangePasswordSuccess = "Успешна смяна на парола";
        public const string EditVehicleSuccess = "Успешно редактирахте автомобил";
        public const string AddVehicleSuccess = "Успешно добавихте автомобил";
        public const string DeleteUserSuccess = "Успешно изтрихте профила си";

        #endregion

        #region Error
        public const string Unsuccess = "Unsuccess";
        public const string UnsuccessVerifyEmail = "Неуспешна верификация на имейл";
        public const string GeneralError = "Нещо се обърка, опитайте отново по-късно";
        public const string InvalidUsernameOrPassword = "Невалиден имейл или парола";
        public const string InvalidEmail = "Невалиден имейл";
        public const string RequiredEmail = "Полето имейл е задължително";
        public const string RequiredPassword = "Полето парола е задължително";
        public const string RequiredAll = "Всички полета са задължителни";
        public const string PassNotMatch = "Паролите не съвпадат";
        public const string InvalidPhone = "Невалиден телефонен номер";
        public const string RequiredFirstNameЕng = "Името на латиница е задължително поле.";
        public const string RequiredFirstName = "Име е задължително поле.";
        public const string RequiredInfo = "Трябва да попълните характерен обект за получаване в секция Информация или да дадете допълнителна информация за адреса като бло, вход, етаж и апартаманент..";
        public const string RequiredSurName = "Презиме е задължително поле.";
        public const string RequiredLastName = "Фамилия е задължително поле.";
        public const string RequiredUin = "ЕГН е задължително поле.";
        public const string RequiredVat = "ЕИК е задължително поле, тъй като сте попълнили име на фирма.";
        public const string RequiredCompanyName = "Име на фирма е задължително поле, тъй като сте попълнили ЕИК.";
        public const string RequiredPhone = "Телефон е задължително поле.";
        public const string RequiredAddress = "Адрес е задължително поле.";
        public const string RequiredPostCode = "Пощенски код е задължително поле.";
        public const string RequiredStreet = "Улица е задължително поле.";
        public const string RequiredCity = "Град е задължително поле.";
        public const string RequiredNeighbourhood = "Квартал е задължително поле.";
        public const string RequiredConfirmPassword = "Повтори парола е задължително поле.";
        public const string InvalidUin = "Невалидно ЕГН/ЛНЧ."; 
        public const string ExistsUin = "Човек с това ЕГН, вече съществува.";
        public const string InvalidVat = "Невалиден ЕИК номер.";
        public const string InvalidConfirmCode = "Невалиден код.";
        public const string InvalidRegistrationCertificateNumber = "Невалиден номер на талон.";
        public const string ExistsRegistrationCertificateNumber = "Този номер на талон вече съществува.";
        public const string InvalidVehiclePlateNumber = "Невалиден регистрационен номер, моля проверете написания от вас регистрационен номер.";
        public const string VehicleModelNotSelected = "Не сте избрали модел на автомобила.";
        public const string VehicleTypeNotSelected = "Не сте избрали тип на автомобила.";
        public const string EngineTypeNotSelected = "Не сте избрали тип на двигателя.";
        public const string ColorNotSelected = "Не сте избрали цвят.";
        public const string SteeringWheelNotSelected = "Не сте избрали позиция на волана";
        public const string BodyTypeNotSelected = "Не сте избрали тип на купето.";
        public const string VehicleUsageNotSelected = "Не сте маркирали за какво се използва автомобила.";
        public const string RegionNotSelected = "Не сте избрали област.";
        public const string MuniciplalityNotSelected = "Не сте избрали община.";
        public const string GuiltNotSelected = "Не сте избрали провинение";
        public const string CityNotSelected = "Не сте избрали населено място.";
        public const string DrivingExpNotSelected = "Не сте избрали шофьорски стаж.";
        public const string NationalityNotSelected = "Не сте избрали националност.";
        public const string ExpiresNotInCorrectFormat = "Дните на изтичане не са валидни.";
        public const string CvvNotInCorrectFormat = "Cvv номера е невалиден.";
        public const string CardNotInCorrectFormat = "Картата е невалидна.";
        public const string NamesNotInCorrectFormat = "Имената не са валидни.";
        public const string RequiredOfficeId= "Не сте избрали офис, до който да доставим пратката";
        public const string Invalid = "Невалиден формат.";
        public const string ChangePasswordError = "Неуспешна смяна на парола";
        public const string CurrentPasswordError = "Попълнете текущата парола";
        public const string EditUserError = "Неуспешна редакция на профила";
        public const string BulgarianError = "Полето трябва да е попълнено на български.";
        public const string RequiredUinOrVatError = "Трябва да въведете ЕГН или ЕИК.";
        public const string AddressError = "Адресът може да съдържа само букви на кирилица и числа.";
        public const string LatinnError = "Полето трябва да е попълнено на латиница.";
        public const string LatinAddressError = "Адресът може да съдържа само букви на латиница и числа.";
        public const string AgeIsZeroError = "Не може да има лице с невъведени години";
        public const string AgeAdultRequirementError = "Трябва да сте пълнолетно лице, за да можете да си направите застраховка.";
        public const string BrandRequiredError = "Полето Марка е задължително.";
        public const string ModelRequiredError = "Полето Модел е задължително.";
        public const string SumRequiredError = "Полето Стойност е задължително.";
        public const string SumGreaterThanZeroError = "Полето Стойност трябва да е по-голямо от 0";
        public const string ColorRequiredError = "Полето Цвят е задължително.";
        public const string UnrepairedDamageRequiredError = "Не сте дали описание на неостранените щети.";
        public const string NoInsuredUsersError = "Трябва да въведете поне едно застраховано лице.";
        public const string FamilyPolicyMoreThanOneUsersError = "Фамилната полица трябва да има повече от едно лице.";
        public const string FamilyPolicyMoreThanFiveUsersError = "По семейна полица може да се застраховат до 5 лица.";
        public const string NonFamilyPolicyOnlyOneUserError = "По несемейна полица застрахованият може да е само един.";
        public const string SizeRequiredError = "Полето размер е задължително.";
        public const string SizeLengthError = "Полето трябва да съдържа поне 3 символа.";
        public const string SumNotBiggerThan = "Сумата не може да е по-голяма от ";
        public const string OnlyDigits = "Полето трябва да съдържа само цифри.";
        public const string InvalidVin = "Полето Номер на рама не е валидно.";
        public const string EgnAndDateIncompatible = "Въвели сте грешно ЕГН или дата на раждане";
        public const string IncorectAgeGroup = "Въвели сте, че Лице {0} е на {1} год., но излиза, че е на {2} год.";
        public const string DuplicatedEgnError = "Не може да има повече от 1 лице с еднакво ЕГН";
        public const string StartDateMustBeAfterEndDate = "Крайната дата трябва да е след началната!";
        public const string EnterUserInfo = "Моля, въведете първо информация за потребителя!"; 
        public const string SumMoreThan100Error = "Стойността не може да е по-малка от 100лв.";
        public const string ChooseValidStartDateError = "Моля, изберете начална дата от позволения период!";
        public const string PurchaseDateFutureDateError = "Полето дата на закупуване не може да бъде бъдеща дата.";
        public const string PurchaseDateOldDateError = "Полето дата на закупуване не може да бъде по-рано от {0} г.";
        public const string CivilInsuranceMissingStickersError = "В момента има проблем с издаването на гражданска застраховка от тази застрахователна компания. Моля опитайте по-късно!";
        public const string MountainGroupMaxDateError = "Максималния срок на групови застраховки може да е 30 дни.";
        public const string InappropriatedAgeError = "Ако желаете да застраховате лице над {0} г., моля свържете се с нас на hello@insurance.bg";
        public const string BadPhoto = "Снимката не се получи добре. \nМоля, опитайте пак!";
        public const string OcrRequestTimeExpired = "Сканирането има временно забавяне. \nМоля, опитайте отново след малко!";
        public const string SameSide = "Снимана е една и съща страна два пъти. \nМоля, снимайте талона от двете страни!";
        #endregion

        #region Other

        public const string YourSpeedyOfficeIs = "Избраният офис е: {0}";
        public const string AlreadyHaveCivilInsurance = "Имате валидна застраховка за избраният автомобил.";
        public const string FocusDocument = "\nМоля, фокусирайте документа вертикално, върху тъмен фон.";

        #endregion
    }
}
