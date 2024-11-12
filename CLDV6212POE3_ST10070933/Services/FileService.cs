using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Files.Shares;
using Azure;


namespace CLDV6212POE3_ST10070933.Services
{
    public class FileService
    {
        private readonly ShareServiceClient _shareServiceClient;

        // Constructor using IConfiguration to initialize ShareServiceClient
        public FileService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureFileStorage");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Azure Storage connection string is not configured properly.");
            }
            _shareServiceClient = new ShareServiceClient(connectionString);
        }

        // Method to upload a file to the specified share and directory //corrected by chatgpt
        public async Task UploadFileAsync(string shareName, string directoryName, string fileName, Stream content)
        {
            if (string.IsNullOrEmpty(shareName))
            {
                throw new ArgumentException("Share name cannot be null or empty.", nameof(shareName));
            }

            if (string.IsNullOrEmpty(directoryName))
            {
                throw new ArgumentException("Directory name cannot be null or empty.", nameof(directoryName));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
            }

            if (content == null || content.Length == 0)
            {
                throw new ArgumentException("Content stream cannot be null or empty.", nameof(content));
            }

            try
            {
                // Get a reference to the share client
                var shareClient = _shareServiceClient.GetShareClient(shareName);

                // Create the share if it doesn't exist
                await shareClient.CreateIfNotExistsAsync();

                // Get the directory client for the specified directory
                var directoryClient = shareClient.GetDirectoryClient(directoryName);
                await directoryClient.CreateIfNotExistsAsync();

                // Get the file client for the specified file
                var fileClient = directoryClient.GetFileClient(fileName);

                // Create the file with the specified length
                await fileClient.CreateAsync(content.Length);

                // Upload the content to the file
                await fileClient.UploadAsync(content);
            }
            catch (RequestFailedException ex)
            {               
                Console.WriteLine($"An error occurred while uploading the file: {ex.Message}");              
            }
            catch (Exception ex)
            {                
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");                
            }
        }
    }
}
