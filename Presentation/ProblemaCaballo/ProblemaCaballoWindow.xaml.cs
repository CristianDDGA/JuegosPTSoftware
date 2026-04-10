using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Application.ProblemaCaballo;
using Presentation.MainMenu;

namespace Presentation.ProblemaCaballo;

public partial class ProblemaCaballoWindow : Window
{
    private readonly App _app;
    private readonly IProblemaCaballoUseCase _useCase;
    private const int Size = 8;
    private readonly Button[,] _cells = new Button[Size, Size];
    private readonly CellState[,] _states = new CellState[Size, Size];
    private int _currentRow = -1;
    private int _currentCol = -1;
    private int _visitedCount = 0;
    private bool _gameStarted = false;

    public ProblemaCaballoWindow(App app, IProblemaCaballoUseCase useCase)
    {
        _app = app;
        _useCase = useCase;
        InitializeComponent();
        BuildBoard();
        ResetBoard();
    }

    private enum CellState { Empty, Current, Visited, Usable }

    private void BuildBoard()
    {
        BoardGrid.Rows = Size;
        BoardGrid.Columns = Size;

        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
            {
                var btn = new Button()
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFCC")),
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(0),
                    Tag = (r, c),
                    FontWeight = FontWeights.Bold,
                    FontSize = 16,
                    Foreground = Brushes.White
                };
                btn.Click += Cell_Click;
                _cells[r, c] = btn;
                _states[r, c] = CellState.Empty;
                BoardGrid.Children.Add(btn);
            }
        }
    }

    private void ResetBoard()
    {
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
            {
                _states[r, c] = CellState.Empty;
                _cells[r, c].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A"));
                _cells[r, c].Content = string.Empty;
                _cells[r, c].IsEnabled = true;
            }

        _visitedCount = 0;
        _gameStarted = false;
        _currentRow = -1;
        _currentCol = -1;
        UpdateProgress();
    }

    private void StartNewGame(int startRow, int startCol)
    {
        if (_gameStarted) return; // evitar reiniciar si ya hay un juego en curso

        _currentRow = startRow;
        _currentCol = startCol;
        _states[startRow, startCol] = CellState.Current;
        _cells[startRow, startCol].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
        _cells[startRow, startCol].Content = "♞";
        _visitedCount = 1;
        _gameStarted = true;

        UpdateProgress();
        UpdateUsableMoves();
    }

    private void UpdateProgress()
    {
        txtProgress.Text = $"Casillas visitadas: {_visitedCount} de {Size * Size}";
    }

    private void UpdateUsableMoves()
    {
        // clear previous usable marks
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
            {
                if (_states[r, c] == CellState.Usable)
                {
                    _states[r, c] = CellState.Empty;
                    _cells[r, c].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A"));
                    _cells[r, c].Content = string.Empty;
                }
            }

        // compute new usable moves
        var moves = _useCase.ObtenerMovimientosValidos(Size, BuildEstadoTablero(), _currentRow, _currentCol);
        foreach (var move in moves)
        {
            var r = move.Row;
            var c = move.Col;
            _states[r, c] = CellState.Usable;
            _cells[r, c].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
            _cells[r, c].Content = string.Empty;
        }

        // check end conditions
        if (_visitedCount == Size * Size)
        {
            MessageBox.Show("¡Felicidades! Has completado el recorrido perfecto del caballo");
            ResetBoard();
        }
        else if (!moves.Any())
        {
            MessageBox.Show("Fin del juego: Te has quedado sin movimientos. Inténtalo de nuevo.");
            ResetBoard();
        }
    }

    private int[,] BuildEstadoTablero()
    {
        var estado = new int[Size, Size];
        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
            {
                estado[r, c] = _states[r, c] is CellState.Visited or CellState.Current ? 1 : -1;
            }
        }

        return estado;
    }

    private void Cell_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        if (btn.Tag is not ValueTuple<int, int> tuple) return;
        var (r, c) = tuple;

        // Si el juego no ha comenzado, iniciar desde esta casilla
        if (!_gameStarted)
        {
            StartNewGame(r, c);
            return;
        }

        // Si el juego ya comenzó, solo permitir clics en casillas usables
        if (_states[r, c] != CellState.Usable) return;

        // mark old current as visited
        _states[_currentRow, _currentCol] = CellState.Visited;
        _cells[_currentRow, _currentCol].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E53935"));
        _cells[_currentRow, _currentCol].Content = string.Empty;

        // set new current
        _currentRow = r;
        _currentCol = c;
        _states[r, c] = CellState.Current;
        _cells[r, c].Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
        _cells[r, c].Content = "♞";

        _visitedCount++;
        UpdateProgress();
        UpdateUsableMoves();
    }

    private void BtnRestart_Click(object sender, RoutedEventArgs e)
    {
        ResetBoard();
    }

    private void BtnVolver_Click(object sender, RoutedEventArgs e)
    {
        MainMenuWindow menu = _app.CreateMainMenuWindow();
        menu.Show();
        this.Close();
    }
}
