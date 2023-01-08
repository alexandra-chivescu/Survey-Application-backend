using Survey_App_be.Models;
using Microsoft.EntityFrameworkCore;

namespace Survey_App.ContextModels
{
    public class SurveyContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public SurveyContext(DbContextOptions<SurveyContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=SurveyApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
        public DbSet<Surveys> Surveys { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<CompletedSurveys> CompletedSurveys { get; set; }
    }
}
