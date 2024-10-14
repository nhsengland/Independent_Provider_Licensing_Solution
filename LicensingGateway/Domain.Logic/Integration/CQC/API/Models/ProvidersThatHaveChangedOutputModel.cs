namespace Domain.Logic.Integration.CQC.API.Models;
public class ProvidersThatHaveChangedOutputModel
{
    public int total { get; set; }
    public int page { get; set; }
    public int totalPages { get; set; }
    public int perPage { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string nextPageUri { get; set; }
    public string firstPageUri { get; set; }
    public string lastPageUri { get; set; }
    public string[] changes { get; set; }
    public string uri { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}
