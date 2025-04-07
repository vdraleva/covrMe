using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Services.LocalServices.Contracts
{
    public interface IQuestionService
    {
        Task<BaseResultModel> AddQuestions(List<QuestionModel>? questionnaire, string myThingsInsuranceId);
        IQueryable<QuestionModel> GetQuestionsByMyThingsInsuranceId(string myThingsInsuranceId);
    }
}
