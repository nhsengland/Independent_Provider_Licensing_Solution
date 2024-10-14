using Domain.Logic.Forms.Helpers;
using Domain.Models.Forms.Rules;
using Domain.Models.ViewModels.Application;

namespace Domain.Logic.Forms.Application.Rules;

public class ApplicationBusinessRules(
    IDateEvaluation dateEvaluation) : IApplicationBusinessRules
{
    private readonly IDateEvaluation dateEvaluation = dateEvaluation;

    public RuleOutcomeDTO AllowedToSubmitApplication(
        ReviewApplicationResponsesViewModel applicationResponses)
    {
        var outcome = new RuleOutcomeDTO();

        if(applicationResponses.CompanyDetails.Any(cd => cd.IsComplete == false))
        {
            outcome.ErrorMessages.Add("Please complete all company questions");
        }

        var corporateDirectors = applicationResponses.CorporateDirectorGroups
            .Where(cd => cd.OneOrMoreIndividuals == true)
            .Select(cd => cd.Directors).Count();

        var parenCompanyDirectors = applicationResponses.ParentCompanyGroups
            .Where(cd => cd.OneOrMoreIndividuals == true)
            .Select(cd => cd.Directors).Count();

        var totalNumberOfDirectors = applicationResponses.Directors.Count + corporateDirectors + parenCompanyDirectors;

        if(totalNumberOfDirectors == 0)
        {
            outcome.ErrorMessages.Add("Please add at least one director");
        }

        if (applicationResponses.CorporateDirectorCheck.Response == ApplicationFormConstants.Yes)
        {
            foreach (var coporateGroup in applicationResponses.CorporateDirectorGroups)
            {
                if (coporateGroup.OneOrMoreIndividuals == true)
                {
                    if (coporateGroup.Directors.Count == 0)
                    {
                        outcome.ErrorMessages.Add($"Please add at least one director to the corporate group called: {coporateGroup.Name}");
                    }
                }
            }
        }

        if (applicationResponses.ParentCompanyDirectorCheck.Response == ApplicationFormConstants.Yes)
        {
            foreach (var parentCompanyGroup in applicationResponses.ParentCompanyGroups)
            {
                if (parentCompanyGroup.OneOrMoreIndividuals == true)
                {
                    if (parentCompanyGroup.Directors.Count == 0)
                    {
                        outcome.ErrorMessages.Add($"Please add at least one director to the parent company called: {parentCompanyGroup.Name}");
                    }
                }
            }
        }

        if (applicationResponses.FinalChecks.Any(cd => cd.IsComplete == false))
        {
            outcome.ErrorMessages.Add("Please complete all final checks");
        }

        if (applicationResponses.IsCrsOrHardToReplace)
        {
            if (!applicationResponses.UltimateControllerCheck.IsComplete)
            {
                outcome.ErrorMessages.Add("Please indicate if you have any ultimate controllers");
            }
            else
            {
                if(applicationResponses.UltimateControllerCheck.Response == ApplicationFormConstants.Yes && applicationResponses.UltimateControllers.Count == 0)
                {
                    outcome.ErrorMessages.Add("Please add at least one ultimate controller");
                }
            }
        }

        return outcome;
    }

    public RuleOutcomeDTO EvaluateDate(int? day, int? month, int? year)
    {
        var evaluatedDate = dateEvaluation.EvaluateDate($"{day}/{month}/{year}");

        if (evaluatedDate == null)
        {
            return new RuleOutcomeDTO()
            {
                ErrorMessages = ["Please enter a valid date"]
            };
        }

        return new RuleOutcomeDTO();
    }

    public RuleOutcomeDTO IsDirectorsDateOfBirthValid(DateOnly dateOfBirth)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.Now);

        if (dateOfBirth < dateNow)
        {
            var years = dateNow.Year - dateOfBirth.Year;

            if (years > 115)
            {
                return new RuleOutcomeDTO()
                {
                    ErrorMessages = ["The date of birth you have supplied cannot be over 115 years ago"]
                };
            }

            if(years < 16)
            {
                return new RuleOutcomeDTO()
                {
                    ErrorMessages = ["The date of birth you have supplied must be 16 years or older"]
                };
            }

            return new RuleOutcomeDTO();
        }
        else
        {
            return new RuleOutcomeDTO()
            {
                ErrorMessages = ["The date you have supplied must be in the past"]
            };
        }
    }

    public RuleOutcomeDTO IsLastFinancialYearEndDateValid(DateOnly date)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.Now);

        if (date < dateNow)
        {
            return new RuleOutcomeDTO();
        }
        else
        {
            return new RuleOutcomeDTO()
            {
                ErrorMessages = ["The date you have supplied must be in the past"]
            };
        }
    }

    public RuleOutcomeDTO IsNextFinancialYearEndDateValid(DateOnly date)
    {
        var dateNow = DateOnly.FromDateTime(DateTime.Now);

        if (date > dateNow)
        {
            return new RuleOutcomeDTO();
        }
        else
        {
            return new RuleOutcomeDTO()
            {
                ErrorMessages = ["The date you have supplied must be in the future"]
            };
        }
    }
}
