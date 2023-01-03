using AdventOfCode2022.Abstract;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AdventOfCode2022.Day3;

internal class Day3Solution : Solution
{
    public Day3Solution() : base(3, "Rucksack Reorganization") { }

    public override void Solve()
    {
        // Format input
        var formattedInput = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        // Part 1
        var rucksacksByCompartments = formattedInput.Select(rucksack =>
                                        new List<string>()
                                            { rucksack[..(rucksack.Length / 2)],
                                            rucksack[(rucksack.Length / 2)..] })
                                        .ToList();
        var repeatedItems = rucksacksByCompartments.Select(rucksack => rucksack[0].ToCharArray().Intersect(rucksack[1].ToCharArray()).ToList()[0]).ToList();
        var priorityRepeatedItems = repeatedItems.Sum(repeatedItems => GetItemPriority(repeatedItems));

        Part1Solution = $"The sum of priorities of the repeated items is {priorityRepeatedItems}.";

        // Part 2
        var rucksacksGroups = formattedInput.Chunk(3).ToList();
        var badgeGroups = rucksacksGroups.Select(rucksack => 
                                            rucksack[0].ToCharArray()
                                            .Intersect(rucksack[1].ToCharArray())
                                            .Intersect(rucksack[2].ToCharArray())
                                            .ToList()[0])
                                        .ToList();
        var priorityBadgeGroups = badgeGroups.Sum(badge => GetItemPriority(badge));

        Part2Solution = $"The sum of priorities of the badges of each three-Elf group is {priorityBadgeGroups}";        
    }

    public int GetItemPriority(char item)
    {
        if (Char.IsLower(item))
        {
            return item - 96;
        }
        else if (Char.IsUpper(item))
        {
            return item - 38;
        }
        else
        {
            throw new InvalidOperationException("Accepts letters only");
        }
    }
}
