using Application.ProblemaViajero;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Presentation
{
    public partial class ProblemaViajeroWindow : Window
    {
        private readonly IViajeroUseCase _viajeroUseCase;

        public ProblemaViajeroWindow()
        {
            InitializeComponent();
            _viajeroUseCase = new ViajeroUseCase();
        }

        private void btnResolver_Click(object sender, RoutedEventArgs e)
        {
            int[,] distanceMatrix = new int[,]
            {
                { 0, 12, 29, 22, 13 },
                { 12, 0, 19, 3, 25 },
                { 29, 19, 0, 21, 5 },
                { 22, 3, 21, 0, 14 },
                { 13, 25, 5, 14, 0 }
            };

            var resultado = _viajeroUseCase.CalcularMejorRuta(distanceMatrix);

            string visualMap = BuildMatrixVisual(distanceMatrix);

            string reporte = "=== REPORTE DETALLADO DEL VIAJE ===\n\n";

            reporte += "1. MATRIZ DE DISTANCIAS (El Mapa)\n";
            reporte += visualMap + "\n";

            string rutaLetras = string.Join(" -> ", resultado.RutaOptima.ConvertAll(city => (char)('A' + city)));
            reporte += "2. RUTA MÁS EFICIENTE\n";
            reporte += $"{rutaLetras}\n\n";

            reporte += "3. DESGLOSE DEL VIAJE\n";
            reporte += string.Join("\n", resultado.DesglosePasos) + "\n";

            reporte += "\n====================================\n";
            reporte += $"DISTANCIA TOTAL ACUMULADA: {resultado.DistanciaTotal} unidades\n";
            reporte += "====================================";

            txtResultado.Text = reporte;
        }

        private static string BuildMatrixVisual(int[,] matrix)
        {
            int size = matrix.GetLength(0);
            string visual = "     A   B   C   D   E\n";
            visual += "   ---------------------\n";
            for (int rowIndex = 0; rowIndex < size; rowIndex++)
            {
                visual += $"{(char)('A' + rowIndex)} | ";
                for (int colIndex = 0; colIndex < size; colIndex++)
                {
                    visual += $"{matrix[rowIndex, colIndex],2}  ";
                }
                visual += "\n";
            }
            return visual;
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow menu = new MainMenuWindow();
            menu.Show();
            this.Close();
        }
    }
}
