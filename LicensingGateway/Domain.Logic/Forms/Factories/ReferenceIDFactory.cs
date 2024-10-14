namespace Domain.Logic.Forms.Factories;
public class ReferenceIDFactory : IReferenceIDFactory
{
    public string CreateForPreApplication(DateTime dateTime, int Id)
    {
        return $"LGPA{dateTime:yyyyMMddHHmmss}.{Id}";
    }

    public string CreateForApplication(DateTime dateTime, int Id)
    {
        return $"HDJ{dateTime:yyyyMMddHHmmss}F{Id}";
    }
}
