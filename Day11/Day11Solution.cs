using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day11;

internal class Day11Solution : Solution
{
    public Day11Solution() : base(11, "Monkey in the Middle", "input.txt") { }

    public override void Solve()
    {
        // Prepare input data
        var notes = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries).Chunk(6).ToList();

        // Create a list of monkeys according to the input data
        var monkeysForFewRounds = new List<Monkey>();
        var monkeysForALotOfRounds = new List<Monkey>();
        
        // I couldn't solve Part 2 until I went to Reddit to understand what I was supposed to do to lower the worry levels
        long moduloForPart2 = 1;

        foreach (var monkeyNote in notes)
        {
            int id = int.Parse(monkeyNote[0].Trim().Split(" ")[^1].Replace(":", ""));

            List<long> startingItems = new(monkeyNote[1].Trim().Split(":")[^1].Trim().Split(", ").Select(long.Parse));

            // Get Operation function
            var instruction = monkeyNote[2].Trim().Split("old", 2)[^1].Trim().Split(" ");
            var mathSymbol = instruction[0];
            var number = int.TryParse(instruction[1], out var n) ? n : 0;
            
            Func<long, long> operation = mathSymbol switch
            {
                "+" when number > 0 => x => x + number,
                "+" when number == 0 => x => x + x,
                "*" when number > 0 => x => x * number,
                "*" when number == 0 => x => x * x,
                _ => x => x
            };

            // Get test function
            var divisor = int.Parse(monkeyNote[3].Trim().Split(" ")[^1]);
            Func<long, bool> test = x => x % divisor == 0;

            moduloForPart2 *= divisor; // Thanks to Reddit for this one

            // Get monkeys to throw items to
            var throwToTrueCase = int.Parse(monkeyNote[4].Trim().Split(" ")[^1]);
            var throwToFalseCase = int.Parse(monkeyNote[5].Trim().Split(" ")[^1]);

            Dictionary<bool, int> monkeysToThrowItemsTo = new() { { true, throwToTrueCase }, { false, throwToFalseCase} };

            monkeysForFewRounds.Add(new Monkey(id, startingItems, operation, test, monkeysToThrowItemsTo));
            monkeysForALotOfRounds.Add(new Monkey(id, startingItems, operation, test, monkeysToThrowItemsTo));
        }

        // Part 1
        var numberOfRounds = 20;
        Func<long, long> firstWorryDecreaseStrategy = x => (int)Math.Floor((x * 1.0) / 3);

        for (int round = 1; round <= numberOfRounds; round++)
        {
            foreach (var monkey in monkeysForFewRounds)
            {
                while (monkey.ItemsWorryLevels.Count > 0)
                {
                    var (item, monkeyId) = monkey.InspectAndThrowItem(firstWorryDecreaseStrategy);
                    var catchingMonkey = monkeysForFewRounds.First(m => m.Id == monkeyId);
                    catchingMonkey.CatchItem(item);
                }
            }
            continue;
        }

        var mostActiveMonkeys = monkeysForFewRounds.OrderByDescending(monkey => monkey.InspectedItems).ToList();

        var monkeyBusinessLevel = mostActiveMonkeys[0].InspectedItems * mostActiveMonkeys[1].InspectedItems;

        Part1Solution = $"The level of monkey business after {numberOfRounds} rounds is {monkeyBusinessLevel}";

        // Part 2
        var numberOfRoundsLongWait = 10000;
        Func<long, long> secondWorryDecreaseStrategy = x => x % moduloForPart2; // Thanks to Reddit for this one

        for (int round = 1; round <= numberOfRoundsLongWait; round++)
        {
            foreach (var monkey in monkeysForALotOfRounds)
            {
                while (monkey.ItemsWorryLevels.Count > 0)
                {
                    var (item, monkeyId) = monkey.InspectAndThrowItem(secondWorryDecreaseStrategy);
                    var catchingMonkey = monkeysForALotOfRounds.First(m => m.Id == monkeyId);
                    catchingMonkey.CatchItem(item);
                }
            }            
        }

        var mostActiveMonkeysAfterLongWait = monkeysForALotOfRounds.OrderByDescending(monkey => monkey.InspectedItems).ToList();

        long monkeyBusinessLevelAfterLongWait = mostActiveMonkeysAfterLongWait[0].InspectedItems * (long)mostActiveMonkeysAfterLongWait[1].InspectedItems;

        Part2Solution = $"The level of monkey business after {numberOfRoundsLongWait} rounds is {monkeyBusinessLevelAfterLongWait}, resulting product of {mostActiveMonkeysAfterLongWait[0].InspectedItems} and {mostActiveMonkeysAfterLongWait[1].InspectedItems}";

    }

    class Monkey
    {
        public int Id { get; init; }
        public Queue<long> ItemsWorryLevels { get; private set; } = new();
        Func<long, long> Operation;
        Func<long, bool> Test;
        Dictionary<bool, int> MonkeysToThrowItemsTo;
        public int InspectedItems { get; private set; }
        
        public Monkey(int id, List<long> items, Func<long, long> operation, Func<long, bool> test, Dictionary<bool, int> monkeysToThrow)
        {
            Id = id;
            items.ForEach(i => CatchItem(i));
            Operation = operation;
            Test = test;
            MonkeysToThrowItemsTo = monkeysToThrow;
        }
        
        public (long item, int monkeyId) InspectAndThrowItem(Func<long, long> worryDecreaseStrategy)
        {
            var item = ItemsWorryLevels.Dequeue();
            
            InspectedItems++;
            
            item = Operation(item);

            // Monkey gets bored and your worry level decreases            
            item = worryDecreaseStrategy(item);
            
            var testResult = Test(item);

            return (item, MonkeysToThrowItemsTo.GetValueOrDefault(testResult, Id));
        }
        
        public void CatchItem(long item)
        {
            ItemsWorryLevels.Enqueue(item);            
        }
    }  
}