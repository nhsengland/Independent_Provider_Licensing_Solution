namespace Domain.Logic.Integration.CQC.API;
public class CQCApiConfiguration
{
    public string BaseUrl { get; set; } = default!;
    public int PageSize { get; set; }
    public int AmendedProvidersNumberOfDaysInThePast { get; set; }

    public string OcpApimSubscriptionKey { get; set; } = default!;
}
