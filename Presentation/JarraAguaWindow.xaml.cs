using System.Collections.Generic;
using System.Windows;
using Application.JarraAgua;
using Domain.JarraAgua;

namespace Presentation;

public partial class JarraAguaWindow : Window
{
    private readonly IJarraAguaUseCase _useCase;

    private JarraAguaState _estadoActual;
    private List<JarraAguaState> _historial;

    public JarraAguaWindow()
    {
        InitializeComponent();
        _useCase = new JarraAguaUseCase();
        _estadoActual = new JarraAguaState(0, 0);
        _historial = new List<JarraAguaState> { _estadoActual };
        ActualizarLista();
    }

    private void ActualizarLista()
    {
        SolucionPanel.Children.Clear();
        int paso = 0;
        foreach (var estado in _historial)
        {
            var border = new System.Windows.Controls.Border
            {
                Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#232336"),
                CornerRadius = new CornerRadius(8),
                BorderBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00AAFF"),
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 8),
                Padding = new Thickness(10, 6, 10, 6)
            };
            var stack = new System.Windows.Controls.StackPanel();
            if (paso == 0)
            {
                var header = new System.Windows.Controls.TextBlock
                {
                    Text = "INICIO",
                    FontWeight = FontWeights.Bold,
                    Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00AAFF"),
                    FontSize = 14
                };
                stack.Children.Add(header);
                var desc = new System.Windows.Controls.TextBlock
                {
                    Text = $"Estado inicial: {estado}",
                    Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#B0B8D1"),
                    FontSize = 13
                };
                stack.Children.Add(desc);
            }
            else
            {
                var header = new System.Windows.Controls.TextBlock
                {
                    Text = $"PASO {paso}",
                    FontWeight = FontWeights.Bold,
                    Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00AAFF"),
                    FontSize = 14
                };
                stack.Children.Add(header);
                var desc = new System.Windows.Controls.TextBlock
                {
                    Text = ExplicacionPaso(_historial[paso - 1], estado),
                    Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#B0B8D1"),
                    FontSize = 13
                };
                stack.Children.Add(desc);
                var state = new System.Windows.Controls.TextBlock
                {
                    Text = $"A: {estado.JarraA}L, B: {estado.JarraB}L",
                    Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00FFCC"),
                    FontSize = 15,
                    Margin = new Thickness(0, 2, 0, 0)
                };
                stack.Children.Add(state);
            }
            border.Child = stack;
            SolucionPanel.Children.Add(border);
            paso++;
        }
        // Mostrar mensaje de éxito si alguna jarra tiene exactamente 2 litros
        var ultimo = _historial[^1];
        if ((ultimo.JarraA == 2 || ultimo.JarraB == 2) && _historial.Count > 1)
        {
            var border = new System.Windows.Controls.Border
            {
                Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#1A3A2A"),
                CornerRadius = new CornerRadius(8),
                BorderBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00FFCC"),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(0, 8, 0, 0),
                Padding = new Thickness(10, 8, 10, 8)
            };
            var msg = new System.Windows.Controls.TextBlock
            {
                Text = "¡Felicitaciones! Se obtuvo exactamente 2 litros en una de las jarras.",
                Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00FFCC"),
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                TextAlignment = TextAlignment.Center
            };
            border.Child = msg;
            SolucionPanel.Children.Add(border);
        }
    }

    private string ExplicacionPaso(JarraAguaState anterior, JarraAguaState actual)
    {
        if (actual.JarraA > anterior.JarraA && actual.JarraB == anterior.JarraB)
            return $"Llenar jarra A (ahora A: {actual.JarraA}L).";
        if (actual.JarraB > anterior.JarraB && actual.JarraA == anterior.JarraA)
            return $"Llenar jarra B (ahora B: {actual.JarraB}L).";
        if (actual.JarraA < anterior.JarraA && actual.JarraB == anterior.JarraB && actual.JarraA == 0)
            return "Vaciar jarra A.";
        if (actual.JarraB < anterior.JarraB && actual.JarraA == anterior.JarraA && actual.JarraB == 0)
            return "Vaciar jarra B.";
        if (actual.JarraA < anterior.JarraA && actual.JarraB > anterior.JarraB)
            return $"Transferir de A a B ({anterior.JarraA}L, {anterior.JarraB}L → {actual.JarraA}L, {actual.JarraB}L).";
        if (actual.JarraB < anterior.JarraB && actual.JarraA > anterior.JarraA)
            return $"Transferir de B a A ({anterior.JarraA}L, {anterior.JarraB}L → {actual.JarraA}L, {actual.JarraB}L).";
        return "";
    }

    private void BtnLlenarA_Click(object sender, RoutedEventArgs e)
    {
        if (_estadoActual.JarraA < 4)
        {
            _estadoActual = new JarraAguaState(4, _estadoActual.JarraB);
            _historial.Add(_estadoActual);
            ActualizarLista();
        }
    }
    private void BtnLlenarB_Click(object sender, RoutedEventArgs e)
    {
        if (_estadoActual.JarraB < 3)
        {
            _estadoActual = new JarraAguaState(_estadoActual.JarraA, 3);
            _historial.Add(_estadoActual);
            ActualizarLista();
        }
    }
    private void BtnVaciarA_Click(object sender, RoutedEventArgs e)
    {
        if (_estadoActual.JarraA > 0)
        {
            _estadoActual = new JarraAguaState(0, _estadoActual.JarraB);
            _historial.Add(_estadoActual);
            ActualizarLista();
        }
    }
    private void BtnVaciarB_Click(object sender, RoutedEventArgs e)
    {
        if (_estadoActual.JarraB > 0)
        {
            _estadoActual = new JarraAguaState(_estadoActual.JarraA, 0);
            _historial.Add(_estadoActual);
            ActualizarLista();
        }
    }
    private void BtnTransferirAB_Click(object sender, RoutedEventArgs e)
    {
        // Transferir de A a B hasta llenar B o vaciar A
        if (_estadoActual.JarraA > 0 && _estadoActual.JarraB < 3)
        {
            int transfer = System.Math.Min(_estadoActual.JarraA, 3 - _estadoActual.JarraB);
            _estadoActual = new JarraAguaState(_estadoActual.JarraA - transfer, _estadoActual.JarraB + transfer);
            _historial.Add(_estadoActual);
            ActualizarLista();
        }
    }
    private void BtnTransferirBA_Click(object sender, RoutedEventArgs e)
    {
        // Transferir de B a A hasta llenar A o vaciar B
        if (_estadoActual.JarraB > 0 && _estadoActual.JarraA < 4)
        {
            int transfer = System.Math.Min(_estadoActual.JarraB, 4 - _estadoActual.JarraA);
            _estadoActual = new JarraAguaState(_estadoActual.JarraA + transfer, _estadoActual.JarraB - transfer);
            _historial.Add(_estadoActual);
            ActualizarLista();
        }
    }

    private void BtnResolver_Click(object sender, RoutedEventArgs e)
    {
        var solucion = _useCase.Resolver();
        SolucionPanel.Children.Clear();
        if (solucion.Count == 0)
        {
            var border = new System.Windows.Controls.Border
            {
                Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#232336"),
                CornerRadius = new CornerRadius(8),
                BorderBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#FF0055"),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(0, 0, 0, 8),
                Padding = new Thickness(10, 8, 10, 8)
            };
            var msg = new System.Windows.Controls.TextBlock
            {
                Text = "No hay solución.",
                Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#FF0055"),
                FontWeight = FontWeights.Bold,
                FontSize = 15,
                TextAlignment = TextAlignment.Center
            };
            border.Child = msg;
            SolucionPanel.Children.Add(border);
        }
        else
        {
            for (int i = 0; i < solucion.Count; i++)
            {
                var estado = solucion[i];
                var border = new System.Windows.Controls.Border
                {
                    Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#232336"),
                    CornerRadius = new CornerRadius(8),
                    BorderBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00AAFF"),
                    BorderThickness = new Thickness(1),
                    Margin = new Thickness(0, 0, 0, 8),
                    Padding = new Thickness(10, 6, 10, 6)
                };
                var stack = new System.Windows.Controls.StackPanel();
                if (i == 0)
                {
                    var header = new System.Windows.Controls.TextBlock
                    {
                        Text = "INICIO",
                        FontWeight = FontWeights.Bold,
                        Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00AAFF"),
                        FontSize = 14
                    };
                    stack.Children.Add(header);
                    var desc = new System.Windows.Controls.TextBlock
                    {
                        Text = $"Estado inicial: {estado}",
                        Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#B0B8D1"),
                        FontSize = 13
                    };
                    stack.Children.Add(desc);
                }
                else
                {
                    var header = new System.Windows.Controls.TextBlock
                    {
                        Text = $"PASO {i}",
                        FontWeight = FontWeights.Bold,
                        Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00AAFF"),
                        FontSize = 14
                    };
                    stack.Children.Add(header);
                    var desc = new System.Windows.Controls.TextBlock
                    {
                        Text = ExplicacionPaso(solucion[i - 1], estado),
                        Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#B0B8D1"),
                        FontSize = 13
                    };
                    stack.Children.Add(desc);
                    var state = new System.Windows.Controls.TextBlock
                    {
                        Text = $"A: {estado.JarraA}L, B: {estado.JarraB}L",
                        Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00FFCC"),
                        FontSize = 15,
                        Margin = new Thickness(0, 2, 0, 0)
                    };
                    stack.Children.Add(state);
                }
                border.Child = stack;
                SolucionPanel.Children.Add(border);
            }
            var final = solucion[^1];
            if (final.JarraA == 2 || final.JarraB == 2)
            {
                var border = new System.Windows.Controls.Border
                {
                    Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#1A3A2A"),
                    CornerRadius = new CornerRadius(8),
                    BorderBrush = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00FFCC"),
                    BorderThickness = new Thickness(2),
                    Margin = new Thickness(0, 8, 0, 0),
                    Padding = new Thickness(10, 8, 10, 8)
                };
                var msg = new System.Windows.Controls.TextBlock
                {
                    Text = "¡Felicitaciones! Se obtuvo exactamente 2 litros en una de las jarras.",
                    Foreground = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#00FFCC"),
                    FontWeight = FontWeights.Bold,
                    FontSize = 15,
                    TextAlignment = TextAlignment.Center
                };
                border.Child = msg;
                SolucionPanel.Children.Add(border);
            }
        }
        // Reiniciar el historial para que el usuario pueda volver a jugar
        _estadoActual = new JarraAguaState(0, 0);
        _historial = new List<JarraAguaState> { _estadoActual };
    }

    private void BtnMenu_Click(object sender, RoutedEventArgs e)
    {
        MainMenuWindow menu = new MainMenuWindow();
        menu.Show();
        this.Close();
    }
    private void BtnReiniciar_Click(object sender, RoutedEventArgs e)
    {
        _estadoActual = new JarraAguaState(0, 0);
        _historial = new List<JarraAguaState> { _estadoActual };
        ActualizarLista();
    }
}
