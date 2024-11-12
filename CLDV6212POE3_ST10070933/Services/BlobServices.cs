using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure;


namespace CLDV6212POE3_ST10070933.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;       
        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureBlobStorage");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Azure Storage connection string is not configured properly.");
            }
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        // Method to upload a blob to the specified container //corrected by chatgpt
        public async Task UploadBlobAsync(string containerName, string blobName, Stream content)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentException("Container name cannot be null or empty.", nameof(containerName));
            }

            if (string.IsNullOrEmpty(blobName))
            {
                throw new ArgumentException("Blob name cannot be null or empty.", nameof(blobName));
            }

            if (content == null || content.Length == 0)
            {
                throw new ArgumentException("Content stream cannot be null or empty.", nameof(content));
            }

            try
            {
                // Get a reference to the container client  //corrected by chatgpt
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it doesn't exist
                await containerClient.CreateIfNotExistsAsync();

                // Get a reference to the blob client
                var blobClient = containerClient.GetBlobClient(blobName);

                // Upload the content to the blob, overwriting if it exists
                await blobClient.UploadAsync(content, overwrite: true);
            }
            catch (RequestFailedException ex)
            {
                // Handle exceptions related to Azure Storage requests
                Console.WriteLine($"An error occurred while uploading the blob: {ex.Message}");
                // Log or handle the exception as necessary
            }
            catch (Exception ex)
            {
                // Handle general exceptions
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                // Log or handle the exception as necessary
            }
        }
    }
}