using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Application.Maze;
using Domain.Maze;
using Presentation.MainMenu;

namespace Presentation.Maze;

public partial class MazeWindow : Window
{
    private readonly App _app;
    private MazeBoard _board = null!;
    private readonly IMazeUseCase _useCase;
    private (int, int) _start = (0, 0);
    private (int, int) _end = (3, 3);
    private (int, int) _playerPosition;
    private List<Rectangle> _cells;
    private bool _isGameOver;
    private CancellationTokenSource? _cancellationSource;

    public MazeWindow(App app, IMazeUseCase useCase)
    {
        _app = app;
        InitializeComponent();
        _useCase = useCase;
        _cells = new List<Rectangle>();
        GenerarLaberintoJugable(25, 25);
    }

    private void GenerarLaberintoJugable(int rows, int cols)
    {
        var mazeConfig = _useCase.GeneratePlayableMaze(rows, cols);
        _board = mazeConfig.board;
        _start = mazeConfig.start;
        _end = mazeConfig.end;
        _playerPosition = _start;
        _isGameOver = false;

        DrawMaze();
    }

    private void DrawMaze()
    {
        GridMaze.Children.Clear();
        GridMaze.RowDefinitions.Clear();
        GridMaze.ColumnDefinitions.Clear();

        for (int r = 0; r < _board.Rows; r++)
            GridMaze.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        for (int c = 0; c < _board.Cols; c++)
            GridMaze.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        _cells.Clear();

        for (int r = 0; r < _board.Rows; r++)
        {
            for (int c = 0; c < _board.Cols; c++)
            {
                var rect = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                    Fill = _board.GetValue(r, c) == 1 ? Brushes.Gray : Brushes.White
                };

                if ((r, c) == _start) rect.Fill = Brushes.Green; // Inicio
                if ((r, c) == _end) rect.Fill = Brushes.Red;   // Fin
                if ((r, c) == _playerPosition) rect.Fill = Brushes.Blue; // Jugador

                Grid.SetRow(rect, r);
                Grid.SetColumn(rect, c);

                GridMaze.Children.Add(rect);
                _cells.Add(rect);
            }
        }
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (_isGameOver) return; // Bloquear si ya terminó

        int newRow = _playerPosition.Item1;
        int newCol = _playerPosition.Item2;

        if (e.Key == System.Windows.Input.Key.Up) newRow--;
        else if (e.Key == System.Windows.Input.Key.Down) newRow++;
        else if (e.Key == System.Windows.Input.Key.Left) newCol--;
        else if (e.Key == System.Windows.Input.Key.Right) newCol++;
        else return;

        if (_board.IsValidMove(newRow, newCol))
        {
            var oldRect = _cells[_playerPosition.Item1 * _board.Cols + _playerPosition.Item2];
            if (_playerPosition == _start) oldRect.Fill = Brushes.Green;
            else oldRect.Fill = Brushes.White;

            _playerPosition = (newRow, newCol);

            var newRect = _cells[_playerPosition.Item1 * _board.Cols + _playerPosition.Item2];
            newRect.Fill = Brushes.Blue;

            if (_playerPosition == _end)
            {
                _isGameOver = true;
                MessageBox.Show("¡Felicidades, ganaste resolviendo el laberinto!", "Meta alcanzada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }

    private void BtnNew_Click(object sender, RoutedEventArgs e)
    {
        _cancellationSource?.Cancel(); // Cancelar animación anterior si está corriendo
        GenerarLaberintoJugable(25, 25);
        GridMaze.Focus(); // Retornar el foco para poder jugar inmediatamente
    }

    private async void BtnSolve_Click(object sender, RoutedEventArgs e)
    {
        _cancellationSource?.Cancel();
        _cancellationSource = new CancellationTokenSource();
        var token = _cancellationSource.Token;

        var path = _useCase.FindPathBFS(_board, _start, _end);

        if (path.Count > 0)
        {
            try
            {
                foreach (var step in path)
                {
                    token.ThrowIfCancellationRequested(); // Finalizar si el token fue cancelado
                    if (step != _start && step != _end)
                    {
                        var rect = _cells[step.Item1 * _board.Cols + step.Item2];
                        rect.Fill = Brushes.Yellow;
                        await Task.Delay(100, token); // Animación simple (Token se encarga de detener el delay)
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Se detuvo la animación por crear un nuevo laberinto o salir
            }
        }
        else
        {
            MessageBox.Show("No se encontró camino.", "Laberinto", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        _cancellationSource?.Cancel();
        MainMenuWindow menu = _app.CreateMainMenuWindow();
        menu.Show();
        this.Close();
    }
}
