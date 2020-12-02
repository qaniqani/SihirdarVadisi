using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace Sihirdar.DataAccessLayer.Infrastructure.Mappers
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