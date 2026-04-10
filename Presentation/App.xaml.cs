using System.Windows;
using Application.Maze;
using Application.NReinas;
using Application.ProblemaCaballo;
using Application.ProblemaViajero;
using Application.TicTacToe;
using Presentation.MainMenu;
using Presentation.NReinas;
using Presentation.ProblemaCaballo;
using Presentation.ProblemaViajero;
using Presentation.TicTacToe;
using Presentation.Maze;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly ITicTacToeUseCase _ticTacToeUseCase = new TicTacToeUseCase();
        private readonly IMazeUseCase _mazeUseCase = new MazeUseCase();
        private readonly INReinasUseCase _nReinasUseCase = new NReinasUseCase();
        private readonly IProblemaCaballoUseCase _problemaCaballoUseCase = new ProblemaCaballoUseCase();
        private readonly IViajeroUseCase _viajeroUseCase = new ViajeroUseCase();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = CreateMainMenuWindow();
            MainWindow.Show();
        }

        public MainMenuWindow CreateMainMenuWindow()
        {
            return new MainMenuWindow(this);
        }

        public TicTacToeWindow CreateTicTacToeWindow()
        {
            return new TicTacToeWindow(this, _ticTacToeUseCase);
        }

        public MazeWindow CreateMazeWindow()
        {
            return new MazeWindow(this, _mazeUseCase);
        }

        public NReinasWindow CreateNReinasWindow()
        {
            return new NReinasWindow(this, _nReinasUseCase);
        }

        public ProblemaCaballoWindow CreateProblemaCaballoWindow()
        {
            return new ProblemaCaballoWindow(this, _problemaCaballoUseCase);
        }

        public ProblemaViajeroWindow CreateProblemaViajeroWindow()
        {
            return new ProblemaViajeroWindow(this, _viajeroUseCase);
        }
    }

}
