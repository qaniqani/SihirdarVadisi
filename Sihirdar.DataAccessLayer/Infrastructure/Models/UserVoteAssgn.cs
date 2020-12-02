namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class UserVoteAssgn
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AnswerId { get; set; }
        public int SurveyId { get; set; }
    }
}