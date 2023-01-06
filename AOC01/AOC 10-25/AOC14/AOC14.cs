namespace AOC01.AOC_10_25.AOC14
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC14
    {
        private static Point SandHole = new Point(500, 0);
        private static Color BackgroundColor = Color.White;
        private static Color RockColor = Color.DarkGray;
        private static Color HoleColor = Color.DeepPink;
        private static Color SandColor = Color.Blue;

        private static int HighestPoint = 0;

        public static void Run()
        {
            Bitmap bitmap = GetBitmap();
            int sandUnits = 0;

            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC14/TextFile14.txt");
            InputMap(bitmap, lines.ToList());
            InputMap(bitmap, new List<string> { $"0,{HighestPoint + 2} -> 999,{HighestPoint + 2}" });

            while(true)
            {
                try
                {
                    DropSandUnit(bitmap);
                    sandUnits++;
                }
                catch (Exception ignore)
                {
                    Console.WriteLine(sandUnits);
                    break;
                }
                
            }

            bitmap.Save("AOC14.bmp", ImageFormat.Bmp);
        }

        private static void DropSandUnit(Bitmap bitmap)
        {
            Point sandUnit = new Point(SandHole.X, SandHole.Y);
            if (bitmap.GetPixel(sandUnit.X, sandUnit.Y).ToArgb() == SandColor.ToArgb())
            {
                throw new Exception("Top");
            }

            bool isSandUnitPlaces = false;

            while (!isSandUnitPlaces)
            {
                if (bitmap.GetPixel(sandUnit.X, sandUnit.Y + 1).ToArgb() == BackgroundColor.ToArgb())
                {
                    sandUnit.Y += 1;
                }
                else
                {
                    if (bitmap.GetPixel(sandUnit.X - 1, sandUnit.Y + 1).ToArgb() == BackgroundColor.ToArgb())
                    {
                        sandUnit.X -= 1;
                        sandUnit.Y += 1;
                    }
                    else if (bitmap.GetPixel(sandUnit.X + 1, sandUnit.Y + 1).ToArgb() == BackgroundColor.ToArgb())
                    {
                        sandUnit.X += 1;
                        sandUnit.Y += 1;
                    }
                    else
                    {
                        bitmap.SetPixel(sandUnit.X, sandUnit.Y, SandColor);
                        isSandUnitPlaces = true;
                    }
                }
            }
        }

        private static void InputMap(Bitmap bitmap, List<string> lines)
        {
            foreach (string line in lines)
            {
                List<string> cordinates = line.Split("->").ToList();

                for (int i = 0; i < cordinates.Count - 1; i++)
                {
                    int startX = int.Parse(cordinates[i].Split(",")[0]);
                    int startY = int.Parse(cordinates[i].Split(",")[1]);
                    int endX = int.Parse(cordinates[i + 1].Split(",")[0]);
                    int endY = int.Parse(cordinates[i + 1].Split(",")[1]);

                    HighestPoint = Math.Max(Math.Max(HighestPoint, startY), endY);

                    Point start = new Point(startX, startY);
                    Point end = new Point(endX, endY);

                    Draw(bitmap, start, end);
                }
            }
        }

        private static Bitmap GetBitmap()
        {
            Bitmap bitmap = new Bitmap(1000, 200);

            for (int x = 0; x < 1000; x++)
            {
                for (int y = 0; y < 200; y++)
                {
                    bitmap.SetPixel(x, y, BackgroundColor);
                }
            }
            bitmap.SetPixel(SandHole.X, SandHole.Y, HoleColor);

            return bitmap;
        }

        private static void Draw(Bitmap bitmap, Point start, Point end)
        {
            List<Point> line = new List<Point>();
            line.Add(start);

            if (start.X == end.X)
            {
                IEnumerable<int> newXCordinates = Enumerable.Range(Math.Min(start.Y, end.Y), Math.Abs(end.Y-start.Y));
                newXCordinates.ToList().ForEach(y => line.Add(new Point(start.X, y)));
            }
            else if (start.Y == end.Y)
            {
                IEnumerable<int> newYCordinates = Enumerable.Range(Math.Min(start.X, end.X), Math.Abs(end.X - start.X));
                newYCordinates.ToList().ForEach(x => line.Add(new Point(x, start.Y)));
            }
            else
            {
                throw new Exception("Points are not connected");
            }
            line.Add(end);

            foreach (Point point in line)
            {
                bitmap.SetPixel(point.X, point.Y, RockColor);
            }
        }
    }
}
