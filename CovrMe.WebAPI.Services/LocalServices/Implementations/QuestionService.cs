using CovrMe.WebAPI.Data.Entities;
using CovrMe.WebAPI.Domain.Repos.Contracts;
using CovrMe.WebAPI.Models.Result;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Services.LocalServices.Contracts;
using CovrMe.WebAPI.Shared.Enums;
using CovrMe.WebAPI.Shared.Helpers;
using CovrMe.WebAPI.Shared.Mapping;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Implementations
{
    public class QuestionService : IQuestionService
    {
        private IRepository<Question> _questionRepository;
        private readonly ILogger _logger;

        public QuestionService(ILogger<QuestionService> logger, IRepository<Question> questionRepository)
        {
            this._logger = logger;
            this._questionRepository = questionRepository;
        }

        public async Task<BaseResultModel> AddQuestions(List<QuestionModel>? questionnaire, string myThingsInsuranceId)
        {
            var result = new BaseResultModel();

            try
            {
                var currentMyThingsInsuranceId = Helpers.ParseStringToGuid(myThingsInsuranceId);

                foreach (var item in questionnaire)
                {
                    var current = new Question
                    {
                        Id = Guid.NewGuid(),
                        QuestionId = item.QuestionId,
                        Answer = item.Answer,
                        MyThingsInsuranceId = currentMyThingsInsuranceId
                    };

                    await this._questionRepository.AddAsync(current);
                }

                await this._questionRepository.SaveChangesAsync();
                result.Code = (int)GeneralStatusEnum.Success;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                result.Code = (int)GeneralStatusEnum.Unsuccess;
            }

            return result;
        }

        public IQueryable<QuestionModel> GetQuestionsByMyThingsInsuranceId(string myThingsInsuranceId)
        {
            var currentMyThingsInsuranceId = Helpers.ParseStringToGuid(myThingsInsuranceId);

            var query = this._questionRepository
                .AllAsNoTracking()
                .Where(x => x.MyThingsInsuranceId == currentMyThingsInsuranceId)
                .To<QuestionModel>();

            return query;
        }
    }
}
