namespace AdvertisingPlatforms.Models;

public class Platform(string name, List<string> regions)
{
    public string Name { get; set; } = name;
    public List<string> Regions { get; set; } = regions;
}