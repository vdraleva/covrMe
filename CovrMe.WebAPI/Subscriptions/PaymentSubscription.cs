using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Queries;

namespace CovrMe.WebAPI.Subscriptions
{
    [ExtendObjectType(typeof(BaseSubscription))]
    public class PaymentSubscription
    {
        [Subscribe]
        [Topic($"{{{nameof(localOrderId)}}}")]
        public BaseResultModel PaymentStatus(string localOrderId,[EventMessage] BaseResultModel result) => result;
    }
}
