using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsuranceInstallments
    {
        public CivilInsuranceInstallment? FirstInstallment { get; set; }

        public CivilInsuranceInstallment? SecondInstallment { get; set; }

        public CivilInsuranceInstallment? ThirdInstallment { get; set; }

        public CivilInsuranceInstallment? FourthInstallment { get; set; }
    }
}
