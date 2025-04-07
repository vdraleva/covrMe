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
    public class InsuranceCompanyService : IInsuranceCompanyService
    {
        private IRepository<InsuranceCompany> _insuranceCompanyRepository;
        private readonly ILogger _logger;
        public InsuranceCompanyService(IRepository<InsuranceCompany> insuranceCompanyRepository, ILogger<InsuranceCompanyService> logger)
        {
            this._insuranceCompanyRepository = insuranceCompanyRepository;
            this._logger = logger;
        }

        public async Task<string> CreateAsync(string companyName)
        {
            InsuranceCompany company = new InsuranceCompany
            {
                Id = Guid.NewGuid(),
                CompanyName = companyName
            };

            await this._insuranceCompanyRepository.AddAsync(company);
            await this._insuranceCompanyRepository.SaveChangesAsync();

            return company.Id.ToString();
        }

        public T GetCompanyByName<T>(string name)
        {
            var company = this._insuranceCompanyRepository
                .AllAsNoTracking()
                .Where(x => x.CompanyName == name)
                .To<T>()
                .FirstOrDefault();

            return company;
        }
        public IQueryable<T> GetCompanies<T>()
        {
            var company = this._insuranceCompanyRepository
                .AllAsNoTracking()
                .To<T>()
                .AsQueryable();

            return company;
        }

        public async Task<List<string>> GetCompanyNames()
        {
            var companies = this._insuranceCompanyRepository
                .AllAsNoTracking()
                .Where(x => x.CompanyName != "Uniqa")
                .Select(x => x.CompanyName.ToLower())
                .ToList();

            return companies;
        }

    }
}
