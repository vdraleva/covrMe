using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Documents
{
    public class InsuranceCompanyBatchInfoModel
    {
        public string? BatchId { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public int TotalStickersCount { get; set; }
        public int TotalUsedStickersCount { get; set; }
        public int TotalUnusedStickersCount { get; set; }
        public int TotalGreenCardsCount { get; set; }
        public int TotalUsedGreenCardsCount { get; set; }
        public int TotalUnusedGreenCardsCount { get; set; }
        public string? StickerCountFormatted { get; set; }
        public string? GreenCardCountFormatted { get; set; }
        public string? InsuranceCompanyName { get; set; }
    }
}
