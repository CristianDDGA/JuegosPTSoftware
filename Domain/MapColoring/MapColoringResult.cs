using System.Collections.Generic;

namespace Domain.MapColoring;

public sealed class MapColoringResult
{
    public bool Success { get; init; }
    public List<ColorOption> Assignments { get; init; } = new();
}
