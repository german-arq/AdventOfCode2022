using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day1;
internal class Day1Solution : Solution
{
    public Day1Solution() : base(1, "Calorie Counting") { }
        
    public override void Solve()
    {
        var caloriesCarriedByElf = Input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(elf => elf.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                .Select(foodByElf => foodByElf.Select(itemFood => int.Parse(itemFood)).Sum())
                .ToList();
        
        caloriesCarriedByElf.Sort();

        Part1Solution = $"The Elf carrying the most calories is carrying {caloriesCarriedByElf.Last()} calories.";
        Part2Solution = $"Part 2: The top three Elves are carrying {caloriesCarriedByElf.TakeLast(3).Sum()} calories.";            
    }
}
