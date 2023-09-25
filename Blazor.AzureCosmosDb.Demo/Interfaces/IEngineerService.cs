using Blazor.AzureCosmosDb.Demo.Data;

namespace Blazor.AzureCosmosDb.Demo.Interfaces
{
    public interface IEngineerService
    {
        Task DeleteEngineer(string? id, string? partitionKey);
        Task<List<Engineer>> GetEngineerDetails();
        Task<Engineer> GetEngineerDetailsById(string? id, string? partitionKey);
        Task UpsertEngineer(Engineer engineer);
    }
}
