using HotChocolate.Authorization;
using CovrMe.WebAPI.Models.Result.Documents;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Services.LocalServices.Contracts;

namespace CovrMe.WebAPI.Queries
{
    [ExtendObjectType(typeof(BaseQuery))]
    public class QuestionQuery
    {
        [GraphQLName("questions")]
        [Authorize]
        public IQueryable<QuestionModel> GetQuestions([Service] IQuestionService questiontService, string myThingsInsuranceId)
        {
            var questions = questiontService.GetQuestionsByMyThingsInsuranceId(myThingsInsuranceId);

            return questions;
        }
    }
}
