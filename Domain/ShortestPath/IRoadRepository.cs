namespace Domain.ShortestPath;

public interface IRoadRepository
{
    IReadOnlyList<Road> GetAllRoads();
    IReadOnlyList<Road> GetRoadsFromCity(City city);
}
