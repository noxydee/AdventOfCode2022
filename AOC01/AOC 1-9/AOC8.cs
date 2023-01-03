namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC8
    { 
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 1-9/InputFiles/TextFile8.txt");
            int[,] map = new int[lines.First().Length, lines.Count()];
            
            for (int x = 0; x < lines.First().Length; x++)
            {
                for (int y = 0; y < lines.Count(); y++)
                {
                    map[x, y] = (int)(lines.ElementAt(y).ElementAt(x)) - 48;
                }
            }

            GetVisibleTrees(map, lines.First().Length, lines.Count());

            int maxScenic = 0;

            for (int x = 1; x < lines.First().Length -1; x++)
            {
                for (int y = 1; y < lines.Count() -1; y++)
                {
                    int newScenic = GetScenicScore(map,x, y, lines.First().Length, lines.Count());
                    maxScenic = Math.Max(maxScenic, newScenic);
                }
            }

            Console.WriteLine(maxScenic);
        }

        public static bool[,] GetVisibleTrees(int[,] map, int xSize, int ySize)
        {
            bool[,] visibleTreesMap = new bool[xSize, ySize];
            int maxInRow = 0;

            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    if (x == 0 || y == 0 || x == xSize - 1 || y == ySize - 1)
                    {
                        visibleTreesMap[x, y] = true;
                    }
                }
            }

            
            for (int y = 1; y < ySize; y++)
            {
                maxInRow = map[0, y];
                for (int x = 1; x < xSize; x++)
                {
                    if (map[x, y] > maxInRow)
                    {
                        visibleTreesMap[x, y] = true;
                        maxInRow = map[x, y] > maxInRow ? map[x, y] : maxInRow;
                    }
                }
            }

            for (int x = 1; x < xSize; x++)
            {
                maxInRow = map[x, 0];
                for (int y = 1; y < ySize; y++)
                {
                    if (map[x, y] > maxInRow)
                    {
                        visibleTreesMap[x, y] = true;
                        maxInRow = map[x, y] > maxInRow ? map[x, y] : maxInRow;
                    }
                }
            }

            for (int x = xSize-1; x >= 0; x--)
            {
                maxInRow = map[x, ySize-1];
                for (int y = ySize -1; y >= 0; y--)
                {
                    if (map[x, y] > maxInRow)
                    {
                        visibleTreesMap[x, y] = true;
                        maxInRow = map[x, y] > maxInRow ? map[x, y] : maxInRow;
                    }
                }
            }

            for (int y = ySize-1; y >= 0; y--)
            {
                maxInRow = map[xSize-1, y];
                for (int x = xSize-1; x >= 0; x--)
                {
                    if (map[x, y] > maxInRow)
                    {
                        visibleTreesMap[x, y] = true;
                        maxInRow = map[x, y] > maxInRow ? map[x, y] : maxInRow;
                    }
                }
            }



            int counter = 0;

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    if (visibleTreesMap[x, y])
                    {
                        counter++;
                    }

                    if (visibleTreesMap[x, y])
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    Console.Write($"{map[x,y]}");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }

            Console.WriteLine(counter);

            return visibleTreesMap;
        }

        public static int GetScenicScore(int[,] map, int xPos, int yPos, int xSize, int ySize)
        {
            int seenTop = 0, seenBottom = 0, seenLeft = 0, seenRight = 0;

            for (int i = xPos-1; i >= 0; i--)
            {
                if (map[i, yPos] >= map[xPos, yPos])
                {
                    seenLeft++;
                    break;
                }
                else
                {
                    seenLeft++;
                }
            }

            for (int i = xPos + 1; i < xSize; i++)
            {
                if (map[i, yPos] >= map[xPos, yPos])
                {
                    seenRight++;
                    break;
                }
                else
                {
                    seenRight++;
                }
            }

            for (int i = yPos - 1; i >= 0; i--)
            {
                if (map[xPos, i] >= map[xPos, yPos])
                {
                    seenTop++;
                    break;
                }
                else
                {
                    seenTop++;
                }
            }

            for (int i = yPos + 1; i < ySize; i++)
            {
                if (map[xPos, i] >= map[xPos, yPos])
                {
                    seenBottom++;
                    break;
                }
                else
                {
                    seenBottom++;
                }
            }

            return seenBottom * seenTop * seenLeft * seenRight;
        }
    }

    public class TreeCell
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool isVisible { get; set; }
        public bool hasBeenReaden { get; set; }

        public TreeCell(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.isVisible = false;
            this.hasBeenReaden = false;
        }
    }
}
