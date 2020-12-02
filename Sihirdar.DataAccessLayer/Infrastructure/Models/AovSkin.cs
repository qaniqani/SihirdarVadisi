namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class AovSkin
    {
        public int Id { get; set; }
        public int Num { get; set; } = 0;
        public string Name { get; set; }
        public string Picture { get; set; }
        public StatusTypes Status { get; set; }
    }
}
