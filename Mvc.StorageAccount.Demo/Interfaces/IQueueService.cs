using Mvc.StorageAccount.Demo.Models;

namespace Mvc.StorageAccount.Demo.Interfaces
{
    public interface IQueueService
    {
        Task SendMessage(EmailMessage emailMessage);
    }
}