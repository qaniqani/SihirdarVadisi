using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Infrastructure.Mappers
{
    public class CounterMapper : EntityTypeConfiguration<Counter>
    {
        public CounterMapper()
        {
            Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}