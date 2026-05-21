namespace Domain.MissionariesCannibals;

public sealed record MissionariesState(
    int MissionariesLeft,
    int CannibalsLeft,
    bool BoatOnLeft)
{
    public const int TotalMissionaries = 3;
    public const int TotalCannibals = 3;

    public static MissionariesState Initial => new(TotalMissionaries, TotalCannibals, true);

    public int MissionariesRight => TotalMissionaries - MissionariesLeft;

    public int CannibalsRight => TotalCannibals - CannibalsLeft;

    public bool IsGoal =>
        MissionariesLeft == 0 &&
        CannibalsLeft == 0 &&
        !BoatOnLeft;

    public bool IsSafe =>
        BankIsSafe(MissionariesLeft, CannibalsLeft) &&
        BankIsSafe(MissionariesRight, CannibalsRight);

    public IEnumerable<(MissionariesState NextState, string MoveDescription)> GetNextStates()
    {
        if (BoatOnLeft)
        {
            foreach (var (missionariesMoved, cannibalsMoved) in GetBoatLoads(MissionariesLeft, CannibalsLeft))
            {
                var nextState = new MissionariesState(
                    MissionariesLeft - missionariesMoved,
                    CannibalsLeft - cannibalsMoved,
                    false);

                if (nextState.IsSafe)
                {
                    yield return (nextState, DescribeCrossing(missionariesMoved, cannibalsMoved, toRight: true));
                }
            }
        }
        else
        {
            foreach (var (missionariesMoved, cannibalsMoved) in GetBoatLoads(MissionariesRight, CannibalsRight))
            {
                var nextState = new MissionariesState(
                    MissionariesLeft + missionariesMoved,
                    CannibalsLeft + cannibalsMoved,
                    true);

                if (nextState.IsSafe)
                {
                    yield return (nextState, DescribeCrossing(missionariesMoved, cannibalsMoved, toRight: false));
                }
            }
        }
    }

    public string DescribeBank(bool leftBank)
    {
        var missionaries = leftBank ? MissionariesLeft : MissionariesRight;
        var cannibals = leftBank ? CannibalsLeft : CannibalsRight;
        var boatHere = BoatOnLeft == leftBank;

        var parts = new List<string>
        {
            $"{missionaries} misionero{(missionaries == 1 ? "" : "s")}",
            $"{cannibals} caníbal{(cannibals == 1 ? "" : "es")}"
        };

        if (boatHere)
        {
            parts.Add("barca");
        }

        return string.Join(" • ", parts);
    }

    private static bool BankIsSafe(int missionaries, int cannibals) =>
        missionaries == 0 || cannibals <= missionaries;

    private static IEnumerable<(int Missionaries, int Cannibals)> GetBoatLoads(int missionariesAvailable, int cannibalsAvailable)
    {
        for (int missionariesMoved = 0; missionariesMoved <= 2; missionariesMoved++)
        {
            for (int cannibalsMoved = 0; cannibalsMoved <= 2; cannibalsMoved++)
            {
                int passengers = missionariesMoved + cannibalsMoved;
                if (passengers is < 1 or > 2)
                {
                    continue;
                }

                if (missionariesMoved > missionariesAvailable || cannibalsMoved > cannibalsAvailable)
                {
                    continue;
                }

                yield return (missionariesMoved, cannibalsMoved);
            }
        }
    }

    private static string DescribeCrossing(int missionariesMoved, int cannibalsMoved, bool toRight)
    {
        var direction = toRight ? "derecha" : "izquierda";
        var passengers = DescribePassengers(missionariesMoved, cannibalsMoved);
        return $"La barca cruza hacia la {direction} con {passengers}";
    }

    private static string DescribePassengers(int missionariesMoved, int cannibalsMoved) =>
        (missionariesMoved, cannibalsMoved) switch
        {
            (1, 0) => "1 misionero",
            (2, 0) => "2 misioneros",
            (0, 1) => "1 caníbal",
            (0, 2) => "2 caníbales",
            (1, 1) => "1 misionero y 1 caníbal",
            _ => "pasajeros inválidos"
        };
}
