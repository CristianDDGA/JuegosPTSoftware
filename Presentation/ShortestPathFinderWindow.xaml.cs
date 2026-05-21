using Application.ShortestPath;
using Domain.ShortestPath;
using Infrastructure.ShortestPath;
using System.Windows;

namespace Presentation;

public partial class ShortestPathFinderWindow : Window
{
    private readonly FindShortestPathUseCase _useCase;

    public ShortestPathFinderWindow()
    {
        InitializeComponent();

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
        _useCase = new FindShortestPathUseCase(pathFinder, roadRepository);
    }

    private void BtnFindPath_Click(object sender, RoutedEventArgs e)
    {
        txtError.Text = "";
        txtRoute.Text = "";
        txtDistance.Text = "";
        routeStepsList.ItemsSource = null;

        string startCityName = txtStartCity.Text.Trim();
        string endCityName = txtEndCity.Text.Trim();

        if (string.IsNullOrWhiteSpace(startCityName) || string.IsNullOrWhiteSpace(endCityName))
        {
            txtError.Text = "Please enter both a start and destination city.";
            return;
        }

        var result = _useCase.Execute(startCityName, endCityName);

        if (result.IsSuccess)
        {
            txtRoute.Text = string.Join(" → ", result.CityNames);
            txtDistance.Text = $"Total distance: {result.TotalDistanceKilometers} km";

            var steps = new List<string>();
            for (int index = 0; index < result.CityNames.Count - 1; index++)
            {
                steps.Add($"  {index + 1}. {result.CityNames[index]} → {result.CityNames[index + 1]}");
            }
            routeStepsList.ItemsSource = steps;
        }
        else
        {
            txtError.Text = $"Error: {result.ErrorMessage}";
        }
    }

    private void BtnVolver_Click(object sender, RoutedEventArgs e)
    {
        MainMenuWindow menu = new MainMenuWindow();
        menu.Show();
        this.Close();
    }
}
