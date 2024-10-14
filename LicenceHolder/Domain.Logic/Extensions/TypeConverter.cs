namespace Domain.Logic.Extensions;

public class TypeConverter : ITypeConverter
{
    public bool Convert(string? input)
    {
        bool.TryParse(input, out bool result);
        
        return result;
    }
}
