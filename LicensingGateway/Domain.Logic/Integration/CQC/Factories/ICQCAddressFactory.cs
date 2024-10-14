namespace Domain.Logic.Integration.CQC.Factories;
public interface ICQCAddressFactory
{
    string Create(
        string? addressLine1,
        string? addressLine2,
        string? townCity,
        string? region,
        string? postalCode);
}
