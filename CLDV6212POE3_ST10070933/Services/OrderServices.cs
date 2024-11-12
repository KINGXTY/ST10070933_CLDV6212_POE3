using CLDV6212POE3_ST10070933.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CLDV6212POE3_ST10070933.Services
{
    public class OrderServices
    {
        private readonly IConfiguration _configuration;

        public OrderServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InsertOrderAsync(Orders profile)
        {
            var connectionString = _configuration.GetConnectionString("SqlDatabase");
            var query = @"INSERT INTO Orders (CustomerProfileId, OrderDate, TotalAmount)
                          VALUES (@CustomerProfileId, @OrderDate, @TotalAmount)"; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerProfileId", profile.CustomerProfileId);
                command.Parameters.AddWithValue("@OrderDate", profile.OrderDate);
                command.Parameters.AddWithValue("@TotalAmount", profile.TotalAmount);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
