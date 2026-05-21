using Domain.MapColoring;
using System.Collections.Generic;

namespace Application.MapColoring;

public class MapColoringUseCase : IMapColoringUseCase
{
    private MapGraph? _graph;
    private List<ColorOption>? _assignments;
    private int _maxColors;

    public MapColoringResult Solve(MapGraph graph, int maxColors = 4)
    {
        _graph = graph;
        _maxColors = maxColors;
        _assignments = new List<ColorOption>(new ColorOption[graph.Count]);

        if (Backtrack(0))
        {
            return new MapColoringResult
            {
                Success = true,
                Assignments = new List<ColorOption>(_assignments)
            };
        }

        return new MapColoringResult
        {
            Success = false,
            Assignments = new List<ColorOption>()
        };
    }

    private bool Backtrack(int regionIndex)
    {
        if (_graph is null || _assignments is null) return false;

        if (regionIndex >= _graph.Count) return true;

        for (int color = 1; color <= _maxColors; color++)
        {
            var candidate = (ColorOption)color;
            if (CanAssign(regionIndex, candidate))
            {
                _assignments[regionIndex] = candidate;

                if (Backtrack(regionIndex + 1)) return true;

                _assignments[regionIndex] = ColorOption.None;
            }
        }

        return false;
    }

    private bool CanAssign(int regionIndex, ColorOption candidate)
    {
        if (_graph is null || _assignments is null) return false;

        if (!_graph.Adjacency.TryGetValue(regionIndex, out var neighbors)) return true;

        foreach (var n in neighbors)
        {
            if (_assignments[n] == candidate) return false;
        }

        return true;
    }
}
