using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Shared.Templates
{
    public class PushTemplates
    {
        public class Generic
        {            
            public const string AndroidAWS = "{ \"default\": \"Sample fallback message\",\"GCM\" : \"{\\\"data\\\" : { \\\"header\\\" : \\\"$(header)\\\", \\\"text\\\" : \\\"$(textMessage)\\\"} }\"}";

            public const string AppleAWS_Dev = "{ \"APNS_SANDBOX\": \"{\\\"aps\\\":{\\\"alert\\\":{\\\"title\\\":\\\"$(header)\\\",\\\"body\\\":\\\"$(textMessage)\\\"}}}\"}";

            public const string AppleAWS_Live = "{ \"APNS\": \"{\\\"aps\\\":{\\\"alert\\\":{\\\"title\\\":\\\"$(header)\\\",\\\"body\\\":\\\"$(textMessage)\\\"}}}\"}";      
        }
    }
}
