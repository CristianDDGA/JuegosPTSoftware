namespace Domain.MissionariesCannibals;

public sealed class MissionariesSolution
{
    public bool IsSolved { get; init; }
    public List<MissionariesState> States { get; init; } = [];
    public List<string> Moves { get; init; } = [];

    public int MoveCount => States.Count > 0 ? States.Count - 1 : 0;
}
