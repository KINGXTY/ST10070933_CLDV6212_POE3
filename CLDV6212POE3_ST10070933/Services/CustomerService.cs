using CLDV6212POE3_ST10070933.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CLDV6212POE3_ST10070933.Services
{
    public class CustomerService
    {
        private readonly IConfiguration _configuration;

        public CustomerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InsertCustomerAsync(CustomerProfile profile)
        {
            var connectionString = _configuration.GetConnectionString("SqlDatabase");
            var query = @"INSERT INTO CustomerProfiles (FirstName, LastName, Email, PhoneNumber)
                          VALUES (@FirstName, @LastName, @Email, @PhoneNumber)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", profile.FirstName);
                command.Parameters.AddWithValue("@LastName", profile.LastName);
                command.Parameters.AddWithValue("@Email", profile.Email);
                command.Parameters.AddWithValue("@PhoneNumber", profile.PhoneNumber);

                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
