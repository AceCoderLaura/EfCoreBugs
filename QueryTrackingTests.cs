using EfCoreBugs.Context;
using EfCoreBugs.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EfCoreBugs;

public class QueryTrackingTests : IDisposable
{
    private readonly PrimaryContext _context;
    private readonly string _dbname = $"{Guid.NewGuid():N}.db";
    private readonly Guid _recordKey = Guid.NewGuid();

    public QueryTrackingTests()
    {
        var options = new DbContextOptionsBuilder<PrimaryContext>()
            .UseSqlite($"Filename={_dbname}")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
            .Options;
        _context = new PrimaryContext(options);

        try
        {
            Seed();
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    private void Seed()
    {
        _context.Database.EnsureCreated();
        _context.TestItems.Add(new TestItem { Key = _recordKey, TextOne = "TEXT ONE", TextTwo = "TEXT TWO" });
        _context.SaveChanges();
    }

    [Fact]
    public void FindReturnsAsNoTracking()
    {
        var record = _context.TestItems.Find(_recordKey);
        var reloaded = _context.TestItems.Find(_recordKey);
        Assert.NotSame(record, reloaded);
    }
   
    [Fact]
    public void SingleReturnsAsNoTracking()
    {
        var record = _context.TestItems.Single(x => x.Key == _recordKey);
        var reloaded = _context.TestItems.Single(x => x.Key == _recordKey);
        Assert.NotSame(record, reloaded);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        SqliteConnection.ClearPool((SqliteConnection)_context.Database.GetDbConnection());
        _context.Dispose();
        File.Delete(_dbname);
    }
}