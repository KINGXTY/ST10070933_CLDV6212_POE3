using CLDV6212POE3_ST10070933.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using Azure.Data.Tables;

namespace CLDV6212POE3_ST10070933
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure Azure services
            builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlobStorage")));
            builder.Services.AddSingleton(x => new ShareServiceClient(builder.Configuration.GetConnectionString("AzureFileStorage")));
            builder.Services.AddSingleton(x => new QueueServiceClient(builder.Configuration.GetConnectionString("AzureQueueStorage")));
            builder.Services.AddSingleton(x => new TableServiceClient(builder.Configuration.GetConnectionString("AzureTableStorage")));
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddSingleton<BlobService>();
            builder.Services.AddSingleton<FileService>();
            builder.Services.AddSingleton<QueueService>();
            builder.Services.AddSingleton<TableService>();
            builder.Services.AddScoped<OrderServices>();
            builder.Services.AddScoped<ProductService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

