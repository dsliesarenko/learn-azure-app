using Azure;
using Azure.Data.Tables;

namespace Mvc.StorageAccount.Demo.Data
{
    public class AttendeeEntity : ITableEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Industry { get; set; } = null!;
        public string ImageName { get; set; } = null!;

        public string PartitionKey { get; set; } = null!;
        public string RowKey { get; set; } = null!;
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
