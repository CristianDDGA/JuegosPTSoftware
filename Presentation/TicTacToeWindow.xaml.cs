using Application.TicTacToe;
using Domain.TicTacToe;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class TicTacToeWindow : Window
{
    private readonly ITicTacToeUseCase _useCase;

    // Definimos los colores neón usando SolidColorBrush
    private readonly SolidColorBrush colorX = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFCC")); // Cian
    private readonly SolidColorBrush colorO = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0055")); // Rosa Neón

    public TicTacToeWindow()
    {
        InitializeComponent();
        _useCase = new TicTacToeUseCase();
        UpdateUI();
    }

    private void Cell_Click(object sender, RoutedEventArgs e)
    {
        if (_useCase.GetCurrentStatus() != GameStatus.InProgress) return;

        Button clickedButton = (Button)sender;
        string[] coords = clickedButton.Tag.ToString().Split(',');
        int row = int.Parse(coords[0]);
        int col = int.Parse(coords[1]);

        if (_useCase.Board.IsValidMove(row, col))
        {
            _useCase.PlayHumanMove(row, col);
            UpdateUI();

            if (_useCase.GetCurrentStatus() == GameStatus.InProgress)
            {
                TxtStatus.Text = "IA CALCULANDO...";
                TxtStatus.Foreground = colorO;
                System.Windows.Application.Current.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Background);

                _useCase.PlayComputerMove();
                UpdateUI();
            }
        }
    }

    private void BtnRestart_Click(object sender, RoutedEventArgs e)
    {
        _useCase.RestartGame();
        UpdateUI();
    }

    private void UpdateUI()
    {
        int index = 0;
        foreach (UIElement element in GridBoard.Children)
        {
            if (element is Button btn)
            {
                int row = index / 3;
                int col = index % 3;

                Player cellValue = _useCase.Board.Grid[row, col];

                btn.Content = cellValue == Player.None ? "" : cellValue.ToString();
                // Pintar X de Cian y O de Rosa Neón
                btn.Foreground = cellValue == Player.X ? colorX : colorO;
                index++;
            }
        }

        GameStatus currentStatus = _useCase.GetCurrentStatus();
        switch (currentStatus)
        {
            case GameStatus.InProgress:
                TxtStatus.Text = "TU TURNO (X)";
                TxtStatus.Foreground = colorX;
                break;
            case GameStatus.XWins:
                TxtStatus.Text = "¡SISTEMA HACKEADO! (GANASTE)";
                TxtStatus.Foreground = colorX;
                break;
            case GameStatus.OWins:
                TxtStatus.Text = "HAS SIDO ELIMINADO.";
                TxtStatus.Foreground = colorO;
                break;
            case GameStatus.Draw:
                TxtStatus.Text = "EMPATE TÁCTICO.";
                TxtStatus.Foreground = new SolidColorBrush(Colors.Yellow);
                break;
        }
    }

    private void BtnVolver_Click(object sender, RoutedEventArgs e)
    {
        MainMenuWindow menu = new MainMenuWindow();
        menu.Show();
        this.Close();
    }
}