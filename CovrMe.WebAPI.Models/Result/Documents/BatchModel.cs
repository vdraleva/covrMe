using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Documents
{
    public class BatchModel
    {
        public string Id { get; set; }
        public string InsuranceCompanyId { get; set; }
        public string InsuranceCompanyName { get; set; }
        public int DocsCount { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
