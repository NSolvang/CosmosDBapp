using CosmosDBapp.Model;
using Microsoft.Azure.Cosmos;

namespace CosmosDBapp.Service
{
    public class SupportService
    {
        private readonly Container _container;

        public SupportService(CosmosClient cosmosClient, string databaseName, string containerName)
        {
            if (string.IsNullOrEmpty(databaseName)) throw new ArgumentNullException(nameof(databaseName));
            if (string.IsNullOrEmpty(containerName)) throw new ArgumentNullException(nameof(containerName));

            _container = cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task AddSupportMessageAsync(SupportMessage supportMessage)
        {
            // Opret et anonymous object der matcher database strukturen PRÆCIS
            var cosmosDoc = new
            {
                id = string.IsNullOrEmpty(supportMessage.Id) ? Guid.NewGuid().ToString() : supportMessage.Id,
                contact = new
                {
                    name = supportMessage.Contact?.Name ?? "",
                    email = supportMessage.Contact?.Email ?? "",
                    phone = supportMessage.Contact?.Phone ?? ""
                },
                description = supportMessage.Description ?? "",
                category = string.IsNullOrWhiteSpace(supportMessage.Category) ? "Ukendt" : supportMessage.Category,
                dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ") // Præcis som i database
            };

            Console.WriteLine("=== SENDING TO COSMOS ===");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(cosmosDoc, new System.Text.Json.JsonSerializerOptions { WriteIndented = true }));
            Console.WriteLine("========================");
    
            await _container.UpsertItemAsync(cosmosDoc, new PartitionKey(cosmosDoc.category));
        }
        
        
        public async Task<SupportMessage?> GetSupportMessageAsync(string id)
        {
            try
            {
                var response = await _container.ReadItemAsync<SupportMessage>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<SupportMessage>> GetAllMessagesAsync()
        {
            var query = _container.GetItemQueryIterator<SupportMessage>("SELECT * FROM c");
            var results = new List<SupportMessage>();

            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response.Resource);
            }

            return results;
        }
    }
}