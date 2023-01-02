using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022.Day1
{
    internal class Day1Solution
    {
        public static void Run()
        {
            var input = ReadInput();

            var caloriesCarriedByElf = input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
                    .Select(elf => elf.Split("\n", StringSplitOptions.RemoveEmptyEntries))
                    .Select(foodByElf => foodByElf.Select(itemFood => int.Parse(itemFood)).Sum())
                    .ToList();
            
            caloriesCarriedByElf.Sort();

            Console.WriteLine($"Part 1: The Elf carrying the most calories is carrying {caloriesCarriedByElf.Last()} calories. ");
            Console.WriteLine($"Part 2: The top three Elves are carrying {caloriesCarriedByElf.TakeLast(3).Sum()} calories. ");            
        }

        private static string ReadInput(string fileName = "input.txt")
        {
            var path = Path.Combine(Environment.CurrentDirectory, "Day1", fileName);
            Console.WriteLine($"Reading input from {path}");
            return File.ReadAllText(path);
        }        
    }
}
