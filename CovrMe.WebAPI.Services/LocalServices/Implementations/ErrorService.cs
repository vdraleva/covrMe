using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Models;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Services.Messaging;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class ErrorService : IErrorService
    {
        private readonly IMailService _mailService;
        private IConfiguration _configuration;
        private string _path;
        public ErrorService(IMailService mailService, IConfiguration configuration)
        {
            this._mailService = mailService;
            _configuration = configuration;

            _path = configuration
                    .GetSection("ReadWriteOptions")
                    .GetSection("LoggerPath")
                    .Value;
        }

        public bool SendSpeedyErrorEmail(string policyNo, string names, string phone)
        {
            string email = GlobalConstants.SpeedyErrorEmаil;
            string subject = GlobalConstants.SpeedyErrorEmailHeader;

            email = email.Replace("$policyNo$", policyNo);
            email = email.Replace("$names$", names);
            email = email.Replace("$phone$", phone);

            var mailData = new MailData();
            mailData.EmailSubject = subject;
            mailData.EmailBody = email;

            var result = _mailService.SendMail(mailData);

            return result;
        }

        public bool SendInsuranceErrorEmail(string names, string phone, string policyNo, string insuranceType)
        {
            string email = GlobalConstants.InsuranceErrorEmаil;
            string subject = GlobalConstants.InsuranceErrorHeader;

            email = email.Replace("$names$", names);
            email = email.Replace("$phone$", phone);
            email = email.Replace("$policyNo$", policyNo);

            var mailData = new MailData();
            mailData.EmailSubject = subject;
            mailData.EmailBody = email;

            var result = _mailService.SendMail(mailData);

            return result;
        }

        public void LogError(string message, string folferPath)
        {
            var logPath = this._path + folferPath;
            if (!File.Exists(logPath))
            {
                using (StreamWriter sw = File.CreateText(logPath))
                {
                    LogError(sw, message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    LogError(sw, message);
                }
            }
        }

        private static void LogError(StreamWriter sw, string message)
        {
            sw.WriteLine($"{message}");
        }
    }
}
