namespace Database.Logic;

public interface IStringLengthRestriction
{
    string Restrict(string input, int restrictTo);
}
