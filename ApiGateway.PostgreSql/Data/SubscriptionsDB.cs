using Microsoft.EntityFrameworkCore;
using ApiGateway.PostgreSql.Models;
namespace ApiGateway.PostgreSql.Data
{
    public class SubscriptionsDB : DbContext
    {
        public SubscriptionsDB(DbContextOptions<SubscriptionsDB> options) : base(options) 
        {
            
        }

        public DbSet<Subscriptions>Subscriptions => Set<Subscriptions>();

    }
}
