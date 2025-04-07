using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Currencies
{
    public class CurrencyModel : IMapFrom<Currency>
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = null!;
    }
}
