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

namespace Presentation;

/// <summary>
/// Lógica de interacción para MainMenuWindow.xaml
/// </summary>
public partial class MainMenuWindow : Window
{
    public MainMenuWindow()
    {
        InitializeComponent();
    }

    private void BtnTicTacToe_Click(object sender, RoutedEventArgs e)
    {
        // Instanciamos y mostramos tu ventana
        TicTacToeWindow ticTacToe = new TicTacToeWindow();
        ticTacToe.Show();

        // Cerramos el menú
        this.Close();
    }

    private void btnAbrirNReinas_Click(object sender, RoutedEventArgs e)
    {
        NReinasWindow ventanaReinas = new NReinasWindow();
        ventanaReinas.Show();
        this.Close();
    }

    private void btnAbrirCaballo_Click(object sender, RoutedEventArgs e)
    {
        // Abrir la ventana del Problema del Caballo
        try
        {
            var ventana = new ProblemaCaballoWindow();
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
        ProblemaViajeroWindow ventanaViajero = new ProblemaViajeroWindow();
        ventanaViajero.Show();
        this.Close();
    }
    private void BtnLaberinto_Click(object sender, RoutedEventArgs e)
    {
        MazeWindow ventanaLaberinto = new MazeWindow();
        ventanaLaberinto.Show();
        this.Close();
    }

    private void BtnAbrirWolfGoatCabbage_Click(object sender, RoutedEventArgs e)
    {
        WolfGoatCabbageWindow wolfGoatCabbageWindow = new WolfGoatCabbageWindow();
        wolfGoatCabbageWindow.Show();
        this.Close();
    }
}
