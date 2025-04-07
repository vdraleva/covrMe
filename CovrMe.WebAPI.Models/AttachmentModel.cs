using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models
{
    public class AttachmentModel
    {
        public string? FileName { get; set; }
        public byte[]? ByteArray { get; set; }
        public string? ContentType { get; set; }
    }
}
