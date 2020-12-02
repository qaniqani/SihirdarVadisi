using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace Sihirdar.DataAccessLayer.Infrastructure.Mappers
{
    public class CounterMapper : EntityTypeConfiguration<Counter>
    {
        public CounterMapper()
        {
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}