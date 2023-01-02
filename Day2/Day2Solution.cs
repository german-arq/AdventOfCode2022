using AdventOfCode2022.Abstract;
using System.ComponentModel;

namespace AdventOfCode2022.Day2;

internal class Day2Solution : Solution
{
    Dictionary<string, int> ROUND_SCORE = new() { { "Win", 6 }, { "Loss", 0 }, { "Draw", 3 } };
    Dictionary<string, int> SHAPE_SCORE = new() { { "Rock", 1 }, { "Paper", 2 }, { "Scissors", 3 } };
    Dictionary<string, string> ENCRYPTED_OPPONENT_CHOICE = new() { { "A", "Rock" }, { "B", "Paper" }, { "C", "Scissors" } };
    Dictionary<string, string> ENCRYPTED_OWN_CHOICE = new() { { "X", "Rock" }, { "Y" , "Paper" }, { "Z", "Scissors" } };
    Dictionary<string, string> ENCRYPTED_OUTCOME_NEEDED = new() { { "X", "Loss" }, { "Y", "Draw" }, { "Z", "Win" } };

    public Day2Solution() : base(2, "Rock Paper Scissors") {}    

    public override void Solve()
    {
        var formattedInput = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries) // Format input
                                    .Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries)); // Split each round into an array

        //// Part 1
        var rounds = formattedInput.Select(round => new List<string>() { ENCRYPTED_OPPONENT_CHOICE[round[0]], ENCRYPTED_OWN_CHOICE[round[1]] }) // Decrypt rounds                            
                            .ToList();        
        rounds.ForEach(round => round.Add(RoundResult(round[0], round[1]))); // Add round result
        var totalScore = rounds.Select(round => +SHAPE_SCORE[round[1]] + ROUND_SCORE[round[2]] ).Sum();

        Part1Solution = $"Following the strategy guide, total score is {totalScore}";

        //// Part 2
        var roundsWithRealStrategy = formattedInput.Select(round => new List<string>() { ENCRYPTED_OPPONENT_CHOICE[round[0]], ENCRYPTED_OUTCOME_NEEDED[round[1]] }) // Decrypt rounds                          
                                                        .ToList();
        roundsWithRealStrategy.ForEach(round => round.Insert(1, OwnChoiceAccordingToOutcomeNeeded(round[1], round[0]))); // Add own choice to round
        var totalScoreRealStrategy = roundsWithRealStrategy.Select(round => +SHAPE_SCORE[round[1]] + ROUND_SCORE[round[2]]).Sum(); // Add round result

        Part2Solution = $"Following the real strategy guide, now total score is {totalScoreRealStrategy}";
    }

    private string RoundResult(string opponentChoice, string ownChoice)
    {
        if (opponentChoice == ownChoice)
        {
            return "Draw";
        }
        else if (opponentChoice == "Rock" && ownChoice == "Paper")
        {
            return "Win";
        }
        else if (opponentChoice == "Paper" && ownChoice == "Scissors")
        {
            return "Win";
        }
        else if (opponentChoice == "Scissors" && ownChoice == "Rock")
        {
            return "Win";
        }
        else
        {
            return "Loss";
        }
    }

    private string OwnChoiceAccordingToOutcomeNeeded(string outcomeNeeded, string opponentChoice)
    {
        if (outcomeNeeded == "Win")
        {
            if (opponentChoice == "Rock")
            {
                return "Paper";
            }
            else if (opponentChoice == "Paper")
            {
                return "Scissors";
            }
            else if (opponentChoice == "Scissors")
            {
                return "Rock";
            }
        }
        else if (outcomeNeeded == "Draw")
        {
            return opponentChoice;
        }
        else if (outcomeNeeded == "Loss")
        {
            if (opponentChoice == "Rock")
            {
                return "Scissors";
            }
            else if (opponentChoice == "Paper")
            {
                return "Rock";
            }
            else if (opponentChoice == "Scissors")
            {
                return "Paper";
            }
        }

        return "Invalid";
    }    
}
