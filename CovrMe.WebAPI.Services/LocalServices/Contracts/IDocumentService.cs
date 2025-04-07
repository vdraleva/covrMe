using CovrMe.WebAPI.Models.Request.Documents;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Documents;
using CovrMe.WebAPI.Models.Result.Documents.Payloads;
using CovrMe.WebAPI.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IDocumentService
    {
        Task<CreateCivilDocumentsBatchPayload> AddCivilDocumentBatch(CreateCivilDocumentsBatchInput input);
        Task<CreateStickerPayload> CreateStickerNumber(CreateStickerNumberInput input);
        Task<CreateGreenCardPayload> CreateGreenCardNumber(CreateGreenCardNumberInput input);
        Task UpdateCivilDocumentStatus(string stickerId, string greenCardId);
        Task<UpdateStickerPayload> UpdateStickerNumber(UpdateStickerNumberInput input);
        Task<UpdateGreenCardPayload> UpdateGreenCardNumber(UpdateGreenCardNumberInput input);
        Task<CivilInsuranceDocumentsResultModel> GetCivilDocumentByInsuranceCompanyName(string insuranceCompany);
        bool CheckForFreeCivilDocs(string insuranceCompany);
        string GetGreenCardNumberById(string greenCardId);
        string GetStickerNumberById(string stickerId);
        IQueryable<BatchModel> GetBatches();
        IQueryable<BatchModel> GetBatchesByCompanyName(string companyName);
        IQueryable<StickerModel> GetStickers();
        IQueryable<GreenCardModel> GetGreenCards();
        IQueryable<StickerModel> GetStickersByBatchId(string batchId);
        IQueryable<GreenCardModel> GetGreenCardsByBatchId(string batchId);
        IQueryable<InsuranceCompanyCivilDocumentsModel> GetBatchesForInsuranceCompanies();
        IQueryable<InsuranceCompanyBatchInfoModel> GetBatchesForInsuranceCompany(string companyId);
        Task<BaseResultModel> StickerGreenCardErrorOccured(string stickerId, string greenCardId);
    }
}
