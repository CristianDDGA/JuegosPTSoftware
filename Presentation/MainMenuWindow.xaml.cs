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

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
}
