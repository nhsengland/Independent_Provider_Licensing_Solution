namespace Database.Entites.Core;

public interface IBaseIdentity<T> : IIdentity<T>
        where T : struct
{
}
