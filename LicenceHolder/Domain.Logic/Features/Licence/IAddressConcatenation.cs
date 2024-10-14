namespace Domain.Logic.Features.Licence;

public interface IAddressConcatenation
{
    string Concat(string line1, string line2, string townOrCity, string county, string postcode);
}
