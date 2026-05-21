using Domain.MissionariesCannibals;

namespace Application.MissionariesCannibals;

public class MissionariesCannibalsUseCase : IMissionariesCannibalsUseCase
{
    public MissionariesSolution Solve()
    {
        var initialState = MissionariesState.Initial;
        var queue = new Queue<MissionariesState>();
        var visitedStates = new HashSet<MissionariesState>();
        var parentMap = new Dictionary<MissionariesState, MissionariesState>();
        var moveMap = new Dictionary<MissionariesState, string>();

        queue.Enqueue(initialState);
        visitedStates.Add(initialState);

        MissionariesState? goalState = null;

        while (queue.Count > 0)
        {
            var currentState = queue.Dequeue();

            if (currentState.IsGoal)
            {
                goalState = currentState;
                break;
            }

            foreach (var (nextState, moveDescription) in currentState.GetNextStates())
            {
                if (visitedStates.Contains(nextState))
                {
                    continue;
                }

                visitedStates.Add(nextState);
                parentMap[nextState] = currentState;
                moveMap[nextState] = moveDescription;
                queue.Enqueue(nextState);
            }
        }

        if (goalState is null)
        {
            return new MissionariesSolution
            {
                IsSolved = false,
                States = [initialState],
                Moves = ["No se encontró una solución válida."]
            };
        }

        var states = new List<MissionariesState>();
        var moves = new List<string>();
        var current = goalState;

        while (true)
        {
            states.Add(current);

            if (current == initialState)
            {
                break;
            }

            moves.Add(moveMap[current]);
            current = parentMap[current];
        }

        states.Reverse();
        moves.Reverse();

        return new MissionariesSolution
        {
            IsSolved = true,
            States = states,
            Moves = moves
        };
    }
}
