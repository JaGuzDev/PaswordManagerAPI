using Microsoft.EntityFrameworkCore;
using PasswordManager.Infrastructure.Entity;

namespace PasswordManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public DbContextOptions<AppDbContext> Options => _options;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Apply configurations from the current assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Explicitly configure CreatedBy relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey("CreatedById")
                .OnDelete(DeleteBehavior.Restrict);

            // Explicitly configure UpdatedBy relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.UpdatedBy)
                .WithMany()
                .HasForeignKey("UpdatedById")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<AuthToken>().ToTable("AuthToken");
            modelBuilder.Entity<Entry>().ToTable("Entry");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        public DbSet<Entry> Entries { get; set; }
    }
}
