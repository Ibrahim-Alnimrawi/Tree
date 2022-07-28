using Microsoft.EntityFrameworkCore;

namespace Tree
{
    public class ApplicationDbContext : DbContext
    {
        public IServiceProvider ServiceProvider { get; set; }
        public ApplicationDbContext(IServiceProvider serviceProvider, DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {
            ServiceProvider = serviceProvider;
            base.ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NodeEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Data);
                entity.HasOne(x => x.Parent)
                    .WithMany(x => x.Children)
                    .HasForeignKey(x => x.ParentId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            // ...
        }
        public DbSet<NodeEntity> Tree { get; set; }
    }
}
