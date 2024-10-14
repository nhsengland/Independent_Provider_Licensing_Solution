using Microsoft.AspNetCore.Http;

namespace Domain.Logic.Forms.Helpers.Session;

public class SessionOrchestration(
    IHttpContextAccessor httpContextAccessor) : ISessionOrchestration
{
    private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;

    public bool Any()
    {
        return httpContextAccessor.HttpContext!.Session.Keys.Any();
    }

    public T? Get<T>(string key)
    {
        return httpContextAccessor.HttpContext!.Session.Get<T>(key);
    }

    public void Remove(string key)
    {
        httpContextAccessor.HttpContext!.Session.Remove(key);
    }

    public void Set<T>(string key, T value)
    {
        httpContextAccessor.HttpContext!.Session.Set<T>(key, value);
    }
}
