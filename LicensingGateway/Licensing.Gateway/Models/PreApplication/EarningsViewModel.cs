using Domain.Logic.Forms.PreApplication;
using Microsoft.AspNetCore.Mvc;

namespace Licensing.Gateway.Models.PreApplication;

public class EarningsViewModel : ApplicationBase
{

    [BindProperty]
    public string Earnings { get; set; } = default!;

    public string[] EarningValues = [PreApplicationFormConstants.Earnings_Answer_1, PreApplicationFormConstants.Earnings_Answer_2, PreApplicationFormConstants.Earnings_Answer_3];

    public string ProviderName { get; set; } = default!;
}
