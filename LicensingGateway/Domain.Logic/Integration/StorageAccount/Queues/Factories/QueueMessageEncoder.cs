using System.Text;

namespace Domain.Logic.Integration.StorageAccount.Queues.Factories;
public class QueueMessageEncoder : IQueueMessageEncoder
{
    public string ToBase64String(string message)
    {
        var bytes = Encoding.UTF8.GetBytes(message);
        return Convert.ToBase64String(bytes);
    }
}
