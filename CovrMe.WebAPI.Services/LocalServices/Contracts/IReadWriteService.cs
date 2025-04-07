using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IReadWriteService
    {
        int SaveFile(string filePath, byte[] file);
        bool CheckIfFileExists(string path);
        byte[] GetFile(string filePath);
        void DeleteFile(string path);
        string ReadFile(string path);
        int WriteTextToFile(string path, string text);
    }
}
