using ApiServer.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiServer.Repository
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<ApiDataset> ApiDataset { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiDataset>()
                .HasKey(_ => _.Guid);
        }
    }
}
