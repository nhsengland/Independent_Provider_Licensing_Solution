namespace Domain.Logic.Forms.Helpers.Session;
public interface ISessionOrchestration
{
    public bool Any();

    public T? Get<T>(string key);

    public void Remove(string key);

    public void Set<T>(string key, T value);
}
