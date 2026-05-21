using Application.MapColoring;
using Domain.MapColoring;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Presentation;

public partial class MapColoringWindow : Window
{
    private readonly IMapColoringUseCase _useCase;
    private readonly List<RegionNodeViewModel> _regions;
    private MapGraph _graph;
    private readonly Dictionary<int, Point> _nodePositions;
    private bool _isSolving;

    public MapColoringWindow()
    {
        InitializeComponent();
        _useCase = new MapColoringUseCase();
        _regions = new List<RegionNodeViewModel>();
        _nodePositions = new Dictionary<int, Point>();

        _graph = MapGraph.ExampleGraph();
        PopulateInitialRegions();
    }

    private void PopulateInitialRegions()
    {
        _regions.Clear();
        _nodePositions.Clear();
        GraphCanvas.Children.Clear();

        ConfigureNodePositions();
        DrawEdges();

        for (int i = 0; i < _graph.Count; i++)
        {
            var neighborsText = string.Empty;
            if (_graph.Adjacency.TryGetValue(i, out var ns))
            {
                var names = new List<string>();
                foreach (var n in ns) names.Add(_graph.Regions[n]);
                neighborsText = names.Count == 0 ? "Sin vecinos" : "Vecinos: " + string.Join(", ", names);
            }

            var node = new RegionNodeViewModel
            {
                RegionName = _graph.Regions[i],
                ColorName = "Sin asignar",
                ColorBrush = Brushes.Gray,
                NeighborsText = neighborsText,
                Position = _nodePositions[i]
            };

            _regions.Add(node);
            DrawNode(i, node);
        }
    }

    private async void BtnSolve_Click(object sender, RoutedEventArgs e)
    {
        if (_isSolving)
        {
            return;
        }

        try
        {
            _isSolving = true;
            BtnSolve.IsEnabled = false;
            BtnReset.IsEnabled = false;
            TxtStatus.Text = "Iniciando backtracking: se probarán colores nodo por nodo.";

            ResetNodeColors();

            var assignments = new List<ColorOption>();
            for (int i = 0; i < _graph.Count; i++)
            {
                assignments.Add(ColorOption.None);
            }

            var solved = await SolveWithAnimationAsync(0, assignments, CancellationToken.None);

            if (!solved)
            {
                TxtStatus.Text = "No se encontró una asignación válida con 4 colores.";
                MessageBox.Show("No se encontró una asignación válida con 4 colores.", "Resultado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                TxtStatus.Text = "Solución encontrada: cada región quedó coloreada sin conflictos.";
            }
        }
        finally
        {
            _isSolving = false;
            BtnSolve.IsEnabled = true;
            BtnReset.IsEnabled = true;
        }
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        if (_isSolving)
        {
            return;
        }

        PopulateInitialRegions();
        TxtStatus.Text = "Mapa reiniciado. Presiona Resolver para ver el algoritmo paso a paso.";
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        var menu = new MainMenuWindow();
        menu.Show();
        Close();
    }

    private static Brush BrushForColor(ColorOption color)
    {
        return color switch
        {
            ColorOption.Red => new SolidColorBrush(Color.FromRgb(0xFF, 0x5A, 0x5A)),
            ColorOption.Green => new SolidColorBrush(Color.FromRgb(0x5A, 0xFF, 0x9A)),
            ColorOption.Blue => new SolidColorBrush(Color.FromRgb(0x5A, 0xC8, 0xFF)),
            ColorOption.Yellow => new SolidColorBrush(Color.FromRgb(0xFF, 0xE6, 0x5A)),
            _ => Brushes.Gray,
        };
    }

    private async Task<bool> SolveWithAnimationAsync(int regionIndex, List<ColorOption> assignments, CancellationToken cancellationToken)
    {
        if (regionIndex >= _graph.Count)
        {
            return true;
        }

        var regionName = _graph.Regions[regionIndex];

        for (int colorIndex = 1; colorIndex <= 4; colorIndex++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var candidate = (ColorOption)colorIndex;
            var candidateName = GetColorDisplayName(candidate);
            TxtStatus.Text = $"Probando {regionName} con {candidateName}...";
            HighlightNode(regionIndex, BrushForColor(candidate), candidateName);
            await Task.Delay(450, cancellationToken);

            if (CanAssignColor(regionIndex, candidate, assignments))
            {
                assignments[regionIndex] = candidate;
                ApplyFinalColor(regionIndex, candidate);
                TxtStatus.Text = $"{regionName} asignado con {candidateName}. Pasando al siguiente nodo.";
                await Task.Delay(350, cancellationToken);

                if (await SolveWithAnimationAsync(regionIndex + 1, assignments, cancellationToken))
                {
                    return true;
                }

                TxtStatus.Text = $"Retrocediendo desde {regionName}: el color {candidateName} no permite completar el grafo.";
                assignments[regionIndex] = ColorOption.None;
                ResetNodeToUnassigned(regionIndex);
                await Task.Delay(350, cancellationToken);
            }
            else
            {
                TxtStatus.Text = $"{candidateName} entra en conflicto con un vecino de {regionName}.";
                await Task.Delay(250, cancellationToken);
            }
        }

        ResetNodeToUnassigned(regionIndex);
        return false;
    }

    private bool CanAssignColor(int regionIndex, ColorOption candidate, List<ColorOption> assignments)
    {
        if (!_graph.Adjacency.TryGetValue(regionIndex, out var neighbors))
        {
            return true;
        }

        foreach (var neighborIndex in neighbors)
        {
            if (assignments[neighborIndex] == candidate)
            {
                return false;
            }
        }

        return true;
    }

    private void HighlightNode(int regionIndex, Brush brush, string colorName)
    {
        var node = _regions[regionIndex];
        node.ColorBrush = brush;
        node.ColorName = $"Probando: {colorName}";
        UpdateNodeAppearance(regionIndex);
    }

    private void ApplyFinalColor(int regionIndex, ColorOption color)
    {
        var node = _regions[regionIndex];
        node.ColorName = GetColorDisplayName(color);
        node.ColorBrush = BrushForColor(color);
        UpdateNodeAppearance(regionIndex);
    }

    private void ResetNodeToUnassigned(int regionIndex)
    {
        var node = _regions[regionIndex];
        node.ColorName = "Sin asignar";
        node.ColorBrush = Brushes.Gray;
        UpdateNodeAppearance(regionIndex);
    }

    private void ResetNodeColors()
    {
        for (int i = 0; i < _regions.Count; i++)
        {
            ResetNodeToUnassigned(i);
        }
    }

    private void ConfigureNodePositions()
    {
        var centerX = 310.0;
        var centerY = 170.0;
        var radiusX = 190.0;
        var radiusY = 118.0;

        for (int index = 0; index < _graph.Count; index++)
        {
            var angle = (Math.PI * 2 * index / _graph.Count) - Math.PI / 2;
            var x = centerX + radiusX * Math.Cos(angle);
            var y = centerY + radiusY * Math.Sin(angle);
            _nodePositions[index] = new Point(x, y);
        }
    }

    private void DrawEdges()
    {
        var edgePen = new Pen(new SolidColorBrush(Color.FromRgb(0x3A, 0x4A, 0x62)), 2);

        foreach (var pair in _graph.Adjacency)
        {
            var fromIndex = pair.Key;
            foreach (var toIndex in pair.Value)
            {
                if (toIndex <= fromIndex)
                {
                    continue;
                }

                var start = _nodePositions[fromIndex];
                var end = _nodePositions[toIndex];

                var line = new Line
                {
                    X1 = start.X,
                    Y1 = start.Y,
                    X2 = end.X,
                    Y2 = end.Y,
                    Stroke = edgePen.Brush,
                    StrokeThickness = edgePen.Thickness,
                    Opacity = 0.7
                };

                GraphCanvas.Children.Add(line);
            }
        }
    }

    private void DrawNode(int index, RegionNodeViewModel node)
    {
        var position = _nodePositions[index];
        var nodeSize = 78.0;

        var nodeBorder = new Border
        {
            Width = nodeSize,
            Height = nodeSize,
            CornerRadius = new CornerRadius(39),
            Background = node.ColorBrush,
            BorderBrush = new SolidColorBrush(Color.FromRgb(0x18, 0x26, 0x38)),
            BorderThickness = new Thickness(2),
            ToolTip = node.NeighborsText
        };

        var nodeText = new TextBlock
        {
            Text = node.RegionName,
            Foreground = Brushes.White,
            FontWeight = FontWeights.Bold,
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };

        var nodeLayout = new Grid();
        nodeLayout.Children.Add(nodeText);
        nodeBorder.Child = nodeLayout;

        Canvas.SetLeft(nodeBorder, position.X - nodeSize / 2);
        Canvas.SetTop(nodeBorder, position.Y - nodeSize / 2);
        GraphCanvas.Children.Add(nodeBorder);

        var caption = new TextBlock
        {
            Text = node.ColorName,
            Foreground = Brushes.White,
            FontSize = 12,
            HorizontalAlignment = HorizontalAlignment.Center,
            TextAlignment = TextAlignment.Center
        };

        Canvas.SetLeft(caption, position.X - 42);
        Canvas.SetTop(caption, position.Y + 48);
        GraphCanvas.Children.Add(caption);

        node.AttachedBorder = nodeBorder;
        node.AttachedCaption = caption;
    }

    private void UpdateNodeAppearance(int index)
    {
        var node = _regions[index];
        if (node.AttachedBorder is not null)
        {
            node.AttachedBorder.Background = node.ColorBrush;
        }

        if (node.AttachedCaption is not null)
        {
            node.AttachedCaption.Text = node.ColorName;
            node.AttachedCaption.Foreground = node.ColorName == "Amarillo" ? Brushes.Black : Brushes.White;
        }
    }

    private sealed class RegionNodeViewModel
    {
        public string RegionName { get; set; } = string.Empty;
        public string ColorName { get; set; } = string.Empty;
        public Brush ColorBrush { get; set; } = Brushes.Gray;
        public string NeighborsText { get; set; } = string.Empty;
        public Point Position { get; set; }
        public Border? AttachedBorder { get; set; }
        public TextBlock? AttachedCaption { get; set; }
    }

    private static string GetColorDisplayName(ColorOption color)
    {
        return color switch
        {
            ColorOption.Red => "Rojo",
            ColorOption.Green => "Verde",
            ColorOption.Blue => "Azul",
            ColorOption.Yellow => "Amarillo",
            _ => "Sin asignar",
        };
    }
}
