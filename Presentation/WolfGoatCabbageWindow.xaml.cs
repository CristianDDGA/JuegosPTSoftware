using Application.WolfGoatCabbage;
using Domain.WolfGoatCabbage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation;

public partial class WolfGoatCabbageWindow : Window
{
    private readonly IRiverCrossingUseCase _useCase;
    private readonly ObservableCollection<RiverCrossingStepCard> _stepCards;

    public WolfGoatCabbageWindow()
    {
        InitializeComponent();
        _useCase = new RiverCrossingUseCase();
        _stepCards = new ObservableCollection<RiverCrossingStepCard>();
        StepsItemsControl.ItemsSource = _stepCards;
        ShowInitialMessage();
    }

    private void BtnSolve_Click(object sender, RoutedEventArgs e)
    {
        var solution = _useCase.Solve();
        ShowSolution(solution);
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        ShowInitialMessage();
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        var menu = new MainMenuWindow();
        menu.Show();
        Close();
    }

    private void ShowInitialMessage()
    {
        TxtSummary.Text = "Presiona Resolver para calcular la secuencia más segura de cruces.";
        _stepCards.Clear();
    }

    private void ShowSolution(RiverCrossingSolution solution)
    {
        TxtSummary.Text = solution.IsSolved
            ? $"Solución encontrada en {solution.MoveCount} movimientos. Sigue la secuencia paso a paso abajo."
            : "No se encontró una solución válida para este estado inicial.";

        _stepCards.Clear();

        for (int index = 0; index < solution.States.Count; index++)
        {
            var state = solution.States[index];
            var isInitialStep = index == 0;
            var actionText = isInitialStep ? "Estado inicial del problema" : solution.Moves[index - 1];

            _stepCards.Add(new RiverCrossingStepCard
            {
                Title = isInitialStep ? "Estado inicial" : $"Paso {index}",
                BadgeText = isInitialStep ? "INICIO" : $"PASO {index}",
                ActionText = actionText,
                LeftSideText = FormatSide(state, RiverSide.Left),
                RightSideText = FormatSide(state, RiverSide.Right)
                , IsExpanded = isInitialStep
            });
        }
    }

    private static string FormatSide(RiverCrossingState state, RiverSide side)
    {
        var items = new List<string>();

        if (state.FarmerSide == side) items.Add("Granjero");
        if (state.WolfSide == side) items.Add("Lobo");
        if (state.GoatSide == side) items.Add("Cabra");
        if (state.CabbageSide == side) items.Add("Col");

        return items.Count == 0 ? "Vacío" : string.Join(" • ", items);
    }

    private sealed class RiverCrossingStepCard
    {
        public string Title { get; set; } = string.Empty;
        public string BadgeText { get; set; } = string.Empty;
        public string ActionText { get; set; } = string.Empty;
        public string LeftSideText { get; set; } = string.Empty;
        public string RightSideText { get; set; } = string.Empty;
        public bool IsExpanded { get; set; }
    }
}
