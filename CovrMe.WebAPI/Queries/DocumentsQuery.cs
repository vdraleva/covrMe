using HotChocolate.Authorization;
using CovrMe.WebAPI.Data;
using CovrMe.WebAPI.Models.Request.Users;
using CovrMe.WebAPI.Models.Result.Documents;
using CovrMe.WebAPI.Models.Result.Users;
using CovrMe.WebAPI.Models.Result.Users.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;

namespace CovrMe.WebAPI.Queries
{
    [ExtendObjectType(typeof(BaseQuery))]
    public class DocumentsQuery
    {
        [GraphQLName("batches")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<BatchModel> GetBatches([Service] IDocumentService documentService)
        {
            var batches = documentService.GetBatches();

            return batches;
        }

        [GraphQLName("batchesForInsuranceCompanies")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<InsuranceCompanyCivilDocumentsModel> GetBatchesForInsuranceCompanies([Service] IDocumentService documentService)
        {
            var batches = documentService.GetBatchesForInsuranceCompanies();

            return batches;
        }

        [GraphQLName("batchesForInsuranceCompany")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<InsuranceCompanyBatchInfoModel> GetBatchesForInsuranceCompany([Service] IDocumentService documentService, string companyId)
        {
            var batches = documentService.GetBatchesForInsuranceCompany(companyId);

            return batches;
        }

        [GraphQLName("batchesByCompanyName")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<BatchModel> GetBatchesByCompanyName([Service] IDocumentService documentService, string companyName)
        {
            var batches = documentService.GetBatchesByCompanyName(companyName);

            return batches;
        }

        [GraphQLName("stickers")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]        
        public IQueryable<StickerModel> GetStickers([Service] IDocumentService documentService)
        {
            var stickers = documentService.GetStickers();

            return stickers;
        }

        [GraphQLName("greencards")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<GreenCardModel> GetGreenCards([Service] IDocumentService documentService)
        {
            var greenCards = documentService.GetGreenCards();

            return greenCards;
        }

        [GraphQLName("stickersByBatchId")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<StickerModel> GetStickersByBatchId([Service] IDocumentService documentService, string batchId)
        {
            var stickers = documentService.GetStickersByBatchId(batchId);

            return stickers;
        }

        [GraphQLName("greencardsByBatchId")]
        [Authorize]
        [UsePaging(IncludeTotalCount = true, MaxPageSize = 100), UseFiltering, UseSorting]
        public IQueryable<GreenCardModel> GetGreenCardsByBatchId([Service] IDocumentService documentService, string batchId)
        {
            var greenCards = documentService.GetGreenCardsByBatchId(batchId);

            return greenCards;
        }
    }
}
