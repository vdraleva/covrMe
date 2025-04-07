using CovrMe.WebAPI.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CovrMe.WebAPI.Services.Messaging
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<MailService> _logger;
        public MailService(IOptions<MailSettings> mailSettingsOptions, ILogger<MailService> logger)
        {
            _mailSettings = mailSettingsOptions.Value;
            _logger = logger;
        }

        public bool SendMail(MailData mailData)
        {
            try
            {
                if (string.IsNullOrEmpty(mailData.EmailToId) && string.IsNullOrEmpty(mailData.EmailToName))
                {
                    mailData.EmailToName = _mailSettings.EmailTo;
                    mailData.EmailToId = _mailSettings.EmailTo;
                }
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.Auto);
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
                return false;
            }
        }

        public bool SendMailWithAttachment(MailDataWithAttachment mailData)
        {
            try
            {
                if (string.IsNullOrEmpty(mailData.EmailToId) && string.IsNullOrEmpty(mailData.EmailToName))
                {
                    mailData.EmailToName = _mailSettings.EmailTo;
                    mailData.EmailToId = _mailSettings.EmailTo;
                    mailData.EmailToCc = _mailSettings.EmailToCc;
                }
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    if (!string.IsNullOrEmpty(mailData.EmailToCc))
                    {
                        MailboxAddress emailToCc = new MailboxAddress(mailData.EmailToCc, mailData.EmailToCc);
                        emailMessage.Cc.Add(emailToCc);
                    }
                    
                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.HtmlBody = mailData.EmailBody;
                   
                    if(mailData.EmailAttachments != null)
                    {
                        foreach (var attachment in mailData.EmailAttachments)
                        {
                            if(attachment.ByteArray.Length == 0)
                            {
                                continue;
                            }

                            emailBodyBuilder.Attachments.Add(attachment.FileName, attachment.ByteArray, ContentType.Parse(attachment.ContentType));
                        }
                    }

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.Auto);
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
