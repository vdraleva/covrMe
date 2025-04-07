using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Shared.Constants
{
    public class MessageConstants
    {
        #region Success
        public const string CreateStickerSuccess= "Успешно създадохте стикер с този номер.";
        public const string CreateGreenCardSuccess= "Успешно създадохте зелена карта с този номер.";
        #endregion

        #region Errors

        public const string Error = "Възникна грешка.";
        public const string ErrorCreatingTransportType = "Възникна грешка при създаване на тип транспорт.";
        public const string StickerAlreadyExists = "Този стикер съществува.";
        public const string GreenCardAlreadyExists = "Тази зелена карта съществува.";
        public const string DuplicateEmailError = "Вече съществува потребител, който е регистриран с този имейл.";
        public const string UserDoNotExists = "Не съществува такъв потребител."; 
        public const string InvalidCredentials = "Неправилен имейл или парола.";
        public const string ExistingVehicleNumbers = "Вече съществува автомобил с този регистрационен номер и номер на талон.";
        public const string CodeDoNotExists = "Неправилен валидационен код.";
        #endregion
    }
}
