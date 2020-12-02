using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Infrastructure.Mappers
{
    public class UserVoteAssgnMapper : EntityTypeConfiguration<UserVoteAssgn>
    {
        public UserVoteAssgnMapper()
        {
            Property(a => a.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}