namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC6
    {
        public static int REP_COUNT = 14;

        public static void Run()
        {
            foreach (string line in File.ReadLines("AOC 1-9/InputFiles/TextFile6.txt"))
            {
                for (int i = 0; i < line.Length - REP_COUNT; i++)
                {
                    List<char> marker = new List<char>();
                    marker.AddRange(line.Substring(i, REP_COUNT));
                    List<char> distinctChars = marker.Distinct().ToList();

                    if(distinctChars.Count == REP_COUNT) 
                    {
                        Console.WriteLine(i + REP_COUNT);
                        break;
                    }
                }
            }
        }
    }
}
