using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IInsuranceCompanyService
    {
        Task<string> CreateAsync(string companyName);
        T GetCompanyByName<T>(string name);
        Task<List<string>> GetCompanyNames();
        IQueryable<T> GetCompanies<T>();

    }
}
