namespace AOC01
{
    using System;
    using System.Collections.Generic;

    internal class AOC1
    {
        public static void Run()
        {
            long maxCalories = 0;
            List<long> allCalories = new List<long>();

            foreach (string line in File.ReadLines("AOC 1-9/InputFiles/TextFile1.txt"))
            {
                if (line.Length > 0)
                {
                    long currentCalories = long.Parse(line);
                    maxCalories += currentCalories;
                }
                else
                {
                    allCalories.Add(maxCalories);
                    maxCalories = 0;
                }
            }

            allCalories.Sort();
            allCalories.Reverse();

            Console.WriteLine(allCalories[0]);
            Console.WriteLine(allCalories[1]);
            Console.WriteLine(allCalories[2]);
            Console.WriteLine(allCalories[0] + allCalories[1] + allCalories[2]);
        }
    }
}
