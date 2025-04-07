using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Documents
{
    public class StickerModel
    {
        public string Id { get; set; }
        public string DocumentsBatchId { get; set; }
        public string InsuranceCompanyId { get; set; }
        public string InsuranceCompanyName { get; set; }
        public string StickerNumber { get; set; }
        public bool IsActive { get; set;}
        public bool IsUsed { get; set; }
        public int UsedStickers { get; set;}
        public int TotalStickers { get; set; }
    }
}
