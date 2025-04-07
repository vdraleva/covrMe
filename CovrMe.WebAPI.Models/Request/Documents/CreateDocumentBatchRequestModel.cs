using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Documents
{
    public class CreateDocumentBatchRequestModel
    {
        public string? InsuranceCompanyId { get; set; }
        public string? GreenCardNumberFrom { get; set; }
        public string? GreenCardNumberTo { get; set; }
        public string? StickerNumberFrom { get; set; }
        public string?StickerNumberTo { get; set; }
    }
}
