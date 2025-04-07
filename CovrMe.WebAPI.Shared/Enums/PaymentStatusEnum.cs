using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Shared.Enums
{
    public enum PaymentStatusEnum
    {
        approved = 0,
        deposited = 1,
        reversed = 2,
        refunded = 3,
        bindingCreated = 4,
        bindingActivityChanged = 5,
        declinedByTimeout = 6,
        declinedCardPresent = 7,
        awaiting = 8,
    }
}
