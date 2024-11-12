using CLDV6212POE3_ST10070933.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CLDV6212POE3_ST10070933.Services
{
    public class ProductService
    {
        private readonly IConfiguration _configuration;

        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InsertProductAsync(Product product)
        {
            var connectionString = _configuration.GetConnectionString("SqlDatabase");
            var query = @"INSERT INTO Products (Name, Description, Price, Category, StockLevel, CreatedDate)
                      VALUES (@Name, @Description, @Price, @Category, @StockLevel, @CreatedDate)";

            using (var connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@Category", product.Category);
                command.Parameters.AddWithValue("@StockLevel", product.StockLevel);              
                command.Parameters.AddWithValue("@CreatedDate", product.CreatedDate); 

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
    
