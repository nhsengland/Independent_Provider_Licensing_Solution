namespace Domain.Logic.Forms.Factories;
public interface IReferenceIDFactory
{
    string CreateForPreApplication(DateTime dateTime, int Id);

    string CreateForApplication(DateTime dateTime, int Id);
}
