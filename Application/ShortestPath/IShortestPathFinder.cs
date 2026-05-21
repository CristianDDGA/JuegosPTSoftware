using Domain.ShortestPath;

namespace Application.ShortestPath;

public interface IShortestPathFinder
{
    RouteResultDto FindShortestPath(City start, City end, IRoadRepository roadRepository);
}
