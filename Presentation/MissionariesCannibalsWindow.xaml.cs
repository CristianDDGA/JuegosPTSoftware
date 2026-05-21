using Application.MissionariesCannibals;
using Domain.MissionariesCannibals;
using System.Collections.ObjectModel;
using System.Windows;

namespace Presentation;

public partial class MissionariesCannibalsWindow : Window
{
    private readonly IMissionariesCannibalsUseCase _useCase;
    private readonly ObservableCollection<MissionariesStepCard> _stepCards;

    public MissionariesCannibalsWindow()
    {
        InitializeComponent();
        _useCase = new MissionariesCannibalsUseCase();
        _stepCards = new ObservableCollection<MissionariesStepCard>();
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
        TxtSummary.Text = "Presiona Resolver para calcular la secuencia óptima de cruces con BFS.";
        _stepCards.Clear();
    }

    private void ShowSolution(MissionariesSolution solution)
    {
        TxtSummary.Text = solution.IsSolved
            ? $"Solución óptima encontrada en {solution.MoveCount} movimientos. La secuencia cumple las reglas de seguridad en cada paso."
            : "No se encontró una solución válida para el estado inicial.";

        _stepCards.Clear();

        for (int index = 0; index < solution.States.Count; index++)
        {
            var state = solution.States[index];
            var isInitialStep = index == 0;
            var actionText = isInitialStep
                ? "Estado inicial: todos en la orilla izquierda, barca a la izquierda."
                : solution.Moves[index - 1];

            _stepCards.Add(new MissionariesStepCard
            {
                Title = isInitialStep ? "Estado inicial" : $"Paso {index}",
                BadgeText = isInitialStep ? "INICIO" : $"PASO {index}",
                ActionText = actionText,
                LeftSideText = state.DescribeBank(leftBank: true),
                RightSideText = state.DescribeBank(leftBank: false)
            });
        }
    }

    private sealed class MissionariesStepCard
    {
        public string Title { get; set; } = string.Empty;
        public string BadgeText { get; set; } = string.Empty;
        public string ActionText { get; set; } = string.Empty;
        public string LeftSideText { get; set; } = string.Empty;
        public string RightSideText { get; set; } = string.Empty;
    }
}
