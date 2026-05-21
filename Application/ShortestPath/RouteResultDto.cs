namespace Application.ShortestPath;

public class RouteResultDto
{
    public List<string> CityNames { get; set; } = new();
    public double TotalDistanceKilometers { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
