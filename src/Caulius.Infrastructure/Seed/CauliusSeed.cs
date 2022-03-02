using System.Text.Json;
using Caulius.Domain.Aggregates.CommandAggregate;
using Caulius.Domain.Common;
using Microsoft.Extensions.Hosting;

namespace Caulius.Infrastructure.Seed;

public class CauliusSeed
{
    private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
    private readonly CauliusContext _context;
    private readonly string _seedFolderPath;

    public CauliusSeed(CauliusContext context, IHostEnvironment hostEnvironment)
    {
        _context = context;
        _seedFolderPath = Path.Combine(hostEnvironment.ContentRootPath, "Seed");
    }

    public void Seed()
    {
        _context.EnsureDatabaseCreated();

        if (!_context.TextCommands.AsEnumerable().Any() && TextCommands.Any())
            _context.TextCommands.AddRange(TextCommands);

        _context.SaveChanges();
    }

    private IEnumerable<TextCommand> TextCommands => GetItems<TextCommand>("TextCommands.json");

    private IEnumerable<T> GetItems<T>(string fileName) where T : Entity
    {
        try
        {
            var content = File.ReadAllText(Path.Combine(_seedFolderPath, fileName));
            return JsonSerializer.Deserialize<IEnumerable<T>>(content, _serializerOptions) ?? Array.Empty<T>();
        }
        catch (Exception)
        {
            return Array.Empty<T>();
        }
    }
}