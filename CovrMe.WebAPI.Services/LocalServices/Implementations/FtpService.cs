using CovrMe.WebAPI.Services.LocalServices.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class FtpService : IFtpService
    {
        private string _hostName;
        private string _password;
        private string _userName;
        private readonly ILogger _logger;

        public FtpService(IConfiguration configuration, ILogger<ReadWriteService> logger)
        {
            this._hostName = configuration
                    .GetSection("FtpSettings")
                    .GetSection("FtpHostName")
                    .Value;

            this._userName = configuration
                    .GetSection("FtpSettings")
                    .GetSection("FtpUserName")
                    .Value;

            this._password = configuration
                    .GetSection("FtpSettings")
                    .GetSection("FtpPassword")
                    .Value;

            _logger = logger;


            //This code is dangerous! It should be changes when the time comes
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, errors) =>
                {
                    return true;
                };

        }

        public void Save(byte[] file, string policyNo)
        {
            string filePath = "ftp://" + this._hostName + policyNo + ".pdf";

            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(this._userName, this._password);
                request.EnableSsl = true;
                request.UseBinary = true;
                request.KeepAlive = true;

                request.ContentLength = file.Length;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(file, 0, file.Length);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }
        }
    }
}
