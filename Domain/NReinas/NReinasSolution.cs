using System.Collections.Generic;

namespace Domain.NReinas;

public class NReinasSolution
{
    public NReinasSolution(IReadOnlyList<int> queenColumnsByRow)
    {
        QueenColumnsByRow = queenColumnsByRow;
    }

    public IReadOnlyList<int> QueenColumnsByRow { get; }
}
