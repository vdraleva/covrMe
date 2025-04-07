using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Documents
{
    public class UpdateStickerNumberInput
    {
        public string StickerId { get; set; }
        public string StickerNumber { get; set;}
    }
}
