using System.Collections.Generic;

namespace Domain.MapColoring;

public class MapGraph
{
    // Region names for presentation
    public List<string> Regions { get; }

    // Adjacency list where key is region index and value is list of neighboring region indices
    public Dictionary<int, List<int>> Adjacency { get; }

    public int Count => Regions.Count;

    public MapGraph(List<string> regions, Dictionary<int, List<int>> adjacency)
    {
        Regions = regions;
        Adjacency = adjacency;
    }

    // Helper: example graph (can be replaced by user data or file)
    public static MapGraph ExampleGraph()
    {
        var regions = new List<string> { "A", "B", "C", "D", "E", "F" };
        var adj = new Dictionary<int, List<int>>
        {
            [0] = new List<int> { 1, 2, 3, 4 },    // A connected to B,C,D,E
            [1] = new List<int> { 0, 2, 3, 5 },    // B connected to A,C,D,F
            [2] = new List<int> { 0, 1, 3, 5 },    // C connected to A,B,D,F
            [3] = new List<int> { 0, 1, 2, 4, 5 }, // D connected to A,B,C,E,F
            [4] = new List<int> { 0, 3 },          // E connected to A,D
            [5] = new List<int> { 1, 2, 3 }        // F connected to B,C,D
        };

        return new MapGraph(regions, adj);
    }
}
