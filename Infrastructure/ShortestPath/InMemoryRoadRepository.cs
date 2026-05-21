using Domain.ShortestPath;

namespace Infrastructure.ShortestPath;

public class InMemoryRoadRepository : IRoadRepository
{
    private readonly List<Road> _roads;

    public InMemoryRoadRepository(List<Road> roads)
    {
        _roads = roads;
    }

    public IReadOnlyList<Road> GetAllRoads() => _roads.AsReadOnly();

    public IReadOnlyList<Road> GetRoadsFromCity(City city)
    {
        return _roads.Where(road => road.Origin.Equals(city) || road.Destination.Equals(city)).ToList();
    }
}
