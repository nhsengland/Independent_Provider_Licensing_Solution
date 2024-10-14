using Domain.Models.Forms.Rules;
using Domain.Models.ViewModels.Application;

namespace Domain.Logic.Forms.Application.Rules;

public interface IApplicationBusinessRules
{
    RuleOutcomeDTO EvaluateDate(int? day, int? month, int? year);

    RuleOutcomeDTO IsDirectorsDateOfBirthValid(DateOnly dateOfBirth);

    RuleOutcomeDTO IsLastFinancialYearEndDateValid(DateOnly date);

    RuleOutcomeDTO IsNextFinancialYearEndDateValid(DateOnly date);

    RuleOutcomeDTO AllowedToSubmitApplication(
        ReviewApplicationResponsesViewModel applicationResponses);
}
