using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day6;

internal class Day6Solution : Solution
{
    public Day6Solution() : base(6, "Tuning Trouble") { }

    public override void Solve()
    {
        var input = Input.ToCharArray();

        // Part 1
        for (int i = 3; i < input.Length; i++)
        {
            var lastFourCharacters = input.Skip(i - 3).Take(4);
            
            if (lastFourCharacters.Distinct().Count() == 4)
            {                
                Part1Solution = $"The first start-of-packet marker is at position {i+1}, and it is {String.Join("", lastFourCharacters)}";
                break;
            }                
        }

        // Part 2
        for (int i = 13; i < input.Length; i++)
        {
            var lastFourteenCharacters = input.Skip(i - 13).Take(14);

            if (lastFourteenCharacters.Distinct().Count() == 14)
            {
                Console.WriteLine("This is a Message Marker! " + String.Join("", lastFourteenCharacters));
                Part2Solution = $"The first start-of-message marker is at position {i + 1}, and it is {String.Join("", lastFourteenCharacters)}";
                break;
            }
        }
    }
}
