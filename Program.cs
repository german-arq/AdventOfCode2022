namespace AdventOfCode2022
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Advent of Code 2022");
            while (true)
            {
                Console.WriteLine("Select a day to run the solution: ");
                var selectedDay = Console.ReadLine();

                switch (selectedDay)
                {
                    case "1":
                        Day1.Day1Solution day1Solution = new();
                        day1Solution.ShowSolution();
                        break;
                    case "2":
                        Day2.Day2Solution day2Solution = new();
                        day2Solution.ShowSolution();
                        break;
                    case "3":
                        Day3.Day3Solution day3Solution = new();
                        day3Solution.ShowSolution();
                        break;
                    case "4":
                        Day4.Day4Solution day4Solution = new();
                        day4Solution.ShowSolution();
                        break;
                    case "5":
                        Day5.Day5Solution day5Solution = new();
                        day5Solution.ShowSolution();
                        break;
                    case "6":
                        Day6.Day6Solution day6Solution = new();
                        day6Solution.ShowSolution();
                        break;
                    default:
                        Console.WriteLine("Invalid day selected! Try Again");
                        break;
                }
                Console.WriteLine(Environment.NewLine);
            }                           
        }
    }
}