using Dapper;
using Discount.gRPC.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Threading.Tasks;

namespace Discount.gRPC.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                        ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });
            if (coupon == null)
            {
                return new Coupon() { ProductName = "No Discount", Amount = 0, Description = "No Discount Description" };
            }

            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName,Description,Amount) VALUES (@ProductName,@Description,@Amount)", new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                });

            return affected > 0;
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName=@ProductName, DESCRIPTION = @Description, Amount = @Amount WHERE ID = @Id",
                new
                {
                    Id = coupon.Id,
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                }
                );

            return affected > 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new
                {
                    ProductName = productName
                }
                );

            return affected > 0;
        }

    }
}
