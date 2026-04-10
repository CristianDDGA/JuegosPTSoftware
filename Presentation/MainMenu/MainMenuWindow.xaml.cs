using Presentation.Maze;
using Presentation.NReinas;
using Presentation.ProblemaCaballo;
using Presentation.ProblemaViajero;
using Presentation.TicTacToe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Presentation.MainMenu;

/// <summary>
/// Lógica de interacción para MainMenuWindow.xaml
/// </summary>
public partial class MainMenuWindow : Window
{
    private readonly App _app;

    public MainMenuWindow(App app)
    {
        _app = app;
        InitializeComponent();
    }

    private void BtnTicTacToe_Click(object sender, RoutedEventArgs e)
    {
        TicTacToeWindow ticTacToe = _app.CreateTicTacToeWindow();
        ticTacToe.Show();

        // Cerramos el menú
        this.Close();
    }

    private void btnAbrirNReinas_Click(object sender, RoutedEventArgs e)
    {
        NReinasWindow ventanaReinas = _app.CreateNReinasWindow();
        ventanaReinas.Show();
        this.Close();
    }

    private void btnAbrirCaballo_Click(object sender, RoutedEventArgs e)
    {
        // Abrir la ventana del Problema del Caballo
        try
        {
            var ventana = _app.CreateProblemaCaballoWindow();
            ventana.Show();
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al abrir la ventana del Caballo: {ex.Message}");
        }
    }
    private void btnAbrirViajero_Click(object sender, RoutedEventArgs e)
    {
        ProblemaViajeroWindow ventanaViajero = _app.CreateProblemaViajeroWindow();
        ventanaViajero.Show();
        this.Close();
    }
    private void BtnLaberinto_Click(object sender, RoutedEventArgs e)
    {
        MazeWindow ventanaLaberinto = _app.CreateMazeWindow();
        ventanaLaberinto.Show();
        this.Close();
    }
}
