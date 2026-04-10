using Application.ProblemaViajero;
using Domain.ProblemaViajero;
using Presentation.MainMenu;
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

namespace Presentation.ProblemaViajero
{
    public partial class ProblemaViajeroWindow : Window
    {
        private readonly App _app;
        private readonly IViajeroUseCase _viajeroUseCase;

        public ProblemaViajeroWindow(App app, IViajeroUseCase viajeroUseCase)
        {
            _app = app;
            InitializeComponent();
            _viajeroUseCase = viajeroUseCase;
        }

        private void btnResolver_Click(object sender, RoutedEventArgs e)
        {
            // OJO: En el futuro, esta matriz debería leerse desde un archivo .txt usando 
            // la capa "Infrastructure" (igual que hacen con su Laberinto). 
            // Por ahora, se la mandamos a la capa de Aplicación.
            int[,] distancias = new int[,]
            {
        { 0, 12, 29, 22, 13 },
        { 12, 0, 19, 3, 25 },
        { 29, 19, 0, 21, 5 },
        { 22, 3, 21, 0, 14 },
        { 13, 25, 5, 14, 0 }
            };

            var resultado = _viajeroUseCase.CalcularMejorRuta(distancias);

            // La vista vuelve a ser "tonta": Solo une los textos y los muestra.
            string reporte = "=== REPORTE DETALLADO DEL VIAJE ===\n\n";

            reporte += "1. MATRIZ DE DISTANCIAS (El Mapa)\n";
            reporte += FormatearMatriz(distancias) + "\n";

            string rutaLetras = string.Join(" -> ", resultado.RutaOptima.ConvertAll(c => (char)('A' + c)));
            reporte += "2. RUTA MÁS EFICIENTE\n";
            reporte += $"{rutaLetras}\n\n";

            reporte += "3. DESGLOSE DEL VIAJE\n";
            reporte += string.Join("\n", resultado.DesglosePasos) + "\n";

            reporte += "\n====================================\n";
            reporte += $"DISTANCIA TOTAL ACUMULADA: {resultado.DistanciaTotal} unidades\n";
            reporte += "====================================";

            txtResultado.Text = reporte;
        }
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow menu = _app.CreateMainMenuWindow();
            menu.Show();
            this.Close();
        }

        private static string FormatearMatriz(int[,] matriz)
        {
            int n = matriz.GetLength(0);
            var sb = new StringBuilder();
            sb.Append("     ");
            for (int i = 0; i < n; i++)
            {
                sb.Append($"{(char)('A' + i),2}  ");
            }

            sb.AppendLine();
            sb.AppendLine("   ---------------------");

            for (int i = 0; i < n; i++)
            {
                sb.Append($"{(char)('A' + i)} | ");
                for (int j = 0; j < n; j++)
                {
                    sb.Append($"{matriz[i, j],2}  ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
