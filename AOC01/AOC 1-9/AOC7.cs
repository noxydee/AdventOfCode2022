namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC7
    {
        public static Directory RootDirectory { get; set; }
        public static Directory CurrentDirectory { get; set; }

        public static Directory DirectoryToRemove { get; set; }

        public static long TOTAL_SIZE = 70000000;
        public static long REQUIRED_SIZE = 30000000;
        public static long spaceToRemove = 0;

        public static void Run()
        {
            foreach (string line in File.ReadLines("AOC 1-9/InputFiles/TextFile7.txt"))
            {
                if (line.StartsWith("$"))
                {
                    if (line.Contains("cd .."))
                    {
                        CurrentDirectory = CurrentDirectory.Parent;
                    }
                    else if (line.Contains("cd"))
                    {
                        string nextDirectory = line.Split(" ").Last();
                        if (CurrentDirectory != null)
                        {
                            CurrentDirectory = CurrentDirectory.Directories.FirstOrDefault(x => x.Name.Equals(nextDirectory))
                            ?? throw new Exception("Directory not found");
                        }
                        else
                        {
                            RootDirectory = new Directory(nextDirectory, null);
                            CurrentDirectory = RootDirectory;
                        }
                    }
                }
                else
                {
                    if (line.StartsWith("dir"))
                    {
                        string newDirectoryFound = line.Split(" ").Last();
                        CurrentDirectory.Directories.Add(new Directory(newDirectoryFound, CurrentDirectory));
                    }
                    else
                    {
                        string[] newFileParams = line.Split(" ");
                        AOCFile newFile = new AOCFile(newFileParams[1], long.Parse(newFileParams[0]));
                        CurrentDirectory.Files.Add(newFile);
                    }
                }
            }
            long currentFreeSpace = TOTAL_SIZE - RootDirectory.GetSize();
            spaceToRemove = REQUIRED_SIZE - currentFreeSpace;

            DirectoryToRemove = RootDirectory;
            SetDirectoryToRemove();

            Console.WriteLine(DirectoryToRemove.GetSize());
        }

        public static void SetDirectoryToRemove()
        {
            List<Directory> queue = new List<Directory>();
            queue.Add(RootDirectory);

            while(queue.Count > 0)
            {
                Directory currentDirectory = queue.Last();
                if (currentDirectory.GetSize() > spaceToRemove)
                {
                    foreach (Directory childDirectory in currentDirectory.Directories)
                    {
                        if (childDirectory.GetSize() > spaceToRemove)
                        {
                            queue.Add(childDirectory);
                        }
                    }

                    if (currentDirectory.GetSize() - spaceToRemove < DirectoryToRemove.GetSize() - spaceToRemove)
                    {
                        DirectoryToRemove = currentDirectory;
                    }
                }

                queue.Remove(currentDirectory);
            }
        }

        public static long GetSmallDirectorySizes(Directory directory)
        {
            if (directory.GetSize() < 100000)
            {
                return directory.GetSize() + directory.Directories.Sum(x => GetSmallDirectorySizes(x));
            }
            else
            {
                return directory.Directories.Sum(x => GetSmallDirectorySizes(x));
            }
        }
    }

    public class Directory
    {
        public string Name { get; set; }
        public Directory Parent { get; set; }
        public List<Directory> Directories { get; set; }
        public List<AOCFile> Files { get; set; }

        public Directory(string name, Directory parent)
        {
            Name = name;
            Parent = parent;
            Directories = new List<Directory>();
            Files = new List<AOCFile>();
        }

        public long GetSize()
        {
            return Files.Sum(x => x.Size) + Directories.Sum(x => x.GetSize());
        }
    }

    public class AOCFile
    {
        public string Name { get; private set; }
        public long Size { get; private set; }

        public AOCFile(string name, long size)
        {
            Name = name;
            Size = size;
        }
    }
}
