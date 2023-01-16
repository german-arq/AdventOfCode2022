using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day12;

internal class Day12Solution : Solution
{
    public Day12Solution() : base(12, "Hill Climbing Algorithm", "input.txt") { }

    public override void Solve()
    {
        const char START_POSITION_MARKER = 'S';
        const char BEST_SIGNAL_POSITION_MARKER = 'E';
        
        var input = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        Position[,] heigthmap = new Position[input[0].Length, input.Length];

        Position startPosition = new Position(0, 0, 0);
        Position bestSignalPosition = new Position(0, 0, 0);

        for (int x = 0; x < input[0].Length; x++)
        {
            for (int y = 0; y < input.Length; y++)
            {
                var elevation = input[y][x];
                if(elevation == START_POSITION_MARKER)
                {
                    startPosition.Y = y;
                    startPosition.X = x;
                    startPosition.Elevation = 1;
                    heigthmap[x, y] = startPosition;                    
                }
                else if (elevation == BEST_SIGNAL_POSITION_MARKER)
                {
                    bestSignalPosition.Y = y;
                    bestSignalPosition.X = x;
                    bestSignalPosition.Elevation = 26;
                    heigthmap[x, y] = bestSignalPosition;
                }
                else
                {                    
                    heigthmap[x, y] = new Position(x, y, elevation - 96);
                }                
            }
        }

        // Part 1

        var hillClimbing = new BestSignalPositionSearchProblem(startPosition, bestSignalPosition, heigthmap);

        (List<Step> solutionPath, List<string> solutionMovements) = hillClimbing.BFSearch();

        // Console.WriteLine(string.Join(",\n", solutionPath));

        Part1Solution = $"The fewest steps required to move from the start position to the best signal position is {solutionPath.Count}.";

        // Part 2

        var startsPositions = heigthmap
            .Cast<Position>()
            .Where(p => p.Elevation == 1)
            .ToList();

        var stepsToBestSignalPosition = new List<int>();

        var cycle = 0;

        foreach (var start in startsPositions)
        {
            var hillClimbingFromElevation1 = new BestSignalPositionSearchProblem(start, bestSignalPosition, heigthmap);
            (List<Step> solutionPathFromElevation1, List<string> solutionMovementsFromElevation1) = hillClimbingFromElevation1.BFSearch();
            
            if(solutionPathFromElevation1.Count > 0)
            {
                stepsToBestSignalPosition.Add(solutionPathFromElevation1.Count);
                cycle++;
                // Console.WriteLine($"{cycle}- From X: {start.X}, Y:{start.Y}, Total Steps: {solutionPath.Count}");
            }
            
            
        }

        Part2Solution = $"The fewest steps required to move from the any square with an elevation of 1 to the best signal position is {stepsToBestSignalPosition.Min()}.";

    }

    class BestSignalPositionSearchProblem
    {
        private Position InitialPosition { get; set; }
        private Position GoalPosition { get; set; }
        private Position[,] Heigthmap { get; set; }
        public BestSignalPositionSearchProblem(Position initialPosition, Position goalPosition, Position[,] heigthmap)
        {
            InitialPosition = initialPosition;
            GoalPosition = goalPosition;
            Heigthmap = heigthmap;
        }
        
        public bool IsGoal(Position position) => position == GoalPosition;

        public List<string> ValidMovements(Position position, Position lastPosition)
        {
            var validMovements = new List<string>();
            if (position.X > 1)
            {
                var leftPosition = Heigthmap[position.X - 1, position.Y];
                if (leftPosition != lastPosition && leftPosition.Elevation - position.Elevation <= 1)
                    validMovements.Add("left");                
            }
            if (position.X < Heigthmap.GetLength(0) - 1)
            {
                var rightPosition = Heigthmap[position.X + 1, position.Y];
                if (rightPosition != lastPosition && rightPosition.Elevation - position.Elevation <= 1)
                    validMovements.Add("right");
            }
            if (position.Y > 0)
            {
                var upPosition = Heigthmap[position.X, position.Y - 1];
                if (upPosition != lastPosition && upPosition.Elevation - position.Elevation <= 1)
                    validMovements.Add("up");
            }
            if (position.Y < Heigthmap.GetLength(1) - 1)
            {
                var downPosition = Heigthmap[position.X, position.Y + 1];
                if (downPosition != lastPosition && downPosition.Elevation - position.Elevation <= 1)
                    validMovements.Add("down");
            }
            
            return validMovements;
        }    
        
        public Step Move(Step currentStep, string movement)
        {
            return movement switch
            {
                "up" => new Step(Heigthmap[currentStep.Position.X, currentStep.Position.Y - 1], currentStep, movement),
                "down" => new Step(Heigthmap[currentStep.Position.X, currentStep.Position.Y + 1], currentStep, movement),
                "left" => new Step(Heigthmap[currentStep.Position.X - 1, currentStep.Position.Y], currentStep, movement),
                "right" => new Step(Heigthmap[currentStep.Position.X + 1, currentStep.Position.Y], currentStep, movement),
                _ => throw new ArgumentException("Invalid movement"),
            };
        }

        public (List<Step>, List<string>) BFSearch()
        { 
            var frontier = new Queue<Step>();
            var reached = new List<Step>();

            var initial = new Step(InitialPosition);

            frontier.Enqueue(initial);

            while (frontier.Count > 0)
            {
                var currentStep = frontier.Dequeue();

                if (IsGoal(currentStep.Position))
                {
                    return (currentStep.GetPositionsPath(), currentStep.GetMovementsPath());
                } 
                
                var lastPosition = currentStep == initial ? new Position(-1, -1, 1) : currentStep.LastStep.Position;
                var movements = ValidMovements(currentStep.Position, lastPosition);

                foreach (var movement in movements)
                {
                    var nextStep = Move(currentStep, movement);
                    if (!reached.Contains(nextStep))
                    {
                        reached.Add(nextStep);
                        frontier.Enqueue(nextStep);
                    }                    
                }                
            }            
            return (new List<Step>(), new List<string>());
        }
    
        public (List<Step>, List<string>) DijkstraSearch()
        {
            {
                var frontier = new Queue<Step>();
                var reached = new List<Step>();

                var initial = new Step(new Position(InitialPosition.X, InitialPosition.Y, 'a'));

                frontier.Enqueue(initial);

                while (frontier.Count > 0)
                {
                    var currentStep = frontier.Dequeue();
                    Console.WriteLine(currentStep);

                    if (IsGoal(currentStep.Position))
                    {
                        return (currentStep.GetPositionsPath(), currentStep.GetMovementsPath());
                    }

                    if (!reached.Contains(currentStep))
                    {
                        reached.Add(currentStep);

                        var lastPosition = currentStep == initial ? new Position(-1, -1, 1) : currentStep.LastStep.Position;
                        var movements = ValidMovements(currentStep.Position, lastPosition);

                        if (movements.Count == 0)
                            Console.WriteLine("\n-----Se rompe el ciclo-----\n");

                        foreach (var movement in movements)
                        {
                            var nextStep = Move(currentStep, movement);
                            frontier.Enqueue(nextStep);
                        }
                    }
                }

                return (null, null);
            }
        }
    }

    class Step
    {      
        public Position Position { get; set; }
        public Step? LastStep { get; set; }
        public string? Movement { get; set; }

        public Step(Position position, Step? lastStep = null, string? movement = null)
        {
            Position = position;
            LastStep = lastStep;
            Movement = movement;
        }
        public override bool Equals(object obj)
        {
            if (obj is Step step)
            {
                return Position.X == step.Position.X && Position.Y == step.Position.Y;
            }
            return false;
        }        

        public List<Step> GetPositionsPath()
        {
            var path = new List<Step>();

            var current = this;

            while (current.LastStep != null)
            {
                path.Add(current);
                current = current.LastStep;
            }

            path.Reverse();

            return path;
        }

        public List<string> GetMovementsPath()
        {
            var path = new List<string>();

            var current = this;

            while (current.LastStep != null)
            {
                path.Add(current.Movement);
                current = current.LastStep;
            }
            
            path.Reverse();
            
            return path;
        }

        public override string ToString()
        {
            return $"X: {Position.X}, Y: {Position.Y}, Elevation: {Position.Elevation}, Movement to get here: {Movement}";
        }
    }
    
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Elevation { get; set; }

        public Position(int x, int y, int elevation)
        {
            X = x;
            Y = y;
            Elevation = elevation;
        }
        public override bool Equals(object obj)
        {
            if (obj is Position position)
            {
                return position.X == X && position.Y == Y;
            }
            return false;
        }
    }
}