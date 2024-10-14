namespace Database.Repository.Helpers;
public class BoolConverter : IBoolConverter
{
    public bool ConvertThrowIfNull(bool? input, int id)
    {
        if (input == null)
        {
            throw new InvalidOperationException($"Value is null and this is not expected: {id}");
        }

        return input.Value;
    }
}
