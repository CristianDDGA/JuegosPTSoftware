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

namespace Presentation;

public partial class MazeWindow : Window
{
    private MazeBoard _board;
    private IMazeUseCase _useCase;
    private (int row, int column) _start = (0, 0);
    private (int row, int column) _end = (3, 3);
    private (int row, int column) _playerPosition;
    private List<Rectangle> _cells;
    private bool _isGameOver;
    private CancellationTokenSource? _cancellationSource;

    public MazeWindow()
    {
        InitializeComponent();
        _useCase = new MazeUseCase();
        _cells = new List<Rectangle>();

        GenerarLaberintoJugable(25, 25);
    }

    private void GenerarLaberintoJugable(int rows, int cols)
    {
        if (rows % 2 == 0) rows++;
        if (cols % 2 == 0) cols++;

        int[,] grid = new int[rows, cols];

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                grid[r, c] = 1;

        Random rnd = new Random();
        CarvePassagesFrom(1, 1, grid, rnd);

        int wallsToRemove = (rows * cols) / 20;
        while (wallsToRemove > 0)
        {
            int wallRow = rnd.Next(1, rows - 1);
            int wallCol = rnd.Next(1, cols - 1);

            if (grid[wallRow, wallCol] == 1)
            {
                if (wallRow > 1 && wallRow < rows - 2 && wallCol > 1 && wallCol < cols - 2)
                {
                    grid[wallRow, wallCol] = 0;
                    wallsToRemove--;
                }
            }
        }

        _start = (1, 1);
        _end = (rows - 2, cols - 2);
        grid[_end.Item1, _end.Item2] = 0;

        _board = new MazeBoard(grid);
        _playerPosition = _start;
        _isGameOver = false;

        DrawMaze();
    }

    private void CarvePassagesFrom(int row, int col, int[,] grid, Random rnd)
    {
        grid[row, col] = 0;

        int[] dr = { -2, 2, 0, 0 };
        int[] dc = { 0, 0, -2, 2 };

        int[] dirs = { 0, 1, 2, 3 };
        for (int i = dirs.Length - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            int temp = dirs[i]; dirs[i] = dirs[j]; dirs[j] = temp;
        }

        foreach (var dir in dirs)
        {
            int nr = row + dr[dir];
            int nc = col + dc[dir];

            if (nr > 0 && nr < grid.GetLength(0) - 1 && nc > 0 && nc < grid.GetLength(1) - 1)
            {
                if (grid[nr, nc] == 1)
                {
                    grid[row + (dr[dir] / 2), col + (dc[dir] / 2)] = 0;
                    CarvePassagesFrom(nr, nc, grid, rnd);
                }
            }
        }
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

                if ((r, c) == _start) rect.Fill = Brushes.Green;
                if ((r, c) == _end) rect.Fill = Brushes.Red;
                if ((r, c) == _playerPosition) rect.Fill = Brushes.Blue;

                Grid.SetRow(rect, r);
                Grid.SetColumn(rect, c);

                GridMaze.Children.Add(rect);
                _cells.Add(rect);
            }
        }
    }

    private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (_isGameOver) return;

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
        _cancellationSource?.Cancel();
        GenerarLaberintoJugable(25, 25);
        GridMaze.Focus();
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
                    token.ThrowIfCancellationRequested();
                    if (step != _start && step != _end)
                    {
                        var rect = _cells[step.row * _board.Cols + step.column];
                        rect.Fill = Brushes.Yellow;
                        await Task.Delay(100, token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
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
        MainMenuWindow menu = new MainMenuWindow();
        menu.Show();
        this.Close();
    }
}
