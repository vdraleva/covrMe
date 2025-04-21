using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class BaseInsuranceOfferModel
    {
        public BaseInsuranceOfferModel()
        {
            this.Installments = new ObservableCollection<InstallmentModel>();
        }
        public string? PolicyNo { get; set; }
        public string? InsuranceId { get; set; }
        public string? UserId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogo { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public byte InsuranceType { get; set; }
        public int Installment { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Title { get; set; }        
        public bool NextInstallment { get; set; }
        public int InstallmentToPay { get; set; }
        public bool LongSearch { get; set; }
        public ObservableCollection<InstallmentModel> Installments { get; set; }
    }
}
