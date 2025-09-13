using AdvertisingPlatforms.Models;

namespace AdvertisingPlatforms.Services;

public class PlatformService
{
    private readonly List<Platform> _platforms = [];
    
    public async Task UploadFile(Stream fileStream)
    {
        if(fileStream.Length == 0)
            throw new ArgumentNullException(nameof(fileStream));

        _platforms.Clear();

        using var reader = new StreamReader(fileStream);
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            if(string.IsNullOrEmpty(line))
                continue;
            
            var parts = line.Split(':', 2);
            if(parts.Length < 2)
                continue;

            var name = parts[0].Trim();
            var regions = parts[1]
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            _platforms.Add(new Platform(name, regions));
        }
    }

    public List<string> GetPlatformsByRegion(string location)
    {
        var found = (from p in _platforms where p.Regions.Any(r => 
            location.StartsWith(r, StringComparison.OrdinalIgnoreCase)) select p.Name).ToList();
        
        return found;
    }
}