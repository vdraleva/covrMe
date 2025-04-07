using CovrMe.WebAPI.Data.Context;
using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Models.Request.Documents;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Documents;
using CovrMe.WebAPI.Models.Result.Documents.Payloads;
using CovrMe.WebAPI.Models.Result.InsuranceCompanies;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Constants;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Shared.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class DocumentService : IDocumentService
    {
        private IRepository<DocumentsBatch> _batchRepository;
        private IRepository<GreenCard> _greenCardRepository;
        private IRepository<Sticker> _stickerRepository;
        private IRepository<InsuranceCompany> _insuranceCompanyRepository;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        private const int docDigitMinCount = 5;

        public DocumentService(ILogger<DocumentService> logger, IRepository<DocumentsBatch> batchRepository, IRepository<GreenCard> greenCardRepository,
            IRepository<Sticker> stickerRepository, IRepository<InsuranceCompany> insuranceCompanyRepository,
            ApplicationDbContext dbContext)
        {
            this._logger = logger;
            this._batchRepository = batchRepository;
            this._greenCardRepository = greenCardRepository;
            this._stickerRepository = stickerRepository;
            this._insuranceCompanyRepository = insuranceCompanyRepository;
            this._dbContext = dbContext;
        }

        #region Public methods
        public async Task<CreateCivilDocumentsBatchPayload> AddCivilDocumentBatch(CreateCivilDocumentsBatchInput input)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            var result = new CreateCivilDocumentsBatchPayload();
            try
            {            
                foreach (var doc in input.Docs)
                {
                    var hasUncompletedBatch = this.CheckForUncompletedCivilBatch(doc.InsuranceCompanyId);
                    string greenCardSeries = doc.GreenCardNumberFrom.Substring(0, 2);

                    string greenCardFrom = doc.GreenCardNumberFrom.Substring(2);
                    string greenCardTo = doc.GreenCardNumberTo.Substring(2);

                    var greenCardNumbers = GetNumbers(greenCardFrom, greenCardTo);
                    var stickerNumbers = GetNumbers(doc.StickerNumberFrom, doc.StickerNumberTo);

                    var batch = new DocumentsBatch
                    {
                        Id = Guid.NewGuid(),
                        CreationDate = DateTime.Now,
                        IsActive = hasUncompletedBatch ? false : true,
                        IsCompleted = false,
                        InsuranceCompanyId = Helpers.ParseStringToGuid(doc.InsuranceCompanyId),
                        DocsCount = greenCardNumbers.Count(),
                    };

                    await this._batchRepository.AddAsync(batch);

                    for (int i = 0; i < greenCardNumbers.Count; i++)
                    {
                        var currentGreenCard = new GreenCard
                        {
                            Id = Guid.NewGuid(),
                            GreenCardNumber = greenCardSeries + greenCardNumbers[i],
                            IsUsed = false,
                            DocumentsBatchId = batch.Id
                        };

                        await this._greenCardRepository.AddAsync(currentGreenCard);
                    }

                    for (int i = 0; i < stickerNumbers.Count; i++)
                    {
                        var currentSticker = new Sticker
                        {
                            Id = Guid.NewGuid(),
                            StickerNumber = stickerNumbers[i],
                            IsUsed = false,
                            DocumentsBatchId = batch.Id
                        };

                        await this._stickerRepository.AddAsync(currentSticker);
                    }
                }

                await this._batchRepository.SaveChangesAsync();
                await this._greenCardRepository.SaveChangesAsync();
                await this._stickerRepository.SaveChangesAsync();

                result.Code = (int)GeneralStatusEnum.Success;
                transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Message = ex.Message;
                transaction.Rollback();
                return result;
            }
        }
        public async Task<CreateStickerPayload> CreateStickerNumber(CreateStickerNumberInput input)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            var result = new CreateStickerPayload();
            try
            {
                var batchId = Helpers.ParseStringToGuid(input.BatchId);

                var batch = this._batchRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == batchId);
               
                if (batch != null)
                {
                    bool hasUncompletedBatch = this._batchRepository
                    .AllAsNoTracking()
                    .Any(x => x.IsActive && x.Id != batchId && x.InsuranceCompanyId == batch.InsuranceCompanyId);

                    var sticker = this._stickerRepository
                        .AllAsNoTracking()
                        .FirstOrDefault(x => x.StickerNumber == input.StickerNumber && x.DocumentsBatchId == batchId);

                    if (sticker == null)
                    {
                        batch.DocsCount += 1;
                        batch.IsCompleted = false;
                        batch.IsActive = hasUncompletedBatch ? false : true;

                        var currentSticker = new Sticker
                        {
                            Id = Guid.NewGuid(),
                            IsUsed = false,
                            DocumentsBatchId = batchId,
                            StickerNumber = input.StickerNumber
                        };

                        await this._stickerRepository.AddAsync(currentSticker);
                        this._batchRepository.Update(batch);
                        await this._stickerRepository.SaveChangesAsync();
                        await this._batchRepository.SaveChangesAsync();

                        result.Code = (int)GeneralStatusEnum.Success;
                        result.Message = MessageConstants.CreateStickerSuccess;
                        transaction.Commit();

                        return result;
                    }
                }
                result.Code = (int)GeneralStatusEnum.Unsuccess;
                result.Message = MessageConstants.StickerAlreadyExists;
                transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Message = ex.Message;
                transaction.Rollback();
                return result;
            }
        }
        public async Task<CreateGreenCardPayload> CreateGreenCardNumber(CreateGreenCardNumberInput input)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            var result = new CreateGreenCardPayload();
            try
            {
                var batchId = Helpers.ParseStringToGuid(input.BatchId);

                var batch = this._batchRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == batchId);

                if (batch != null)
                {
                    bool hasUncompletedBatch = this._batchRepository
                        .AllAsNoTracking()
                        .Any(x => x.IsActive && x.Id != batchId && x.InsuranceCompanyId == batch.InsuranceCompanyId);

                    var greenCard = this._greenCardRepository
                        .AllAsNoTracking()
                        .FirstOrDefault(x => x.GreenCardNumber == input.GreenCardNumber && x.DocumentsBatchId == batchId);

                    if (greenCard == null)
                    {
                        batch.DocsCount += 1;
                        batch.IsCompleted = false;
                        batch.IsActive = hasUncompletedBatch ? false : true; ;

                        var currentGreenCard = new GreenCard
                        {
                            Id = Guid.NewGuid(),
                            IsUsed = false,
                            DocumentsBatchId = batchId,
                            GreenCardNumber = input.GreenCardNumber
                        };

                        await this._greenCardRepository.AddAsync(currentGreenCard);
                        this._batchRepository.Update(batch);
                        await this._greenCardRepository.SaveChangesAsync();
                        await this._batchRepository.SaveChangesAsync();
                        result.Code = (int)GeneralStatusEnum.Success;
                        result.Message = MessageConstants.CreateGreenCardSuccess;
                        transaction.Commit();

                        return result;
                    }
                }
                result.Code = (int)GeneralStatusEnum.Unsuccess;
                result.Message = MessageConstants.GreenCardAlreadyExists;
                transaction.Commit();

                return result;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Message = ex.Message;
                transaction.Rollback();
                return result;
            }
        }
        public async Task<UpdateStickerPayload> UpdateStickerNumber(UpdateStickerNumberInput input)
        {
            var result = new UpdateStickerPayload();
            try
            {
                var currentId = Helpers.ParseStringToGuid(input.StickerId);

                var sticker = this._stickerRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == currentId);

                if (sticker != null)
                {
                    sticker.StickerNumber = input.StickerNumber;

                    this._stickerRepository.Update(sticker);
                    await this._stickerRepository.SaveChangesAsync();
                }

                result.Code = (int)GeneralStatusEnum.Success;

                return result;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Message = ex.Message;
                return result;
            }
        }
        public async Task<UpdateGreenCardPayload> UpdateGreenCardNumber(UpdateGreenCardNumberInput input)
        {
            var result = new UpdateGreenCardPayload();
            try
            {
                var currentId = Helpers.ParseStringToGuid(input.GreenCardId);

                var greenCard = this._greenCardRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == currentId);

                if (greenCard != null)
                {
                    greenCard.GreenCardNumber = input.GreenCardNumber;

                    this._greenCardRepository.Update(greenCard);
                    await this._greenCardRepository.SaveChangesAsync();
                }

                result.Code = (int)GeneralStatusEnum.Success;

                return result;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Message = ex.Message;
                return result;
            }
        }
        public async Task<CivilInsuranceDocumentsResultModel> GetCivilDocumentByInsuranceCompanyName(string insuranceCompany)
        {
            var result = new CivilInsuranceDocumentsResultModel();

            try
            {
                var batch = this._batchRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.InsuranceCompany.CompanyName == insuranceCompany &&
                                       x.IsCompleted == false && x.IsActive == true);

                if (batch == null)
                {
                    batch = this._batchRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.InsuranceCompany.CompanyName == insuranceCompany &&
                                           x.IsCompleted == false && x.IsActive == false);

                    if (batch == null)
                    {
                        throw new Exception($"There is no batches for {insuranceCompany}");
                    }
                }

                var greenCardId = GetGreenCardFromBatch(batch.Id);
                var stickerId = GetStickerFromBatch(batch.Id);
                string currentBatchId = batch.Id.ToString();

                if (!string.IsNullOrEmpty(stickerId) && !string.IsNullOrEmpty(greenCardId))
                {
                    result.StickerId = stickerId;
                    result.GreenCardId = greenCardId;
                    result.BatchId = currentBatchId;

                    await this.UpdateCivilDocumentStatus(stickerId, greenCardId);
                }

                return result;
            }
            catch (Exception ex)
            {
                await UpdateCivilDocumentStatus(result.StickerId, result.GreenCardId); // return the stickers and greencards
                _logger.LogError(ex.Message);
                return result;
            }
        }
        public async Task UpdateCivilDocumentStatus(string stickerId, string greenCardId)
        {
            var currentStickerId = Helpers.ParseStringToGuid(stickerId);
            var currentGreenCardId = Helpers.ParseStringToGuid(greenCardId);

            var updatedStickerBatchId = await this.UpdateStickerStatus(currentStickerId);
            var updateGreenCardBatchId = await this.UpdateGreenCardStatus(currentGreenCardId);

            if (!string.IsNullOrEmpty(updatedStickerBatchId) && !string.IsNullOrEmpty(updateGreenCardBatchId))
            {
                var currentBatch = this._batchRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == Helpers.ParseStringToGuid(updatedStickerBatchId));

                if (currentBatch != null)
                {
                    bool hasFree = this.CheckForFreeStickersByBatch(currentBatch.Id);

                    if (hasFree)
                    {
                        currentBatch.IsCompleted = false;
                        currentBatch.IsActive = true;

                        this._batchRepository.Update(currentBatch);
                        await this._batchRepository.SaveChangesAsync();
                    }
                    else
                    {
                        currentBatch.IsCompleted = true;
                        currentBatch.IsActive = false;

                        this._batchRepository.Update(currentBatch);
                        await this._batchRepository.SaveChangesAsync();
                    }
                }
            }

        }
        public string GetGreenCardNumberById(string greenCardId)
        {
            var currentGreenCardId = Helpers.ParseStringToGuid(greenCardId);

            var greenCard = this._greenCardRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == currentGreenCardId);

            if (greenCard != null)
            {
                return greenCard.GreenCardNumber;
            }

            return string.Empty;
        }
        public string GetStickerNumberById(string stickerId)
        {
            var currentStickerId = Helpers.ParseStringToGuid(stickerId);

            var sticker = this._stickerRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == currentStickerId);

            if (sticker != null)
            {
                return sticker.StickerNumber;
            }

            return string.Empty;
        }
        public bool CheckForFreeCivilDocs(string insuranceCompany)
        {
            var batch = this._batchRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.InsuranceCompany.CompanyName == insuranceCompany &&
                                     x.IsCompleted == false);

            if (batch != null)
            {
                return true;
            }

            return false;
        }
        public IQueryable<BatchModel> GetBatches()
        {
            var batches = this._batchRepository
                .AllAsNoTracking()
                .Join(
                    this._insuranceCompanyRepository.AllAsNoTracking(),
                    batch => batch.InsuranceCompanyId,
                    company => company.Id,
                    (batch, company) => new BatchModel
                    {
                        Id = batch.Id.ToString(),
                        DocsCount = batch.DocsCount,
                        CreationDate = batch.CreationDate,
                        InsuranceCompanyId = company.Id.ToString(),
                        InsuranceCompanyName = company.CompanyName,
                        IsActive = batch.IsActive,
                        IsCompleted = batch.IsCompleted,
                    })
                .OrderBy(s => s.InsuranceCompanyName)
                .ThenBy(x => x.CreationDate);

            return batches;
        }

        public IQueryable<InsuranceCompanyCivilDocumentsModel> GetBatchesForInsuranceCompanies()
        {
            var result = this._insuranceCompanyRepository.AllAsNoTracking()
                    .Where(ic => ic.CompanyName != GlobalConstants.Uniqa)
                    .Select(ic => new InsuranceCompanyCivilDocumentsModel
                    {
                        InsuranceCompanyId = ic.Id.ToString(),
                        InsuranceCompanyName = ic.CompanyName,
                        TotalStickersCount = this._stickerRepository.AllAsNoTracking().Count(s => s.DocumentsBatch.InsuranceCompanyId == ic.Id),
                        TotalUsedStickersCount = this._stickerRepository.AllAsNoTracking().Count(s => s.DocumentsBatch.InsuranceCompanyId == ic.Id && s.IsUsed),
                        TotalUnusedStickersCount = this._stickerRepository.AllAsNoTracking().Count(s => s.DocumentsBatch.InsuranceCompanyId == ic.Id && !s.IsUsed),
                        TotalGreenCardsCount = this._greenCardRepository.AllAsNoTracking().Count(gc => gc.DocumentsBatch.InsuranceCompanyId == ic.Id),
                        TotalUsedGreenCardsCount = this._greenCardRepository.AllAsNoTracking().Count(gc => gc.DocumentsBatch.InsuranceCompanyId == ic.Id && gc.IsUsed),
                        TotalUnusedGreenCardsCount = this._greenCardRepository.AllAsNoTracking().Count(gc => gc.DocumentsBatch.InsuranceCompanyId == ic.Id && !gc.IsUsed),
                        GreenCardCountFormatted = $"{this._greenCardRepository.AllAsNoTracking().Count(gc => gc.DocumentsBatch.InsuranceCompanyId == ic.Id && !gc.IsUsed)} / {this._greenCardRepository.AllAsNoTracking().Count(gc => gc.DocumentsBatch.InsuranceCompanyId == ic.Id)}",
                        StickerCountFormatted = $"{this._stickerRepository.AllAsNoTracking().Count(s => s.DocumentsBatch.InsuranceCompanyId == ic.Id && !s.IsUsed)} / {this._stickerRepository.AllAsNoTracking().Count(s => s.DocumentsBatch.InsuranceCompanyId == ic.Id)}",


                    });

            return result;
        }

        public IQueryable<InsuranceCompanyBatchInfoModel> GetBatchesForInsuranceCompany(string companyId)
        {
            var currentCompanyId = Helpers.ParseStringToGuid(companyId);

            var batches = this._batchRepository
                .AllAsNoTracking()
                .Where(x => x.InsuranceCompanyId == currentCompanyId)
                .Select(x => new InsuranceCompanyBatchInfoModel
                {
                    BatchId = x.Id.ToString(),
                    CreationDate = x.CreationDate,
                    IsActive = x.IsActive,
                    IsCompleted = x.IsCompleted,
                    TotalStickersCount = x.Stickers.Count(),
                    TotalUsedStickersCount = x.Stickers.Count(x => x.IsUsed),
                    TotalUnusedStickersCount = x.Stickers.Count(x => !x.IsUsed),
                    TotalGreenCardsCount = x.GreenCards.Count(),
                    TotalUnusedGreenCardsCount = x.GreenCards.Count(x => !x.IsUsed),
                    TotalUsedGreenCardsCount = x.GreenCards.Count(x => x.IsUsed),
                    GreenCardCountFormatted = $"{x.GreenCards.Count(x => !x.IsUsed)} / {x.GreenCards.Count()}",
                    StickerCountFormatted = $"{x.Stickers.Count(x => !x.IsUsed)} / {x.Stickers.Count()}",
                    InsuranceCompanyName = x.InsuranceCompany.CompanyName

                });

            return batches;
        }

        public IQueryable<BatchModel> GetBatchesByCompanyName(string companyName)
        {
            var batches = this._batchRepository
                .AllAsNoTracking()
                .Where(b => b.InsuranceCompany.CompanyName == companyName)
                .ToList();


            var batchModels = batches.Select(s => new BatchModel
            {
                Id = s.Id.ToString(),
                DocsCount = s.DocsCount,
                CreationDate = s.CreationDate,
                InsuranceCompanyName = companyName,
                InsuranceCompanyId = s.InsuranceCompanyId.ToString(),
                IsActive = s.IsActive,
                IsCompleted = s.IsCompleted
            })
                .OrderBy(s => s.InsuranceCompanyName)
                .OrderByDescending(x => x.CreationDate);

            return batchModels.AsQueryable();
        }
        public IQueryable<StickerModel> GetStickers()
        {
            var stickers = this._stickerRepository
                .AllAsNoTracking()
                .Include(x => x.DocumentsBatch)
                .ToList();

            var stickerModels = stickers
                .Join(
                    this._batchRepository.AllAsNoTracking(),
                    sticker => sticker.DocumentsBatchId,
                    batch => batch.Id,
                    (sticker, batch) => new { Sticker = sticker, Batch = batch })
                .Join(
                    this._insuranceCompanyRepository.AllAsNoTracking(),
                    combined => combined.Batch.InsuranceCompanyId,
                    company => company.Id,
                    (combined, company) => new StickerModel
                    {
                        Id = combined.Sticker.Id.ToString(),
                        DocumentsBatchId = combined.Sticker.DocumentsBatchId.ToString(),
                        InsuranceCompanyId = company.Id.ToString(),
                        InsuranceCompanyName = company.CompanyName,
                        StickerNumber = combined.Sticker.StickerNumber,
                        IsActive = combined.Batch.IsActive,
                        IsUsed = combined.Sticker.IsUsed,
                        UsedStickers = combined.Batch.Stickers.Count(st => st.IsUsed),
                        TotalStickers = combined.Batch.Stickers.Count
                    })
                .OrderBy(s => s.InsuranceCompanyName);

            return stickerModels.AsQueryable();
        }

        public IQueryable<GreenCardModel> GetGreenCards()
        {
            var greenCards = this._greenCardRepository
                 .AllAsNoTracking()
                 .Include(x => x.DocumentsBatch)
                 .ToList();

            var insCompanies = GetAllInsuranceCompanies();

            var greenCardModels = greenCards.Select(s => new GreenCardModel
            {
                Id = s.Id.ToString(),
                DocumentsBatchId = s.DocumentsBatchId.ToString(),
                InsuranceCompanyName = GetInsuranceCompanyNameByBatchId(s.DocumentsBatchId),
                GreenCardNumber = s.GreenCardNumber,
                IsActive = s.DocumentsBatch.IsActive,
                IsUsed = s.IsUsed,
                UsedGreenCards = s.DocumentsBatch?.GreenCards.Count(st => st.IsUsed) ?? 0,
                TotalGreenCards = s.DocumentsBatch?.GreenCards.Count ?? 0
            }).OrderBy(s => s.InsuranceCompanyName);

            return greenCardModels.AsQueryable();
        }
        public IQueryable<StickerModel> GetStickersByBatchId(string batchId)
        {
            var currentBatchId = Helpers.ParseStringToGuid(batchId);

            var stickerModels = this._stickerRepository
                .AllAsNoTracking()
                .Include(x => x.DocumentsBatch)
                .Where(x => x.DocumentsBatchId == currentBatchId)
                .Select(s => new StickerModel
                {
                    Id = s.Id.ToString(),
                    DocumentsBatchId = s.DocumentsBatchId.ToString(),
                    InsuranceCompanyId = GetInsuranceCompanyIdByBatchId(currentBatchId),
                    InsuranceCompanyName = GetInsuranceCompanyNameByBatchId(currentBatchId),
                    StickerNumber = s.StickerNumber,
                    IsActive = s.DocumentsBatch.IsActive,
                    IsUsed = s.IsUsed,
                    UsedStickers = s.DocumentsBatch.Stickers.Count(st => st.IsUsed),
                    TotalStickers = s.DocumentsBatch.Stickers.Count
                })
                .OrderBy(s => s.InsuranceCompanyName);

            return stickerModels.AsQueryable();
        }

        public IQueryable<GreenCardModel> GetGreenCardsByBatchId(string batchId)
        {
            var currentBatchId = Helpers.ParseStringToGuid(batchId);

            var greenCardModels = this._greenCardRepository
                .AllAsNoTracking()
                .Include(x => x.DocumentsBatch)
                .Where(x => x.DocumentsBatchId == currentBatchId)
                .Select(s => new GreenCardModel
                {
                    Id = s.Id.ToString(),
                    DocumentsBatchId = s.DocumentsBatchId.ToString(),
                    InsuranceCompanyName = GetInsuranceCompanyNameByBatchId(currentBatchId),
                    GreenCardNumber = s.GreenCardNumber,
                    IsActive = s.DocumentsBatch.IsActive,
                    IsUsed = s.IsUsed,
                    UsedGreenCards = s.DocumentsBatch.GreenCards.Count(st => st.IsUsed),
                    TotalGreenCards = s.DocumentsBatch.GreenCards.Count
                })
                .OrderBy(s => s.InsuranceCompanyName);

            return greenCardModels.AsQueryable();
        }

        public async Task<BaseResultModel> StickerGreenCardErrorOccured(string stickerId, string greenCardId)
        {
            var result = new BaseResultModel();

            try
            {
                var currentStickerId = Helpers.ParseStringToGuid(stickerId);
                var currentGreenCardId = Helpers.ParseStringToGuid(greenCardId);

                var sticker = this._stickerRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == currentStickerId);

                var greenCard = this._greenCardRepository
                    .AllAsNoTracking()
                    .FirstOrDefault(x => x.Id == currentGreenCardId);

                if (sticker != null)
                {
                    sticker.IsError = true;
                    this._stickerRepository.Update(sticker);
                    await this._stickerRepository.SaveChangesAsync();
                }

                if (greenCard != null)
                {
                    greenCard.IsError = true;
                    this._greenCardRepository.Update(greenCard);
                    await this._greenCardRepository.SaveChangesAsync();
                }

                result.Code = (int)GeneralStatusEnum.Success;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Code = (int)GeneralStatusEnum.Unsuccess;
            }

            return result;
        }
        #endregion

        #region Private methods
        private string GetInsuranceCompanyNameByBatchId(Guid batchId)
        {
            var batch = this._batchRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == batchId);

            if (batch != null)
            {
                var companyId = batch.InsuranceCompanyId;
                var company = GetAllInsuranceCompanies().FirstOrDefault(ic => ic.Id == companyId);
                return company?.CompanyName ?? "Unknown";
            }

            return "Unknown";
        }
        private string GetInsuranceCompanyIdByBatchId(Guid batchId)
        {
            var batch = this._batchRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == batchId);

            if (batch != null)
            {
                var companyId = batch.InsuranceCompanyId;

                return companyId.ToString();
            }

            return "Unknown";
        }
        private bool CheckForFreeStickersByBatch(Guid batchId)
        {
            var hasFree = this._stickerRepository
                .AllAsNoTracking()
                .Where(x => x.DocumentsBatchId == batchId)
                .Any(x => x.IsUsed == false);

            return hasFree;
        }
        private string GetGreenCardFromBatch(Guid batchId)
        {
            string result = string.Empty;

            var greenCard = this._greenCardRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.DocumentsBatchId == batchId && x.IsUsed == false);

            if (greenCard != null)
            {
                result = greenCard.Id.ToString();
            }

            return result;
        }
        private string GetStickerFromBatch(Guid batchId)
        {
            string result = string.Empty;

            var sticker = this._stickerRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.DocumentsBatchId == batchId && x.IsUsed == false);

            if (sticker != null)
            {
                result = sticker.Id.ToString();
            }

            return result;
        }
        private async Task<bool> ActivateCivilBatch(Guid batchId)
        {
            bool result = false;

            var batch = this._batchRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == batchId);

            if (batch != null)
            {
                batch.IsActive = true;

                this._batchRepository.Update(batch);
                await this._batchRepository.SaveChangesAsync();

                result = true;
            }

            return result;
        }
        private async Task<string> UpdateGreenCardStatus(Guid greenCardId)
        {
            string result = string.Empty;

            var greenCard = this._greenCardRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == greenCardId);

            if (greenCard != null)
            {
                if (greenCard.IsUsed)
                {
                    greenCard.IsUsed = false;
                }
                else
                {
                    greenCard.IsUsed = true;
                }

                this._greenCardRepository.Update(greenCard);
                await this._greenCardRepository.SaveChangesAsync();

                result = greenCard.DocumentsBatchId.ToString();
            }

            return result;
        }
        private async Task<string> UpdateStickerStatus(Guid stickerId)
        {
            string result = string.Empty;

            var sticker = this._stickerRepository
                .AllAsNoTracking()
                .FirstOrDefault(x => x.Id == stickerId);

            if (sticker != null)
            {
                if (sticker.IsUsed)
                {
                    sticker.IsUsed = false;
                }
                else
                {
                    sticker.IsUsed = true;
                }

                this._stickerRepository.Update(sticker);
                await this._stickerRepository.SaveChangesAsync();

                result = sticker.DocumentsBatchId.ToString();
            }

            return result;
        }
        private List<string> GetNumbers(string numberFrom, string numberTo)
        {
            string numFromWithoutZeroes = numberFrom.TrimStart('0');
            string numToWithoutZeroes = numberTo.TrimStart('0');

            int numFrom = int.Parse(numFromWithoutZeroes);
            int numTo = int.Parse(numToWithoutZeroes);

            var result = new List<string>();

            for (int i = numFrom; i <= numTo; i++)
            {
                var currentNumber = i;
                var currentNumberString = currentNumber.ToString();
                var currentNumberLenght = currentNumberString.Length;

                if (currentNumberLenght < docDigitMinCount)
                {
                    currentNumberString = currentNumberString.PadLeft(docDigitMinCount, '0');
                }

                result.Add(currentNumberString);
            }

            return result;
        }
        private bool CheckForUncompletedCivilBatch(string companyId)
        {
            var currentCompanyId = Helpers.ParseStringToGuid(companyId);

            var hasUncompleted = this._batchRepository
                .AllAsNoTracking()
                .Any(x => x.IsCompleted == false && x.InsuranceCompanyId == currentCompanyId);

            return hasUncompleted;
        }
        private IQueryable<T> GetStickersByBatchId<T>(Guid batchId)
        {
            var stickers = this._stickerRepository
                .AllAsNoTracking()
                .Where(x => x.DocumentsBatchId.Equals(batchId))
                .To<T>();

            return stickers;
        }
        private T GetStickerById<T>(string id)
        {
            var currentId = Helpers.ParseStringToGuid(id);

            var sticker = this._stickerRepository
                 .AllAsNoTracking()
                .Where(x => x.Id.Equals(currentId))
                .To<T>()
                .FirstOrDefault();

            return sticker;
        }
        private List<InsuranceCompany> GetAllInsuranceCompanies()
        {
            var query = this._insuranceCompanyRepository
                .AllAsNoTracking()
                .ToList();

            return query;
        }
        private T GetInsuranceById<T>(Guid insuranceId)
        {
            var query = this._insuranceCompanyRepository
                .AllAsNoTracking()
                .Where(x => x.Id == insuranceId)
                .To<T>()
                .FirstOrDefault();

            return query;
        }

        #endregion
    }
}
