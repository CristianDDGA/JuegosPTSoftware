using Domain.WolfGoatCabbage;

namespace Application.WolfGoatCabbage;

public class RiverCrossingUseCase : IRiverCrossingUseCase
{
    public RiverCrossingSolution Solve()
    {
        var initialState = RiverCrossingState.Initial;
        var queue = new Queue<RiverCrossingState>();
        var visitedStates = new HashSet<RiverCrossingState>();
        var parentMap = new Dictionary<RiverCrossingState, RiverCrossingState>();
        var moveMap = new Dictionary<RiverCrossingState, string>();

        queue.Enqueue(initialState);
        visitedStates.Add(initialState);

        RiverCrossingState? goalState = null;

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
            return new RiverCrossingSolution
            {
                IsSolved = false,
                States = [initialState],
                Moves = ["No solution found"]
            };
        }

        var states = new List<RiverCrossingState>();
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

        return new RiverCrossingSolution
        {
            IsSolved = true,
            States = states,
            Moves = moves
        };
    }
}
