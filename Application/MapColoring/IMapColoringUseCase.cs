using Domain.MapColoring;

namespace Application.MapColoring;

public interface IMapColoringUseCase
{
    MapColoringResult Solve(MapGraph graph, int maxColors = 4);
}
