using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using Azure;


namespace CLDV6212POE3_ST10070933.Services
{
    public class QueueService
    {
        private readonly QueueServiceClient _queueServiceClient;      
        public QueueService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureQueueStorage");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Azure Storage connection string is not configured properly.");
            }
            _queueServiceClient = new QueueServiceClient(connectionString);
        }

        // Method to send a message to the specified queue //corrected by chatgpt
        public async Task SendMessageAsync(string queueName, string message)
        {
            if (string.IsNullOrEmpty(queueName))
            {
                throw new ArgumentException("Queue name cannot be null or empty.", nameof(queueName));
            }

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty.", nameof(message));
            }

            try
            {
                // Get a reference to the queue client
                var queueClient = _queueServiceClient.GetQueueClient(queueName);

                // Create the queue if it doesn't exist
                await queueClient.CreateIfNotExistsAsync();

                // Send the message to the queue
                await queueClient.SendMessageAsync(message);
            }
            catch (RequestFailedException ex)
            {             
                Console.WriteLine($"An error occurred while sending the message: {ex.Message}");
                // Log or handle the exception as necessary
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                // Log or handle the exception as necessary
            }
        }
    }
}