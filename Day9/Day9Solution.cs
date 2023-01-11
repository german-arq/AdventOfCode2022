using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day9;

internal class Day9Solution : Solution
{
    public Day9Solution() : base(9, "Rope Bridge") { }

    public override void Solve()
    {
        var moves = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

        // Part 1
        var ropeHeadPosition = new int[] { 0, 0 }; // Position in { X, Y }
        var ropeTailPosition = new int[] { 0, 0 }; 

        var ropeHeadTrack = new List<int[]>() { ropeHeadPosition };
        var ropeTailTrack = new List<int[]>() { ropeTailPosition };

        foreach (var move in moves)
        {
            var instruction = move.Split(" ");
            var direction = instruction[0];
            var steps = int.Parse(instruction[1]);

            for (int i = 1; i <= steps; i++)
            {
                // Move head
                var headCoordX = ropeHeadPosition[0];
                var headCoordY = ropeHeadPosition[1];
                
                switch (direction)
                {
                    case "R":
                        headCoordX += 1;
                        break;
                    case "L":
                        headCoordX -= 1;
                        break;
                    case "U":
                        headCoordY += 1;
                        break;
                    case "D":
                        headCoordY -= 1;
                        break;
                };

                ropeHeadPosition = new int[] { headCoordX, headCoordY };
                ropeHeadTrack.Add(ropeHeadPosition);


                // Update tail position
                var tailCoordX = ropeTailPosition[0];
                var tailCoordY = ropeTailPosition[1];

                var differenceX = headCoordX - tailCoordX;
                var differenceY = headCoordY - tailCoordY;

                var manhattanDistance = Math.Abs(differenceX) + Math.Abs(differenceY);

                if (manhattanDistance == 0 || manhattanDistance == 1)
                {
                    // Implies that tail and head are adjacent or at the same position. There is no need to move the tail.
                }
                else if (manhattanDistance == 2)
                {
                    if (differenceX == 2)
                    {
                        tailCoordX = headCoordX - 1;
                    }
                    else if (differenceX == -2)
                    {
                        tailCoordX = headCoordX + 1;
                    }
                    else if (differenceY == 2)
                    {
                        tailCoordY = headCoordY - 1;
                    }
                    else if (differenceY == -2)
                    {
                        tailCoordY = headCoordY + 1;
                    }
                    // An else case implies differenceX == 1 && differenceY == 1. Its valid and not need to update the tail position.
                }
                else if (manhattanDistance == 3)
                {
                    if (differenceX == 2)
                    {
                        tailCoordX = headCoordX - 1;
                        tailCoordY = headCoordY;
                    }
                    else if (differenceX == -2)
                    {
                        tailCoordX = headCoordX + 1;
                        tailCoordY = headCoordY;
                    }
                    else if (differenceY == 2)
                    {
                        tailCoordY = headCoordY - 1;
                        tailCoordX = headCoordX;
                    }
                    else if (differenceY == -2)
                    {
                        tailCoordY = headCoordY + 1;
                        tailCoordX = headCoordX;
                    }
                }
                else 
                {
                    Console.WriteLine("Error. Not valid Manhattan Distance. There is some problem with the logic. Please check. Exiting...");
                    break;
                }
                
                ropeTailPosition = new int[] { tailCoordX, tailCoordY };
                ropeTailTrack.Add(ropeTailPosition);
            }   
        }

        Part1Solution = $"The rope tail visited {ropeTailTrack.Select(position => string.Join("|", position)).Distinct().Count()} positions at least once.";


        // Part 2

        

    }

}
