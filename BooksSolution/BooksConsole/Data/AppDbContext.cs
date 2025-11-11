using System.IO;
using BooksConsole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BooksConsole.Data;

public class AppDbContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<TitleTag> TitlesTags => Set<TitleTag>();

    private static readonly ILoggerFactory _loggerFactory =
        LoggerFactory.Create(builder => builder.AddConsole());

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // data/ en la raíz del proyecto
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var dataDir = Path.Combine(projectRoot, "data");
        Directory.CreateDirectory(dataDir);

        var dbPath = Path.Combine(dataDir, "books.db");
        optionsBuilder
            //.UseLoggerFactory(_loggerFactory) // opcional para logging SQL
            .UseSqlite($"Data Source={dbPath};");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tabla de cruce renombrada y relaciones
        modelBuilder.Entity<TitleTag>(e =>
        {
            e.ToTable("TitlesTags");           // nombre en BD
            e.HasKey(tt => tt.TitleTagId);

            e.HasOne(tt => tt.Title)
             .WithMany(t => t.TitleTags)
             .HasForeignKey(tt => tt.TitleId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(tt => tt.Tag)
             .WithMany(tg => tg.TitleTags)
             .HasForeignKey(tt => tt.TagId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(tt => new { tt.TitleId, tt.TagId }).IsUnique();
        });

        // Orden de columnas en Title: TitleId, AuthorId, TitleName
        modelBuilder.Entity<Title>(e =>
        {
            e.Property(p => p.TitleId).HasColumnOrder(0);
            e.Property(p => p.AuthorId).HasColumnOrder(1);
            e.Property(p => p.TitleName).HasColumnOrder(2);
        });

        // NOT NULL en strings
        modelBuilder.Entity<Author>().Property(a => a.AuthorName).IsRequired();
        modelBuilder.Entity<Title>().Property(t => t.TitleName).IsRequired();
        modelBuilder.Entity<Tag>().Property(t => t.TagName).IsRequired();

        base.OnModelCreating(modelBuilder);
    }
}
