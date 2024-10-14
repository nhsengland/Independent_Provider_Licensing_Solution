using System.Text;

namespace Domain.Logic.Integrations.StorageAccount.Queues.Factories;
public class QueueMessageEncoder : IQueueMessageEncoder
{
    public string ToBase64String(string message)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(message));
    }
}
