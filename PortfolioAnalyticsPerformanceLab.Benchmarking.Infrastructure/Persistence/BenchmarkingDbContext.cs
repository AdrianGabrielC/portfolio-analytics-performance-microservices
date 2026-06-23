using Microsoft.EntityFrameworkCore;
using PortfolioAnalyticsPerformanceLab.Benchmarking.Domain.Entities;

namespace PortfolioAnalyticsPerformanceLab.Benchmarking.Infrastructure.Persistence;

public sealed class BenchmarkingDbContext : DbContext
{
    public BenchmarkingDbContext(DbContextOptions<BenchmarkingDbContext> options) : base(options)
    {
    }

    public DbSet<BenchmarkRun> BenchmarkRuns { get; set; }
    public DbSet<BenchmarkRunEvent> BenchmarkRunEvents { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BenchmarkingDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BenchmarkRun>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.Events)
                .WithOne(x => x.BenchmarkRun)
                .HasForeignKey(x => x.BenchmarkRunId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BenchmarkRunEvent>(entity =>
        {
            entity.HasKey(x => x.Id);
            
            entity.Property(x => x.EventType)
                .HasMaxLength(100)
                .IsRequired();
            
            entity.Property(x => x.PayloadJson)
                .IsRequired();
            
            entity.Property(x => x.CreatedAtUtc)
                .IsRequired();

            entity.HasIndex(x => x.BenchmarkRunId);
            entity.HasIndex(x => x.CreatedAtUtc);
        });
        
        
        
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Type)
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(x => x.PayloadJson)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasConversion<int>();

            entity.HasIndex(x => new { x.Status, x.OccurredAtUtc });
        });
    }
}