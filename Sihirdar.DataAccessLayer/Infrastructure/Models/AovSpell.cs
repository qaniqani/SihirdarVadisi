namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class AovSpell
    {
        public int Id { get; set; }
        public int Num { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public StatusTypes Status { get; set; }
    }
}
