namespace Database.Repository.Helpers;
public interface IBoolConverter
{
    bool ConvertThrowIfNull(bool? input, int id);
}
