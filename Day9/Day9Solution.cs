using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day9;

internal class Day9Solution : Solution
{
    public Day9Solution() : base(9, "Rope Bridge") { }

    public override void Solve()
    {
        var moves = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

        // Part 1 
        var shortRope = new Rope(2);

        foreach (var move in moves)
        {
            var instruction = move.Split(" ");
            var direction = instruction[0];
            var steps = int.Parse(instruction[1]);

            shortRope.Move(direction, steps);
        }

        Part1Solution = $"The rope tail visited {shortRope.Tail.Track.Select(position => string.Join("|", position)).Distinct().Count()} positions at least once.";    

        // Part 2
        var longRope = new Rope(10);

        // var testMoves = new List<string>() { "R 5", "U 8", "L 8", "D 3", "R 17", "D 10", "L 25", "U 20" }; 

        foreach (var move in moves)
        {
            var instruction = move.Split(" ");
            var direction = instruction[0];
            var steps = int.Parse(instruction[1]);

            longRope.Move(direction, steps);
        }

        Part2Solution = $"The rope tail visited {longRope.Tail.Track.Select(position => string.Join("|", position)).Distinct().Count()} positions at least once.";
        
    }

    class Rope
    {
        public List<Knot> Knots = new List<Knot>();
        public Knot Head => Knots.First();
        public Knot Tail => Knots.Last();
        public Rope(int numberOfKnots = 2)
        {
            // Knots = new List<Knot>();
            for (int i = 0; i < numberOfKnots; i++)
            {
                Knots.Add(new Knot());
            }
        }

        public void Move(string direction, int steps)
        {
            // Console.WriteLine($"\n ... Move {direction} {steps}");
            
            for (int i = 1; i <= steps; i++)
            {
                // First move head
                int headNewCoordX = Head.CoordX;
                int headNewCoordY = Head.CoordY;
                
                switch (direction)
                {                    
                    case "R":
                        headNewCoordX += 1;
                        break;
                    case "L":
                        headNewCoordX -= 1;
                        break;
                    case "U":
                        headNewCoordY += 1;
                        break;
                    case "D":
                        headNewCoordY -= 1;
                        break;
                }

                Head.ChangePosition(headNewCoordX, headNewCoordY);

                // Console.WriteLine($"Head moved to {Head.CoordX}|{Head.CoordY}");

                // Then move knots all the way to the tail
                for (int knotIndex = 1; knotIndex < Knots.Count; knotIndex++)
                {
                    // Update knot position
                    var currentKnot = Knots[knotIndex];
                    var currentKnotNewCoordX = currentKnot.CoordX;
                    var currentKnotNewCoordY = currentKnot.CoordY;

                    var previousKnot = Knots[knotIndex - 1];
                    var previousKnotCoordX = previousKnot.CoordX;
                    var previousKnotCoordY = previousKnot.CoordY;

                    var differenceX = previousKnotCoordX - currentKnotNewCoordX;
                    var differenceY = previousKnotCoordY - currentKnotNewCoordY;

                    var manhattanDistance = Math.Abs(differenceX) + Math.Abs(differenceY);

                    if (manhattanDistance == 0 || manhattanDistance == 1)
                    {
                        // Implies that current knot and previous knot are adjacent or at the same position. There is no need to move the current knot.
                    }
                    else if (manhattanDistance == 2)
                    {
                        if (differenceX == 2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX - 1;
                        }
                        else if (differenceX == -2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX + 1;
                        }
                        else if (differenceY == 2)
                        {
                            currentKnotNewCoordY = previousKnotCoordY - 1;
                        }
                        else if (differenceY == -2)
                        {
                            currentKnotNewCoordY = previousKnotCoordY + 1;
                        }
                        // An else case implies differenceX == 1 && differenceY == 1. Its valid and not need to update the tail position.
                    }
                    else if (manhattanDistance == 3)
                    {
                        if (differenceX == 2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX - 1;
                            currentKnotNewCoordY = previousKnotCoordY;
                        }
                        else if (differenceX == -2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX + 1;
                            currentKnotNewCoordY = previousKnotCoordY;
                        }
                        else if (differenceY == 2)
                        {
                            currentKnotNewCoordY = previousKnotCoordY - 1;
                            currentKnotNewCoordX = previousKnotCoordX;
                        }
                        else if (differenceY == -2)
                        {
                            currentKnotNewCoordY = previousKnotCoordY + 1;
                            currentKnotNewCoordX = previousKnotCoordX;
                        }
                    }
                    else if (manhattanDistance == 4 && Math.Abs(differenceX) == 2 && Math.Abs(differenceY) == 2) 
                        // Implies at least 3 knots are aligned in diagonal, and the movement must be en diagonal too
                    {
                        if(differenceX == 2 && differenceY == 2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX - 1;
                            currentKnotNewCoordY = previousKnotCoordY - 1;
                        }
                        else if (differenceX == 2 && differenceY == -2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX - 1;
                            currentKnotNewCoordY = previousKnotCoordY + 1;
                        }
                        else if (differenceX == -2 && differenceY == -2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX + 1;
                            currentKnotNewCoordY = previousKnotCoordY + 1;
                        }
                        else if (differenceX == -2 && differenceY == 2)
                        {
                            currentKnotNewCoordX = previousKnotCoordX + 1;
                            currentKnotNewCoordY = previousKnotCoordY - 1;
                        }
                    }

                    else
                    {                        
                        Console.WriteLine("Error. Not valid Manhattan Distance. There is some problem with the logic. Please check. Exiting...");
                        break;
                    }                    

                    currentKnot.ChangePosition(currentKnotNewCoordX, currentKnotNewCoordY);

                    // Console.WriteLine($"Knot {knotIndex} moved to {currentKnot.CoordX}|{currentKnot.CoordY}");
                }

            }
        }
    } 
        public class Knot
        {
            public int CoordX { get; private set; }
            public int CoordY { get; private set; }
            public List<int[]> Track = new List<int[]>(); // Track of all positions the knot has been to.

        public Knot(int coordX = 0, int coordY = 0)
            {
                CoordX = coordX;
                CoordY = coordY;
                Track.Add(new int[] { CoordX, CoordY }); // Add initial position to track
            }

            public void ChangePosition(int coordX , int coordY)
            {
                CoordX = coordX;
                CoordY = coordY;
                Track.Add(new int[] { CoordX, CoordY }); // Add updated position to track
            }
        } 
}
