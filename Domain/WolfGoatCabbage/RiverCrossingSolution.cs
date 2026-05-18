namespace Domain.WolfGoatCabbage;

public sealed class RiverCrossingSolution
{
    public bool IsSolved { get; init; }
    public List<RiverCrossingState> States { get; init; } = [];
    public List<string> Moves { get; init; } = [];

    public int MoveCount => States.Count > 0 ? States.Count - 1 : 0;
}
