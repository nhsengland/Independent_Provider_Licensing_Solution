namespace Domain.Logic.Forms.Helpers;
public interface IResponseConverter
{
    bool? Convert(string value);

    string ConvertToYesOrNo(bool value);

    string ConvertToYesOrNo(bool? value);
}
