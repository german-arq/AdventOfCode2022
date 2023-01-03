using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day4;

internal class Day4Solution : Solution
{
    public Day4Solution() : base(4, "Camp Cleanup") { }

    public override void Solve()
    {
        var formatterInput = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var assigmentPairs = formatterInput.Select(pair => pair.Split(",", StringSplitOptions.RemoveEmptyEntries))
                                            .Select(pair => pair.SelectMany(p => p.Trim().Split("-", StringSplitOptions.RemoveEmptyEntries)))
                                            .Select(pair => pair.Select(idSection => int.Parse(idSection)).ToList()).ToList();

        // Part 1  
        var fullyContainmentPairs = assigmentPairs.Where(pair => pair[0] <= pair[2] && pair[1] >= pair[3] // First pair fully contains second pair
                                                                || pair[0] >= pair[2] && pair[1] <= pair[3]) // Second pair fully contains first pair
                                                    .ToList();

        Part1Solution = $"There are {fullyContainmentPairs.Count} fully contained ranges in its respective assigment pair";

        // Part 2
        var overlappingPairs = assigmentPairs.Where(pair => pair[2] >= pair[0] && pair[2] <= pair[1] // Second pair range starts within first range
                                                            || pair[3] >= pair[0] && pair[3] <= pair[1] // Second pair range ends within first range
                                                            || pair[2] <= pair[0] && pair[3] >= pair[1]) // Second pair fully contains first pair
                                            .ToList();

        Part2Solution = $"There are {overlappingPairs.Count} overlapping ranges in its respective assigment pair";
    }
}
