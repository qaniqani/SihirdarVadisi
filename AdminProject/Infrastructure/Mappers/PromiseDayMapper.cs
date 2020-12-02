using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Infrastructure.Mappers
{
    public class PromiseDayMapper : EntityTypeConfiguration<PromiseDay>
    {
        public PromiseDayMapper()
        {
            Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}