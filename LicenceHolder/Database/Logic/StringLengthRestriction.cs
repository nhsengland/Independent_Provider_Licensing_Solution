namespace Database.Logic;

public class StringLengthRestriction : IStringLengthRestriction
{
    public string Restrict(string input, int restrictTo)
    {
        if (input.Length > restrictTo)
        {
            return input.Substring(0, restrictTo);
        }

        return input;
    }
}
