using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using AdminProject.Infrastructure.Mappers;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Infrastructure
{
    public class AdminDbContext : DbContext
    {
        public AdminDbContext() : base("name=AdminDbContext")
        {
            
        }
        public AdminDbContext(string connectionString = "AdminDbContext")
            : base(connectionString)
        {
            Database.SetInitializer<AdminDbContext>(null);
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<GalleryDetail> GalleryDetails { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureSize> PictureSizes { get; set; }
        public DbSet<PictureSizeDetail> PictureSizeDetails { get; set; }
        public DbSet<PromiseDay> PromiseDays { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EsportCalendar> EsportCalendars { get; set; }
        public DbSet<UserVoteAssgn> UserVoteAssgns { get; set; }
        public DbSet<LiveBroadcast> LiveBroadcasts { get; set; }
        public DbSet<Counter> Counters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations
                .Add(new UserMapper())
                .Add(new FileMapper())
                .Add(new AdminMapper())
                .Add(new AdvertMapper())
                .Add(new AnswerMapper())
                .Add(new SurveyMapper())
                .Add(new PictureMapper())
                .Add(new ContentMapper())
                .Add(new CounterMapper())
                .Add(new GalleryMapper())
                .Add(new CategoryMapper())
                .Add(new LanguageMapper())
                .Add(new SettingsMapper())
                .Add(new PromiseDayMapper())
                .Add(new PictureSizeMapper())
                .Add(new GalleryDetailMapper())
                .Add(new UserVoteAssgnMapper())
                .Add(new LiveBroadcastMapper())
                .Add(new EsportCalendarMapper())
                .Add(new PictureSizeDetailMapper())
                .Add(new SliderMapper());
        }
    }
}