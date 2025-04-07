using CovrMe.WebAPI.Shared.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Shared.Helpers
{
    public static class Helpers
    {
        public static Guid ParseStringToGuid(string value)
        {
            Guid guid;
            bool parsed = Guid.TryParse(value, out guid);

            if (parsed)
            {
                return guid;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public static string GenerateVerificationCode()
        {
            var generator = new Random();
            var randomCode = generator.Next(0, 9999).ToString("D4");

            return randomCode;
        }

        public static HttpClientHandler GenerateHttpClientHandler(string certificatePath)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.SslProtocols = SslProtocols.Tls12;
            handler.ClientCertificates.Add(new X509Certificate2(certificatePath, "" ,X509KeyStorageFlags.MachineKeySet));

            return handler;
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateGreenCardNumber()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string prefix = new string(Enumerable.Repeat(chars, 2)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            const string charsNum = "0123456789";
            string generated =  new string(Enumerable.Repeat(charsNum, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return prefix + generated;
        }

        public static string GenerateStickerNumber()
        {
            Random random = new Random();
            const string chars = "0123456789";
            string generated = new string(Enumerable.Repeat(chars, 7)
                .Select(s => s[random.Next(s.Length)]).ToArray());

            return  generated;
        }

        public static string GetEnumDescription(Enum enumVal)
        {
            MemberInfo[] memInfo = enumVal.GetType().GetMember(enumVal.ToString());
            DescriptionAttribute attribute = CustomAttributeExtensions.GetCustomAttribute<DescriptionAttribute>(memInfo[0]);
            return attribute.Description;
        }
        public static int GetRandomNumber()
        {
            Random random = new Random();
            int num = random.Next();

            return num;
        }

        public static bool IsValidCivilExceptionMessage(string message)
        {
            bool isValid = false;

            if (Regex.IsMatch(message, GlobalConstants.OnlyCyrilicRegex))
            {
                isValid = true;
            }
            else if (message.StartsWith(GlobalConstants.ErrorWord) && !message.Contains(GlobalConstants.SqlWord) && !message.Contains(GlobalConstants.ExceptionWord))
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
