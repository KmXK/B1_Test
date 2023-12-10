using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Task_02.Persistence;

/// <summary>
/// DbContext design factory required for <b>dotnet tools</b> to create database using code-first approach.
/// </summary>
public class AppDbContextDesignFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer();

        return new AppDbContext(builder.Options);
    }
}