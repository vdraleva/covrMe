using CovrMe.Models.Vehicles.Result;
using CovrMe.Shared.Enums;

namespace CovrMe.Services.Contracts
{
    public interface IRegExtractorRegexService
    {
        public Task<RegCertificateResultModel> ParseToModel(byte[] image, DocumentTypeEnum docType);
        public bool IsFront(RegCertificateResultModel regCertificateModel);
        public bool IsPhotoBad(RegCertificateResultModel regCertificateModel, DocumentTypeEnum documentType);
    }
}
