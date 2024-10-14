namespace Domain.Logic.Integration.CQC.API.Models;
public record ProvidersOutputModel
{
    public int total { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string firstPageUri { get; set; }
    public int page { get; set; }
    public object previousPageUri { get; set; }
    public string lastPageUri { get; set; }
    public string nextPageUri { get; set; }
    public int perPage { get; set; }
    public int totalPages { get; set; }
    public Provider[] providers { get; set; }
    public string uri { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public record Provider
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string providerId { get; set; }
        public string providerName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
