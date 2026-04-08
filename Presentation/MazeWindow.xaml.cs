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
    private (int, int) _start = (0, 0);
    private (int, int) _end = (3, 3);
    private (int, int) _playerPosition;
    private List<Rectangle> _cells;
    private bool _isGameOver;
    private CancellationTokenSource? _cancellationSource;

    public MazeWindow()
    {
        InitializeComponent();
        _useCase = new MazeUseCase();
        _cells = new List<Rectangle>();

        GenerarLaberintoJugable(25, 25); // Tamaño más grande para mayor complejidad
    }

    private void GenerarLaberintoJugable(int rows, int cols)
    {
        // Aseguramos dimensiones impares para el algoritmo de paredes de Backtracking
        if (rows % 2 == 0) rows++;
        if (cols % 2 == 0) cols++;

        int[,] grid = new int[rows, cols];

        // 1. Llenar todo de paredes (1)
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                grid[r, c] = 1;

        Random rnd = new Random();

        // 2. Esculpir caminos desde una posición inicial impar (1, 1)
        CarvePassagesFrom(1, 1, grid, rnd);

        // 2.5 Añadir complejidad: derribar algunas paredes adicionales (Braid Maze) 
        // para tener múltiples rutas y evitar un único camino obvio y largo.
        int wallsToRemove = (rows * cols) / 20; // 5% de las celdas
        while (wallsToRemove > 0)
        {
            int r = rnd.Next(1, rows - 1);
            int c = rnd.Next(1, cols - 1);

            // Si es pared, verificar que no rompa los bordes
            if (grid[r, c] == 1)
            {
                // Solo derribar paredes internas
                if (r > 1 && r < rows - 2 && c > 1 && c < cols - 2)
                {
                    grid[r, c] = 0;
                    wallsToRemove--;
                }
            }
        }

        // 3. Establecer inicio y fin
        _start = (1, 1);
        _end = (rows - 2, cols - 2);
        grid[_end.Item1, _end.Item2] = 0; // Asegurar que la meta es camino

        _board = new MazeBoard(grid);
        _playerPosition = _start;
        _isGameOver = false;

        DrawMaze();
    }

    private void CarvePassagesFrom(int r, int c, int[,] grid, Random rnd)
    {
        grid[r, c] = 0; // Marcar como camino

        // Direcciones: Arriba, Abajo, Izquierda, Derecha
        int[] dr = { -2, 2, 0, 0 };
        int[] dc = { 0, 0, -2, 2 };

        // Aleatorizar el orden de las direcciones
        int[] dirs = { 0, 1, 2, 3 };
        for (int i = dirs.Length - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            int temp = dirs[i]; dirs[i] = dirs[j]; dirs[j] = temp;
        }

        foreach (var dir in dirs)
        {
            int nr = r + dr[dir];
            int nc = c + dc[dir];

            // Validar que está dentro de los límites
            if (nr > 0 && nr < grid.GetLength(0) - 1 && nc > 0 && nc < grid.GetLength(1) - 1)
            {
                if (grid[nr, nc] == 1) // Si el destino sigue siendo pared
                {
                    grid[r + (dr[dir] / 2), c + (dc[dir] / 2)] = 0; // Romper pared intermedia
                    CarvePassagesFrom(nr, nc, grid, rnd); // Llamada recursiva
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
        MainMenuWindow menu = new MainMenuWindow();
        menu.Show();
        this.Close();
    }
}
