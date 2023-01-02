namespace AdventOfCode2022.Abstract;

internal abstract class Solution
{
    public int Day { get; set; }
    public string Name { get; set; }
    public string Input { get; set; }
    public string Part1Solution { get; set; }
    public string Part2Solution { get; set; }

    public Solution(int day, string name, string inputFilename = "input.txt")
    {
        Day = day;
        Name = name;
        Input = ReadInput(inputFilename);
    }

    public abstract void Solve();

    public string ReadInput(string fileName)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "Day" + Day.ToString(), fileName);
        return File.ReadAllText(path);
    }

    public void ShowSolution()
    {
        Solve();
        Console.WriteLine($"Solution for Day {Day} - {Name}");
        Console.WriteLine("===================================");
        Console.WriteLine($"Part 1: {Part1Solution}");
        Console.WriteLine($"Part 2: {Part2Solution}");
    }
}
