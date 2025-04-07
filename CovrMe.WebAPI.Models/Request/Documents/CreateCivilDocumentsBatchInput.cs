using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Documents
{
    public class CreateCivilDocumentsBatchInput
    {
        public CreateCivilDocumentsBatchInput()
        {
            this.Docs = new List<CreateDocumentBatchRequestModel>();
        }

        public List<CreateDocumentBatchRequestModel>? Docs { get; set; }
    }
}
