using CovrMe.Shared.Enums;

namespace CovrMe.Services.Contracts
{
    public interface IOcrService
    {
        public Task<string> RecognizeImage(byte[] image);
    }
}
