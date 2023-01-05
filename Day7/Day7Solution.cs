using AdventOfCode2022.Abstract;

namespace AdventOfCode2022.Day7;

internal class Day7Solution : Solution
{
    public Day7Solution() : base(7, "No Space Left On Device") { }

    public override void Solve()
    {
        var input = Input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

        var root = new Directory("root");
        var currentDirectory = root;

        foreach (var line in input)
        {
            var lineParts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (lineParts[0] == "dir")
            {
                var directory = new Directory(lineParts[1], currentDirectory);
                currentDirectory.AddSubelement(directory);
            }
            else if (lineParts[0].All(c => Char.IsNumber(c)))
            {
                var file = new File(lineParts[1], int.Parse(lineParts[0]));
                currentDirectory.AddSubelement(file);
            }
            else if (lineParts[1] == "cd")
            {
                if (lineParts[2] == "..")
                {
                    currentDirectory = currentDirectory.Parent;
                }
                else if (lineParts[2] == "/") // Is the root directory
                {
                    continue;
                }
                else
                {
                    currentDirectory = currentDirectory.Subelements.OfType<Directory>().First(d => d.Name == lineParts[2]);
                }
            }
        }

        // Show the tree of the file system
        // Console.WriteLine(root.Show()); 

        // Part 1

        var directoriesWithSizeAtMost1000000 = root.GetAllSubdirectories().Where(d => d.GetTotalSize() <= 100000);   

        Part1Solution = $"The sum of the sizes of all the directories with a total size of at most 100000 is {directoriesWithSizeAtMost1000000.Sum(d => d.GetTotalSize())}.";

        // Part 2

        var totalDiskSpaceAvailable = 70000000;
        var requiredFreeSpace = 30000000;
        
        var totalDiskSpaceOccupied = root.GetTotalSize();

        var spaceToFreeUp = requiredFreeSpace - (totalDiskSpaceAvailable - totalDiskSpaceOccupied);

        var directoryToDelete = root.GetAllSubdirectories().OrderBy(d => d.GetTotalSize()).First(d => d.GetTotalSize() > spaceToFreeUp);

        Part2Solution = $"The directory to delete is {directoryToDelete.Name} with a size of {directoryToDelete.GetTotalSize()}";
    }

    interface FileSystemComponent
    {
        public int GetTotalSize();
        public string Show(int deepLevel = 0);
    }

    class Directory : FileSystemComponent
    {
        public string Name { get; set; }
        public Directory? Parent { get; set; }
        public List<FileSystemComponent> Subelements { get; set; }

        public Directory(string name, Directory? parent = null)
        {
            Name = name;
            Parent = parent;
            Subelements = new List<FileSystemComponent>();
        }
        public void AddSubelement(FileSystemComponent subelement)
        {
            Subelements.Add(subelement);
        }
        
        public List<Directory> GetAllSubdirectories()
        {
            var subdirectories = Subelements.OfType<Directory>().ToList();
            subdirectories.AddRange(Subelements.OfType<Directory>().SelectMany(d => d.GetAllSubdirectories()).ToList());
            return subdirectories;
        }

        public int GetTotalSize()
        {
            return Subelements.Sum(subelement => subelement.GetTotalSize());
        }

        public string Show(int deepLevel = 0)
        {
            var tabs = new string('\t', deepLevel);
            return tabs + "- " + Name + " (dir, total size= " + GetTotalSize() + ")\n" + string.Join("\n", Subelements.Select(subelement => subelement.Show(deepLevel + 1)));
        }

    }

    class File : FileSystemComponent
    {
        public string Name { get; set; }
        public int Size { get; set; }

        public File(string name, int size)
        {
            Name = name;
            Size = size;
        }
        public int GetTotalSize()
        {
            return Size;
        }
        public string Show(int deepLevel = 0)
        {
            var tabs = new string('\t', deepLevel);
            return $"{tabs} - {Name} (file, size={Size})";
        }
    }
}
