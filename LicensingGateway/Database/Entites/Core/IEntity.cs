namespace Database.Entites.Core;
public interface IEntity<T> : IIdentifiable<T>
        where T : struct
{
}
