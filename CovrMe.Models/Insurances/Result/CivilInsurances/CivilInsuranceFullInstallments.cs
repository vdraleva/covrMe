using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsuranceFullInstallments
    {
        public CivilInsuranceInstallments? FirstInstallment { get; set; }
        public CivilInsuranceInstallments? SecondInstallment { get; set; }
        public CivilInsuranceInstallments? FourthInstallment { get; set; }
    }
}
