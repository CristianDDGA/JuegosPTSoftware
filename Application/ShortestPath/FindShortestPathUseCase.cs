using Domain.ShortestPath;

namespace Application.ShortestPath;

public class FindShortestPathUseCase
{
    private readonly IShortestPathFinder _pathFinder;
    private readonly IRoadRepository _roadRepository;

    public FindShortestPathUseCase(IShortestPathFinder pathFinder, IRoadRepository roadRepository)
    {
        _pathFinder = pathFinder;
        _roadRepository = roadRepository;
    }

    public RouteResultDto Execute(string startCityName, string endCityName)
    {
        var allRoads = _roadRepository.GetAllRoads();
        var allCities = allRoads.SelectMany(road => new[] { road.Origin, road.Destination }).Distinct().ToList();
        var start = allCities.FirstOrDefault(city => city.Name == startCityName);
        var end = allCities.FirstOrDefault(city => city.Name == endCityName);

        if (start == null)
            return new RouteResultDto { IsSuccess = false, ErrorMessage = $"Start city '{startCityName}' not found" };
        if (end == null)
            return new RouteResultDto { IsSuccess = false, ErrorMessage = $"End city '{endCityName}' not found" };

        return _pathFinder.FindShortestPath(start, end, _roadRepository);
    }
}
