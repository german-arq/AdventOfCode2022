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
                    default:
                        Console.WriteLine("Invalid day selected! Try Again");
                        break;
                }
                Console.WriteLine(Environment.NewLine);
            }                           
        }
    }
}