using Sihirdar.DataAccessLayer.Infrastructure.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Sihirdar.DataAccessLayer.Infrastructure.Mappers
{
    public class ArenaValorChampMapper : EntityTypeConfiguration<ArenaValorChamp>
    {
        public ArenaValorChampMapper()
        {
            Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}