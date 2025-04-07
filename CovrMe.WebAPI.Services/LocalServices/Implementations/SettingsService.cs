using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Shared.Mapping;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class SettingsService : ISettingsService
    {
        private IRepository<Setting> _settingsRepository;
        private readonly ILogger _logger;
        public SettingsService(ILogger<SettingsService> logger, IRepository<Setting> settingsRepository)
        {
            this._logger = logger;
            this._settingsRepository = settingsRepository;
        }

        public T GetSettingByCode<T>(string code)
        {
            var setting = this._settingsRepository
                .AllAsNoTracking()
                .Where(x => x.Code == code)
                .To<T>()
                .FirstOrDefault();

            return setting;
        }

        public async Task UpdateSettingValue(string id, string value)
        {
            var currentSettingsId = Helpers.ParseStringToGuid(id);

            var currentSettings = this._settingsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == currentSettingsId)
                .FirstOrDefault();

            if(currentSettings != null)
            {
                currentSettings.Value = value;
                this._settingsRepository.Update(currentSettings);
                await this._settingsRepository.SaveChangesAsync();
            }
        }
    }
}
