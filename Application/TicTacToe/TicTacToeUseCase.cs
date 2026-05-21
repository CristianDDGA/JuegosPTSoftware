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

    public void PlayComputerMove()
    {
        if (Board.CheckStatus() != GameStatus.InProgress) return;

        int bestScore = int.MinValue;
        int bestMoveRow = -1;
        int bestMoveColumn = -1;

        for (int row = 0; row < 3; row++)
        {
            for (int column = 0; column < 3; column++)
            {
                if (Board.Grid[row, column] == Player.None)
                {
                    Board.MakeMove(row, column, Player.O);

                    int score = Minimax(Board, false);

                    Board.UndoMove(row, column);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMoveRow = row;
                        bestMoveColumn = column;
                    }
                }
            }
        }

        if (bestMoveRow != -1)
        {
            Board.MakeMove(bestMoveRow, bestMoveColumn, Player.O);
        }
    }

    private int Minimax(TicTacToeBoard board, bool isMaximizing)
    {
        GameStatus status = board.CheckStatus();

        if (status == GameStatus.OWins) return 10;
        if (status == GameStatus.XWins) return -10;
        if (status == GameStatus.Draw) return 0;

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (board.Grid[row, column] == Player.None)
                    {
                        board.MakeMove(row, column, Player.O);
                        bestScore = Math.Max(bestScore, Minimax(board, false));
                        board.UndoMove(row, column);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    if (board.Grid[row, column] == Player.None)
                    {
                        board.MakeMove(row, column, Player.X);
                        bestScore = Math.Min(bestScore, Minimax(board, true));
                        board.UndoMove(row, column);
                    }
                }
            }
            return bestScore;
        }
    }
}
