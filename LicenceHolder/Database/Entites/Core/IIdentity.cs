namespace Database.Entites.Core
{
    public interface IIdentity<T>
        where T : struct
    {
        public T Id { get; set; }
    }
}
