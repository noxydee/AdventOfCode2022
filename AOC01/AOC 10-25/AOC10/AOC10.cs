namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC10
    {
        private static string NOOP = "noop";
        private static string ADDX = "addx";

        private static int register = 1;
        private static int currentCycle = 0;
        private static int signalStrength = 0;

        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC10/TextFile10.txt");

            foreach (string line in lines)
            {
                if (line.StartsWith(NOOP))
                {
                    IncrementCycle(1);
                }
                else if (line.StartsWith(ADDX))
                {
                    IncrementCycle(2);
                    register += int.Parse(line.Split(" ").Last());
                }
            }
        }

        public static void IncrementCycle(int incrementation)
        {
            for (int i = 0; i < incrementation; i++)
            {
                currentCycle += 1;

                if (Math.Abs(currentCycle % 40 - register-1) < 2)
                {
                    Console.Write("#");
                }
                else 
                {
                    Console.Write(".");
                } 
                
                if (currentCycle % 40 == 0)
                {
                    Console.WriteLine();
                }

                if (currentCycle == 20 || (currentCycle - 20) % 40 == 0)
                {
                    signalStrength += currentCycle* register;
                }
            }
        }
    }
}
