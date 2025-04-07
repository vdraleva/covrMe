using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface ISettingsService
    {
        T GetSettingByCode<T>(string code);
        Task UpdateSettingValue(string id, string value);
    }
}
