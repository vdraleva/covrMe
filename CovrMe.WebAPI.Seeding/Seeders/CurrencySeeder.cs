using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Models.Result.Currencies;
using CovrMe.WebAPI.Seeding.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace CovrMe.WebAPI.Seeding.Seeders
{
    public class CurrencySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var currencyService = serviceProvider.GetRequiredService<ICurrencyService>();

            await SeedCompanyAsync(GlobalConstants.Bgn, currencyService);
            await SeedCompanyAsync(GlobalConstants.Eur, currencyService);
        }
        private static async Task SeedCompanyAsync(string currencyCode, ICurrencyService currencyService)
        {
            var currentCurrency = currencyService.GetCurrencyByCode<CurrencyModel>(currencyCode);

            if (currentCurrency == null)
            {
                var result = await currencyService.CreateAsync(currencyCode);

                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception(MessageConstants.Error);
                }
            }

        }
    }
}
