using System.Data.Entity;

namespace Sihirdar.DataAccessLayer.Infrastructure
{
    public class AdminDbContextInitializer : DropCreateDatabaseAlways<AdminDbContext>
    {
        protected override void Seed(AdminDbContext context)
        {

        }
    }
}