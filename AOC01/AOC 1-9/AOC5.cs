namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal static class AOC5
    {
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 1-9/InputFiles/TextFile5.txt");
            Stack<Crate>[] stackOfCrates = new Stack<Crate>[SplitBy(lines.ElementAt(0), 4).Count()];

            for (int i = 0; i < stackOfCrates.Length; i++)
            {
                stackOfCrates[i] = new Stack<Crate>();
            }

            foreach (string line in lines)
            {
                if(line.Contains("["))
                {
                    string[] level = SplitBy(line, 4).ToArray();

                    for (int i = 0; i < level.Length; i++)
                    {
                        if (level[i].Length > 0)
                        {
                            stackOfCrates[i].Push(new Crate(level[i]));
                        }
                    }
                    
                }

                if (line.All(x => x.Equals(' ')))
                {
                    for (int i = 0; i < stackOfCrates.Length; i++) 
                    {
                        Stack<Crate> newStack = new();
                        while (stackOfCrates[i].Count != 0)
                        {
                            newStack.Push(stackOfCrates[i].Pop());
                        }
                        stackOfCrates[i] = newStack;
                    }
                }

                if (line.Contains("move"))
                {
                    int[] values = Regex.Matches(line, @"\d+").OfType<Match>().Select(m => int.Parse(m.Value)).ToArray();
                    //stackOfCrates.PerformMoveOneByOne(values[0], values[1], values[2]);
                    stackOfCrates.PerformMoveBulk(values[0], values[1], values[2]);
                }

                
            }

            foreach (Stack<Crate> stack in stackOfCrates)
            {
                if (stack.Count > 0)
                {
                    Console.Write(stack.Peek().Symbol);
                }
            }
        }

        public static void PerformMoveOneByOne(this Stack<Crate>[] stacks, int count, int from, int to)
        {
            for (int i = 0; i < count; i++)
            {
                Crate crateToMove = stacks.ElementAt(from - 1).Pop();
                stacks.ElementAt(to - 1).Push(crateToMove);
            }
        }

        public static void PerformMoveBulk(this Stack<Crate>[] stacks, int count, int from, int to)
        {
            List<Crate> cratesToMove = new List<Crate>();
            for (int i = 0; i < count; i++)
            {
                cratesToMove.Add(stacks.ElementAt(from - 1).Pop());
            }

            cratesToMove.Reverse();
            foreach (Crate crate in cratesToMove)
            {
                stacks.ElementAt(to - 1).Push(crate);
            }
        }

        public static IEnumerable<string> SplitBy(string str, int chunkLength)
        {
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                yield return str.Substring(i, chunkLength).Trim();
            }
        }
    }

    public class Crate 
    {
        public string Symbol { get; set; }

        public Crate(string symbol)
        {
            Symbol = symbol;
        }
    }
}
