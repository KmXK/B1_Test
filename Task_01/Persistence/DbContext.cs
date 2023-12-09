using Microsoft.EntityFrameworkCore;
using Task_01.Persistence.Entities;

namespace Task_01.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<FileData> FilesData { get; set; } = null!;
}