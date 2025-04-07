using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Request.Documents;
using CovrMe.WebAPI.Models.Request.Speedy;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Documents.Payloads;
using CovrMe.WebAPI.Models.Result.Speedy.Payloads;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Mutations
{
    [ExtendObjectType(typeof(BaseMutation))]
    public class DocumentsMutation
    {
        [Authorize]
        public async Task<CreateCivilDocumentsBatchPayload> CreateCivilDocumentsBatch([Service] IDocumentService documentService, CreateCivilDocumentsBatchInput input)
        {
            var result = await documentService.AddCivilDocumentBatch(input);

            return result;
        }

        [Authorize]
        public async Task<CreateStickerPayload> CreateSticker([Service] IDocumentService documentService, CreateStickerNumberInput input)
        {
            var result = await documentService.CreateStickerNumber(input);

            return result;
        }

        [Authorize]
        public async Task<CreateGreenCardPayload> CreateGreenCard([Service] IDocumentService documentService, CreateGreenCardNumberInput input)
        {
            var result = await documentService.CreateGreenCardNumber(input);

            return result;
        }

        [Authorize]
        public async Task<UpdateStickerPayload> UpdateSticker([Service] IDocumentService documentService, UpdateStickerNumberInput input)
        {
            var result = await documentService.UpdateStickerNumber(input);

            return result;
        }

        [Authorize]
        public async Task<UpdateGreenCardPayload> UpdateGreenCard([Service] IDocumentService documentService, UpdateGreenCardNumberInput input)
        {
            var result = await documentService.UpdateGreenCardNumber(input);

            return result;
        }
    }
}
