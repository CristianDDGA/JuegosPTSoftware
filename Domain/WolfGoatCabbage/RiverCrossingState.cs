namespace Domain.WolfGoatCabbage;

public sealed record RiverCrossingState(
    RiverSide FarmerSide,
    RiverSide WolfSide,
    RiverSide GoatSide,
    RiverSide CabbageSide)
{
    public static RiverCrossingState Initial => new(
        RiverSide.Left,
        RiverSide.Left,
        RiverSide.Left,
        RiverSide.Left);

    public bool IsGoal =>
        FarmerSide == RiverSide.Right &&
        WolfSide == RiverSide.Right &&
        GoatSide == RiverSide.Right &&
        CabbageSide == RiverSide.Right;

    public bool IsSafe =>
        !(WolfSide == GoatSide && FarmerSide != WolfSide) &&
        !(GoatSide == CabbageSide && FarmerSide != GoatSide);

    public IEnumerable<(RiverCrossingState NextState, string MoveDescription)> GetNextStates()
    {
        var oppositeSide = GetOppositeSide(FarmerSide);

        var aloneState = this with { FarmerSide = oppositeSide };
        if (aloneState.IsSafe)
        {
            yield return (aloneState, $"El granjero cruza solo al lado {GetSideLabel(oppositeSide)}");
        }

        if (WolfSide == FarmerSide)
        {
            var wolfState = this with { FarmerSide = oppositeSide, WolfSide = oppositeSide };
            if (wolfState.IsSafe)
            {
                yield return (wolfState, $"El granjero lleva al lobo al lado {GetSideLabel(oppositeSide)}");
            }
        }

        if (GoatSide == FarmerSide)
        {
            var goatState = this with { FarmerSide = oppositeSide, GoatSide = oppositeSide };
            if (goatState.IsSafe)
            {
                yield return (goatState, $"El granjero lleva a la cabra al lado {GetSideLabel(oppositeSide)}");
            }
        }

        if (CabbageSide == FarmerSide)
        {
            var cabbageState = this with { FarmerSide = oppositeSide, CabbageSide = oppositeSide };
            if (cabbageState.IsSafe)
            {
                yield return (cabbageState, $"El granjero lleva la col al lado {GetSideLabel(oppositeSide)}");
            }
        }
    }

    public string Describe()
    {
        var leftBankItems = new List<string>();
        var rightBankItems = new List<string>();

        AddItem(leftBankItems, rightBankItems, "Granjero", FarmerSide);
        AddItem(leftBankItems, rightBankItems, "Lobo", WolfSide);
        AddItem(leftBankItems, rightBankItems, "Cabra", GoatSide);
        AddItem(leftBankItems, rightBankItems, "Col", CabbageSide);

        return $"Izquierda: {FormatBank(leftBankItems)} | Derecha: {FormatBank(rightBankItems)}";
    }

    private static void AddItem(List<string> leftBankItems, List<string> rightBankItems, string itemName, RiverSide itemSide)
    {
        if (itemSide == RiverSide.Left)
        {
            leftBankItems.Add(itemName);
        }
        else
        {
            rightBankItems.Add(itemName);
        }
    }

    private static string FormatBank(List<string> items)
    {
        return items.Count == 0 ? "-" : string.Join(", ", items);
    }

    private static RiverSide GetOppositeSide(RiverSide side)
    {
        return side == RiverSide.Left ? RiverSide.Right : RiverSide.Left;
    }

    private static string GetSideLabel(RiverSide side)
    {
        return side == RiverSide.Left ? "izquierdo" : "derecho";
    }
}
