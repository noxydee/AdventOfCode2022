namespace AOC01.AOC_10_25
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC13
    {
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/InputFiles/TextFile13.txt");

            foreach (string line in lines)
            {
                List<object> root = new List<object>();

                foreach (char character in line.ToCharArray())
                {
                    List<object> currentList = null;
                    if (character == '[')
                    {
                        if (currentList == null)
                        {
                            currentList = new List<object>();
                        }
                    }
                    else if (character == ']')
                    {
                        List<object> listToAdd = new List<object>();
                        if (currentList != null)
                        {
                            listToAdd.AddRange(currentList);
                        }
                        root.Add(listToAdd);
                        currentList = null;
                    }
                    else if (character == ',')
                    {
                        
                    }
                    else
                    {
                        if (currentList != null)
                        {
                            currentList.Add(character);
                        }
                        else
                        {
                            root.Add(character);
                        }
                    }
                }

                var x = 10;
            }
        }
    }
}
