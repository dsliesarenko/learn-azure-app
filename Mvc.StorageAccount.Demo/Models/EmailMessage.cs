namespace Mvc.StorageAccount.Demo.Models
{
    public class EmailMessage
    {
        public string EmailAddress { get; set; } = null!;

        public DateTime TimeStamp { get; set; }

        public string Message { get; set; } = null!;
    }
}
