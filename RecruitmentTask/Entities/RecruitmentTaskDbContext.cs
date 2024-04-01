using Microsoft.EntityFrameworkCore;

namespace RecruitmentTask.Entities
{
    public class RecruitmentTaskDbContext : DbContext
    {
        public RecruitmentTaskDbContext(DbContextOptions<RecruitmentTaskDbContext> options) : base(options) 
        {
            
        }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
