using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day8;

internal class Day8Solution : Solution
{
    public Day8Solution() : base(8, "Treetop Tree House") { }

    public override void Solve()
    {
        var treesGrid = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
        var transposeTreesGrid = treesGrid.First().Select((_, i) => string.Join("", treesGrid.Select(x => x[i]).ToArray())).ToArray();
        
        // Part 1
        var numberOfVisibleTrees = 0; // From outside the grid

        for (int i = 0; i < treesGrid.Length; i++)
        {
            var row = treesGrid[i];

            for (int j = 0; j < row.Length; j++)
            {
                // Trees in the edge of the grid
                if (i == 0 || i == row.Length - 1 || j == 0 || j == row.Length - 1)
                {
                    numberOfVisibleTrees++;
                    continue;
                }

                var heightCurrentTree = treesGrid[i][j];

                var treesToUp = transposeTreesGrid[j].Take(i).ToArray();
                var treesToDown = transposeTreesGrid[j].Skip(i + 1).ToArray();
                var treesToLeft = row.Take(j).ToArray();
                var treesToRight = row.Skip(j + 1).ToArray();

                // Is visible from the outside of the grid
                if (treesToUp.All(t => t < heightCurrentTree) || treesToDown.All(t => t < heightCurrentTree) 
                    || treesToLeft.All(t => t < heightCurrentTree) || treesToRight.All(t => t < heightCurrentTree))
                {
                    numberOfVisibleTrees++;                    
                    continue;
                }
            }
        }

        Part1Solution = $"There are {numberOfVisibleTrees} trees visible from the outside of the grid";

        // Part 2

        var highestTreeScenicScore = 0;

        for (int i = 0; i < treesGrid.Length; i++)
        {
            var row = treesGrid[i];

            for (int j = 0; j < row.Length; j++)
            {
                var heightCurrentTree = treesGrid[i][j];

                var treesToUp = transposeTreesGrid[j].Take(i).Reverse().ToArray();
                var treesToDown = transposeTreesGrid[j].Skip(i + 1).ToArray();
                var treesToLeft = row.Take(j).Reverse().ToArray();
                var treesToRight = row.Skip(j + 1).ToArray();

                var treesViewedToUp = treesToUp.ToList().FindIndex(t => t >= heightCurrentTree);
                treesViewedToUp = treesViewedToUp == -1 ? treesToUp.Length : treesViewedToUp + 1; 
                // If there is no tree higher than the current tree or the tree is in the edge, the score is the number of trees in the direction

                var treesViewedToDown = treesToDown.ToList().FindIndex(t => t >= heightCurrentTree);
                treesViewedToDown = treesViewedToDown == -1 ? treesToDown.Length : treesViewedToDown + 1;

                var treesViewedToLeft = treesToLeft.ToList().FindIndex(t => t >= heightCurrentTree);
                treesViewedToLeft = treesViewedToLeft == -1 ? treesToLeft.Length : treesViewedToLeft + 1;

                var treesViewedToRigth = treesToRight.ToList().FindIndex(t => t >= heightCurrentTree);
                treesViewedToRigth = treesViewedToRigth == -1 ? treesToRight.Length : treesViewedToRigth + 1;

                var treeScenicScore = treesViewedToUp * treesViewedToDown * treesViewedToLeft * treesViewedToRigth;

                if (treeScenicScore > highestTreeScenicScore)
                {
                    highestTreeScenicScore = treeScenicScore;
                }
            }
        }

        Part2Solution = $"The highest scenic score possible for any tree is {highestTreeScenicScore}";
    }
}
