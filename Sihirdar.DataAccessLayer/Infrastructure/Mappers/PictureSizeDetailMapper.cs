using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace Sihirdar.DataAccessLayer.Infrastructure.Mappers
{
    public class PictureSizeDetailMapper : EntityTypeConfiguration<PictureSizeDetail>
    {
        public PictureSizeDetailMapper()
        {
            Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}