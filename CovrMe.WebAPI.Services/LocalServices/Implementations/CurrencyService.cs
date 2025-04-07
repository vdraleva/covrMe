using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Services.Sirma.Contracts;
using CovrMe.WebAPI.Shared.Mapping;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        private IRepository<Currency> _currencyRepository;

        public CurrencyService(ILogger<CurrencyService> logger, IConfiguration configuration, IRepository<Currency> currencyRepository)
        {
            this._logger = logger;
            this._configuration = configuration;
            this._currencyRepository = currencyRepository;
        }

        public async Task<string> CreateAsync(string currencyCode)
        {
            Currency currency = new Currency
            {
                Id = Guid.NewGuid(),
                Code = currencyCode
            };

            await this._currencyRepository.AddAsync(currency);
            await this._currencyRepository.SaveChangesAsync();

            return currency.Id.ToString();
        }

        public T GetCurrencyByCode<T>(string code)
        {
            var currency = this._currencyRepository
                .AllAsNoTracking()
                .Where(x => x.Code == code)
                .To<T>()
                .FirstOrDefault();

            return currency;
        }
    }
}
