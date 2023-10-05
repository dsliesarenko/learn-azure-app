using Mvc.StorageAccount.Demo.Data;

namespace Mvc.StorageAccount.Demo.Interfaces
{
    public interface ITableStorageService
    {
        Task DeleteAttendee(string industry, string id);
        Task<AttendeeEntity> GetAttendee(string industry, string id);
        List<AttendeeEntity> GetAttendees();
        Task UpsertAttendee(AttendeeEntity attendeeEntity);
    }
}