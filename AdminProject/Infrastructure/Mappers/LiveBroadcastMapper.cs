using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Infrastructure.Mappers
{
    public class LiveBroadcastMapper : EntityTypeConfiguration<LiveBroadcast>
    {
        public LiveBroadcastMapper()
        {
            Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}