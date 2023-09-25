using Blazor.AzureCosmosDb.Demo.Data;
using Blazor.AzureCosmosDb.Demo.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Blazor.AzureCosmosDb.Demo.Services
{
    public class EngineerService : IEngineerService
    {
        private readonly string CosmosDbConnectionString = "AccountEndpoint=https://azure-dima-cosmos-db.documents.azure.com:443/;AccountKey=X3QJ6iLSHfKfgFnQs7D2SsUZFbk3RAd69avo5ctsmoaS5NCE7w99rzMJbhvpkvLqP23T9YkXArCXACDbqcbA7g==;";
        private readonly string CosmosDbName = "Contractors";
        private readonly string CosmosDbContainerName = "Engineers";
        private readonly Container _container;

        public EngineerService()
        {
            _container = new CosmosClient(CosmosDbConnectionString).GetContainer(CosmosDbName, CosmosDbContainerName);
        }

        public async Task UpsertEngineer(Engineer engineer)
        {
            if (engineer.id == Guid.Empty)
            {
                engineer.id = Guid.NewGuid();
            }

            var updateRes = await _container.UpsertItemAsync(engineer, new PartitionKey(engineer.id.ToString()));
            Console.Write(updateRes.StatusCode);
        }

        public async Task DeleteEngineer(string? id, string? partitionKey)
        {
            var response = await _container.DeleteItemAsync<Engineer>(id, new PartitionKey(partitionKey));
        }

        public async Task<List<Engineer>> GetEngineerDetails()
        {
            List<Engineer> engineers = new List<Engineer>();
            var sqlQuery = "SELECT * FROM c";
            QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
            FeedIterator<Engineer> queryResultSetIterator = _container.GetItemQueryIterator<Engineer>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<Engineer> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (Engineer engineer in currentResultSet)
                {
                    engineers.Add(engineer);
                }
            }

            return engineers;
        }

        public async Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey)
        {
            ItemResponse<Engineer> response = await _container.ReadItemAsync<Engineer>(id, new PartitionKey(partitionKey));
            return response.Resource;
        }
    }
}
