namespace Domain.ShortestPath;

public class City
{
    public string Name { get; }
    public double? Latitude { get; }
    public double? Longitude { get; }

    public City(string name, double? latitude = null, double? longitude = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Latitude = latitude;
        Longitude = longitude;
    }

    public override bool Equals(object? obj) => obj is City other && Name == other.Name;

    public override int GetHashCode() => Name.GetHashCode();
}
