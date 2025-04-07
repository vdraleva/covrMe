using CovrMe.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.Messaging
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
        bool SendMailWithAttachment(MailDataWithAttachment mailData);
    }
}
