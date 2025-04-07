using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Models.Result.InsuranceCompanies;
using CovrMe.WebAPI.Seeding.Contracts;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace CovrMe.WebAPI.Seeding.Seeders
{
    public class InsuranceCompanySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var companyService = serviceProvider.GetRequiredService<IInsuranceCompanyService>();

            await SeedCompanyAsync(GlobalConstants.Dzi, companyService);
            await SeedCompanyAsync(GlobalConstants.Euroins, companyService);
            await SeedCompanyAsync(GlobalConstants.Generali, companyService);
            await SeedCompanyAsync(GlobalConstants.Groupama, companyService);
            await SeedCompanyAsync(GlobalConstants.Ozk, companyService);
            await SeedCompanyAsync(GlobalConstants.Bulins, companyService);
            await SeedCompanyAsync(GlobalConstants.Bulstrad, companyService);
            await SeedCompanyAsync(GlobalConstants.Uniqa, companyService);
        }
        private static async Task SeedCompanyAsync(string companyName, IInsuranceCompanyService companyService)
        {
            var currentCompany = companyService.GetCompanyByName<InsuranceCompanyModel>(companyName);

            if (currentCompany == null)
            {
                var result = await companyService.CreateAsync(companyName);

                if (string.IsNullOrEmpty(result))
                {
                    throw new Exception(MessageConstants.Error);
                }
            }
        }
    }
}
