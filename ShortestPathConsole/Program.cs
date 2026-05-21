using Application.ShortestPath;
using Domain.ShortestPath;
using Infrastructure.ShortestPath;

var quito = new City("Quito");
var ambato = new City("Ambato");
var riobamba = new City("Riobamba");
var cuenca = new City("Cuenca");

var roads = new List<Road>
{
    new Road(quito, ambato, new Distance(120)),
    new Road(ambato, riobamba, new Distance(60)),
    new Road(riobamba, cuenca, new Distance(200))
};

var roadRepository = new InMemoryRoadRepository(roads);
var pathFinder = new DijkstraShortestPathFinder();
var useCase = new FindShortestPathUseCase(pathFinder, roadRepository);

Console.WriteLine("=== Shortest Path Finder ===");
Console.Write("Enter start city: ");
string? startName = Console.ReadLine();
Console.Write("Enter destination city: ");
string? endName = Console.ReadLine();

if (string.IsNullOrWhiteSpace(startName) || string.IsNullOrWhiteSpace(endName))
{
    Console.WriteLine("City names cannot be empty.");
    return;
}

var result = useCase.Execute(startName, endName);

if (result.IsSuccess)
{
    Console.WriteLine($"Route: {string.Join(" -> ", result.CityNames)}");
    Console.WriteLine($"Total distance: {result.TotalDistanceKilometers} km");
}
else
{
    Console.WriteLine($"Error: {result.ErrorMessage}");
}
