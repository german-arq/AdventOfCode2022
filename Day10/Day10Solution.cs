using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day10;

internal class Day10Solution : Solution
{
    public Day10Solution() : base(9, "Cathode-Ray Tube") { }

    public override void Solve()
    {
        var signal = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        // Part 1
        var interestingSignalCycles = new List<int>() { 20, 60, 100, 140, 180, 220};
        var cycle = 0;        
        var X = 1;
        var register = new List<int>() { X };        

        foreach (var i in signal)
        {
            var instruction = i.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (instruction[0] == "noop")
            {
                cycle++;
                register.Add(X);
            }

            else if (instruction[0] == "addx")
            {
                cycle++;
                register.Add(X);

                var value = int.Parse(instruction[1]);
                X += value;

                cycle++;

                register.Add(X);
            }

            else
            {
                Console.WriteLine("Instruction not valid");
                break;
            }
        }

        var interestingSignalsTotalStrength = register.Select((value, index) => new { value, index })
                                                        .Where(r => interestingSignalCycles.Contains(r.index + 1))
                                                        .Sum(r => r.value * (r.index + 1));

        Part1Solution = $"Sum of six interesting signal strengths: {interestingSignalsTotalStrength}";

            

        // Part 2
        var sprite = "###";

        var crtPixelsWide = 40;

        var crtScreen = new List<string>();

        for (int i = 0; i < register.Count - 1; i++)
        {
            var pixelPosition = i % crtPixelsWide;
            var spritePosition = new List<int>() { register[i] - 1, register[i], register[i] + 1 };

            // Console.WriteLine($"CRT draws pixel in position {pixelPosition}, and stripe position is {string.Join("|", spritePosition)}");

            if (spritePosition.Contains(pixelPosition))
            {
                crtScreen.Add("#");
            }
            else
            {
                crtScreen.Add(".");
            }
        }

        Console.WriteLine("Screen: \n");
        crtScreen.Chunk(crtPixelsWide).ToList().ForEach(c => Console.WriteLine(string.Join("", c)));
        Console.WriteLine("\n");

        Part2Solution = "See above";


    }    
}


