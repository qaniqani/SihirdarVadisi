namespace AdminProject.Models
{
    public class ContactModelDto
    {
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public SubjectTypes Subject { get; set; }
        public string Message { get; set; }
    }

    public enum SubjectTypes
    {
        NotSelected = 0,
        Opinions = 1,
        Errors = 2,
        Adverts = 3,
        Other = 4
    }
}