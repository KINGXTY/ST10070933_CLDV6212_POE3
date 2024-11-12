using Microsoft.AspNetCore.Mvc;
using Azure.Data.Tables;
using System.Threading.Tasks;

namespace CLDV6212POE3_ST10070933.Services
{
    public class TableService
    {
        private readonly TableServiceClient _tableServiceClient;

        public TableService(TableServiceClient tableServiceClient)
        {
            _tableServiceClient = tableServiceClient ?? throw new ArgumentNullException(nameof(tableServiceClient));
        }

        // Method to ensure the table exists //corrected by chatgpt
        private async Task CreateTableIfNotExistsAsync(string tableName)
        {
            try
            {
                var tableClient = _tableServiceClient.GetTableClient(tableName);
                // to create the table if it doesn't exist
                await tableClient.CreateIfNotExistsAsync();
            }
            catch (Exception ex)
            {     // Log or handle the exception as needed                       
                throw; // Rethrow or handle accordingly
            }
        }

        public async Task AddEntityAsync<T>(string tableName, T entity) where T : class, ITableEntity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Ensure the table exists before adding an entity
            await CreateTableIfNotExistsAsync(tableName);

            try
            {
                var tableClient = _tableServiceClient.GetTableClient(tableName);
                await tableClient.AddEntityAsync(entity);
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed              
                throw; // Rethrow or handle accordingly
            }
        }
    }
}