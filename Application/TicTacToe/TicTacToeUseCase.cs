using Domain.TicTacToe;

namespace Application.TicTacToe;

public class TicTacToeUseCase : ITicTacToeUseCase
{
    public TicTacToeBoard Board { get; private set; }

    public TicTacToeUseCase()
    {
        Board = new TicTacToeBoard();
    }

    public void PlayHumanMove(int row, int col)
    {
        if (Board.IsValidMove(row, col) && Board.CheckStatus() == GameStatus.InProgress)
        {
            Board.MakeMove(row, col, Player.X);
        }
    }

    public void RestartGame()
    {
        Board = new TicTacToeBoard();
    }

    public GameStatus GetCurrentStatus()
    {
        return Board.CheckStatus();
    }

    // --- LÓGICA DE LA IA (MINIMAX) ---

    public void PlayComputerMove()
    {
        if (Board.CheckStatus() != GameStatus.InProgress) return;

        int bestScore = int.MinValue;
        int[] bestMove = new int[] { -1, -1 };

        // Recorrer todo el tablero buscando la mejor jugada inicial
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Board.Grid[i, j] == Player.None)
                {
                    // 1. Simular jugada
                    Board.MakeMove(i, j, Player.O);

                    // 2. Evaluar qué pasaría usando Minimax (falso = es turno del humano)
                    int score = Minimax(Board, false);

                    // 3. Deshacer la jugada simulada
                    Board.UndoMove(i, j);

                    // 4. Si esta jugada da mejor puntaje, guardarla
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove[0] = i;
                        bestMove[1] = j;
                    }
                }
            }
        }

        // Ejecutar la mejor jugada encontrada en el tablero real
        if (bestMove[0] != -1)
        {
            Board.MakeMove(bestMove[0], bestMove[1], Player.O);
        }
    }

    // El algoritmo recursivo
    private int Minimax(TicTacToeBoard board, bool isMaximizing)
    {
        GameStatus status = board.CheckStatus();

        // Condiciones de parada (hojas del árbol)
        if (status == GameStatus.OWins) return 10;   // Gana IA
        if (status == GameStatus.XWins) return -10;  // Gana Humano
        if (status == GameStatus.Draw) return 0;     // Empate

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.Grid[i, j] == Player.None)
                    {
                        board.MakeMove(i, j, Player.O);
                        bestScore = Math.Max(bestScore, Minimax(board, false));
                        board.UndoMove(i, j);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.Grid[i, j] == Player.None)
                    {
                        board.MakeMove(i, j, Player.X);
                        bestScore = Math.Min(bestScore, Minimax(board, true));
                        board.UndoMove(i, j);
                    }
                }
            }
            return bestScore;
        }
    }
}
