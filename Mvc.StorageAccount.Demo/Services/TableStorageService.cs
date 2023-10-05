﻿using Azure;
using Azure.Data.Tables;
using Mvc.StorageAccount.Demo.Data;
using Mvc.StorageAccount.Demo.Interfaces;

namespace Mvc.StorageAccount.Demo.Services
{
    public class TableStorageService : ITableStorageService
    {
        private readonly TableClient _tableClient;

        public TableStorageService(TableClient tableClient)
        {
            _tableClient = tableClient;
        }

        public async Task<AttendeeEntity> GetAttendee(string industry, string id)
        {
            return await _tableClient.GetEntityAsync<AttendeeEntity>(industry, id);
        }

        public List<AttendeeEntity> GetAttendees()
        {
            Pageable<AttendeeEntity> attendeeEntities = _tableClient.Query<AttendeeEntity>();
            return attendeeEntities.ToList();
        }

        public async Task UpsertAttendee(AttendeeEntity attendee)
        {
            await _tableClient.UpsertEntityAsync(attendee);
        }

        public async Task DeleteAttendee(string industry, string id)
        {
            await _tableClient.DeleteEntityAsync(industry, id);
        }
    }
}
