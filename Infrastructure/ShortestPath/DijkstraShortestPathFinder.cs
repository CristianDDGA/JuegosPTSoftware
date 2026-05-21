using Application.ShortestPath;
using Domain.ShortestPath;

namespace Infrastructure.ShortestPath;

public class DijkstraShortestPathFinder : IShortestPathFinder
{
    public RouteResultDto FindShortestPath(City start, City end, IRoadRepository roadRepository)
    {
        var shortestDistanceToCity = new Dictionary<City, double>();
        var previousCityInPath = new Dictionary<City, City>();

        foreach (var road in roadRepository.GetAllRoads())
        {
            shortestDistanceToCity[road.Origin] = double.PositiveInfinity;
            shortestDistanceToCity[road.Destination] = double.PositiveInfinity;
        }
        shortestDistanceToCity[start] = 0;

        var priorityQueue = new PriorityQueue<City, double>();
        var processedCities = new HashSet<City>();
        priorityQueue.Enqueue(start, 0);

        while (priorityQueue.Count > 0)
        {
            var currentCity = priorityQueue.Dequeue();

            if (currentCity.Equals(end))
                break;

            if (!processedCities.Add(currentCity))
                continue;

            foreach (var road in roadRepository.GetRoadsFromCity(currentCity))
            {
                var neighborCity = road.GetOtherCity(currentCity);
                double newDistance = shortestDistanceToCity[currentCity] + road.Distance.Kilometers;

                if (newDistance < shortestDistanceToCity[neighborCity])
                {
                    shortestDistanceToCity[neighborCity] = newDistance;
                    previousCityInPath[neighborCity] = currentCity;
                    priorityQueue.Enqueue(neighborCity, newDistance);
                }
            }
        }

        if (!previousCityInPath.ContainsKey(end) && !start.Equals(end))
        {
            return new RouteResultDto { IsSuccess = false, ErrorMessage = "No route found" };
        }

        var pathCities = new List<City>();
        var current = end;
        while (!current.Equals(start))
        {
            pathCities.Add(current);
            current = previousCityInPath[current];
        }
        pathCities.Add(start);
        pathCities.Reverse();

        return new RouteResultDto
        {
            CityNames = pathCities.Select(city => city.Name).ToList(),
            TotalDistanceKilometers = shortestDistanceToCity[end],
            IsSuccess = true
        };
    }
}
