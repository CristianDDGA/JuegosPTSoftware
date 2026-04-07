using System;
using System.Collections.Generic;
using System.Windows;
using Application.NReinas; // Asegúrate de que el namespace sea correcto

namespace Presentation
{
    public partial class NReinasWindow : Window
    {
        // Instanciamos el UseCase (Capa de Aplicación)
        private readonly INReinasUseCase _nReinasUseCase;

        public NReinasWindow()
        {
            InitializeComponent();
            _nReinasUseCase = new NReinasUseCase();
        }

        private void btnResolver_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtN.Text, out int n) && n > 0)
            {
                // 1. Obtener las soluciones desde la capa de Aplicación
                var soluciones = _nReinasUseCase.Resolver(n);

                // 2. Limpiar pantalla anterior
                txtResultado.Text = "";

                // 3. Mostrar resultados
                MostrarEnConsolaYPantalla(soluciones, n);
            }
            else
            {
                MessageBox.Show("Por favor, ingresa un número válido mayor a 0.");
            }
        }

        private void MostrarEnConsolaYPantalla(List<int[]> soluciones, int n)
        {
            string reporte = $"Se encontraron {soluciones.Count} soluciones para N={n}:\n\n";
            Console.WriteLine(reporte);

            foreach (var sol in soluciones)
            {
                string tableroVisual = "";
                for (int i = 0; i < n; i++)
                {
                    string fila = "";
                    for (int j = 0; j < n; j++)
                    {
                        fila += (sol[i] == j) ? " Q " : " . ";
                    }
                    tableroVisual += fila + "\n";
                    Console.WriteLine(fila);
                }

                reporte += tableroVisual + new string('-', n * 3) + "\n";
                Console.WriteLine(new string('-', n * 3));
            }
            txtResultado.Text = reporte;
        }
        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow menu = new MainMenuWindow();
            menu.Show();
            this.Close();
        }
    }
}