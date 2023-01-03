using AdventOfCode2022.Abstract;
using System.Text.RegularExpressions;

namespace AdventOfCode2022.Day5;

internal class Day5Solution : Solution
{
    public Day5Solution() : base(5, "Supply Stacks") { }

    public override void Solve()
    {
        var input = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries).ToList();

        var originalCratesStacks = GetOriginalCratesStacks(input.Take(8).Select(row => row.Chunk(4).Select(c => c[1]).ToList()).ToList());

        var movements = input.Skip(9).Select(text => GetMovement(text)).ToList();

        // Part 1
        var cratesStacksAfterRearrangment = originalCratesStacks.Select(stack => new Stack<char>(stack)).ToList();

        movements.ForEach(movement => cratesStacksAfterRearrangment = ExecuteMovementCrateMover9000(cratesStacksAfterRearrangment, movement));

        var cratesInTopOfEachStack = cratesStacksAfterRearrangment.Select(stack => stack.Pop()).ToList();

        Part1Solution = $"The cranes in top of each stack are in order {string.Join("", cratesInTopOfEachStack)}";

        // Part 2
        var cratesStacksAfterRearragementWithCM9001 = originalCratesStacks.Select(stack => stack.ToList()).ToList();

        movements.ForEach(movement =>
                    cratesStacksAfterRearragementWithCM9001 = ExecuteMovementCrateMover9001(cratesStacksAfterRearragementWithCM9001, movement));

        var cratesInTopOfEachStackWithCM9001 = cratesStacksAfterRearragementWithCM9001.Select(stack => stack[^1]).ToList();

        Part2Solution = $"With the superb CrateMover 9001, " +
            $"now the cranes in top of each stack are in order {string.Join("", cratesInTopOfEachStackWithCM9001)}";
    }

    private static List<List<char>> GetOriginalCratesStacks(List<List<char>> input)
    {
        var numberOfStacks = input[0].Count;
        var cratesStacks = new List<List<char>>();        

        for (int i = 0; i < numberOfStacks; i++)
        {
            var stack = new List<char>();            
            foreach (var row in input)
            {
                if (!Char.IsWhiteSpace(row[i]))
                    stack.Add(row[i]);
            }
            stack.Reverse();
            cratesStacks.Add(stack);
        }
        
        return cratesStacks;
    }

    private static List<int> GetMovement(string movementAsText)
    {
        string pattern = @"(\d+)";
        var matches = Regex.Matches(movementAsText, pattern);
        var movement = new List<int>();

        foreach (Match match in matches)
        {
            movement.Add(int.Parse(match.Value));
        }
        
        return movement;
    }

    private static List<Stack<char>> ExecuteMovementCrateMover9000(List<Stack<char>> cratesStacks, List<int> movement)
    {
        var quantityOfCratesToMove = movement[0];
        var stackFrom = movement[1];
        var stackTo = movement[2];        

        for (int i = 0; i < quantityOfCratesToMove; i++)
        {
            var crateToMove = cratesStacks[stackFrom-1].Pop();
            cratesStacks[stackTo-1].Push(crateToMove);
        }

        return cratesStacks;
    }

    private static List<List<char>> ExecuteMovementCrateMover9001(List<List<char>> cratesStacks, List<int> movement)
    {
        var quantityOfCratesToMove = movement[0];
        var stackFrom = movement[1];
        var stackTo = movement[2];

        var crateToMove = cratesStacks[stackFrom - 1].TakeLast(quantityOfCratesToMove).ToList();

        cratesStacks[stackFrom - 1].RemoveRange(cratesStacks[stackFrom - 1].Count -  quantityOfCratesToMove, quantityOfCratesToMove);
        cratesStacks[stackTo - 1].AddRange(crateToMove);

        return cratesStacks;
    }


}
