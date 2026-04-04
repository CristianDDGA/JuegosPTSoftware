using Domain.TicTacToe;

namespace Application.TicTacToe;

public interface ITicTacToeUseCase
{

    TicTacToeBoard Board { get; }

    // Acciones que puede pedir la interfaz gráfica
    void PlayHumanMove(int row, int col);
    void PlayComputerMove();
    void RestartGame();
    GameStatus GetCurrentStatus();

}
