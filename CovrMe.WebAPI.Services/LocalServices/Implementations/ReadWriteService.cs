using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class ReadWriteService : IReadWriteService
    {
        private string _path;
        private IConfiguration _configuration;
        private readonly ILogger _logger;
        public ReadWriteService(IConfiguration configuration, ILogger<ReadWriteService> logger)
        {
            _path = configuration
                    .GetSection("ReadWriteOptions")
                    .GetSection("UserPolicyPath")
                    .Value;

            _logger = logger;   

        }

        public int SaveFile(string filePath, byte[] file)
        {
            int result = 0;
            try
            {             
                var path = Path.Combine(this._path, filePath);
                string directoryPath = Path.GetDirectoryName(path);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                if (!File.Exists(path))
                {
                    WriteContent(path, file);
                }
                else
                {
                    File.Delete(path);

                    WriteContent(path, file);
                }

                result = 1;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }
        }

        public int WriteTextToFile(string path, string text)
        {
            int result = 0;

            try
            {
                string directoryPath = Path.GetDirectoryName(path);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(path, text);

                result = 1;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return result;
            }
        }
        public byte[] GetFile(string filePath)
        {
            var path = Path.Combine(this._path, filePath);

            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return bytes;
        }

        public bool CheckIfFileExists(string path)
        {
            return File.Exists(path);
        }

        public string ReadFile(string path)
        {
            string text = string.Empty;

            if (this.CheckIfFileExists(path))
            {
                text = File.ReadAllText(path);
            }
            
            return text;
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
            Thread.Sleep(30);
        }

        private byte[] ConvertStringToByte(string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);

            return bytes;
        }
        private void WriteContent(string path, byte[] file)
        {

            File.WriteAllBytes(path, file);
        }
    }
}
