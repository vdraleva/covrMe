using CovrMe.WebAPI.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IErrorService
    {
        bool SendSpeedyErrorEmail(string policyNo, string names, string phone);
        bool SendInsuranceErrorEmail(string names, string phone, string policyNo, string insuranceType);
        void LogError(string message, string folferPath);
    }
}
