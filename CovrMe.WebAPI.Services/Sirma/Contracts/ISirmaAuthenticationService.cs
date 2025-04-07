using CovrMe.WebAPI.Models.Result.Sirma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Sirma.Contracts
{
    public interface ISirmaAuthenticationService
    {
        Task<SirmaAuthenticationResultModel> Authenticate();
    }
}
