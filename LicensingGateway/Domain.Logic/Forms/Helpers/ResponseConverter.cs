using Domain.Logic.Forms.Application;

namespace Domain.Logic.Forms.Helpers;
public class ResponseConverter : IResponseConverter
{
    public bool? Convert(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (value == ApplicationFormConstants.No)
        {
            return false;
        }

        if (value == ApplicationFormConstants.Yes)
        {
            return true;
        }

        throw new InvalidOperationException("Unable to convert value to boolean");
    }

    public string ConvertToYesOrNo(bool value)
    {
        if(value == true)
        {
            return ApplicationFormConstants.Yes;
        }

        return ApplicationFormConstants.No;
    }

    public string ConvertToYesOrNo(bool? value)
    {
        if (value == null)
        {
            /* the question hasn't been asked/saved yet */
            return string.Empty;
        }

        return value.Value == true ? ApplicationFormConstants.Yes : ApplicationFormConstants.No;
    }
}
