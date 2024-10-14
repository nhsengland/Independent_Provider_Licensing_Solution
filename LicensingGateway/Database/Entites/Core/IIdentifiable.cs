namespace Database.Entites.Core;

public interface IIdentifiable<T>
        where T : struct
{
    public T Id { get; set; }
}
