using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models
{
    public class MailDataWithAttachment : MailData
    {
        public List<AttachmentModel>? EmailAttachments { get; set; }
    }
}
