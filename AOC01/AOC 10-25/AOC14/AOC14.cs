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
        public static void Run()
        {
            Bitmap bitmap = new Bitmap(1000, 1000);
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC14/TextFile14.txt");

            foreach (string line in lines)
            {
                List<string> cordinates = line.Split("->").ToList();

                for (int i = 0; i < cordinates.Count-1; i++)
                {
                    int startX = int.Parse(cordinates[i].Split(",")[0]) - 400;
                    int startY = int.Parse(cordinates[i].Split(",")[1]) - 400;
                    int endX = int.Parse(cordinates[i+1].Split(",")[0]) - 400;
                    int endY = int.Parse(cordinates[i+1].Split(",")[1]) - 400;

                    Point start = new Point(startX, startY);
                    Point end = new Point(endX, endY);

                    Draw(bitmap, start, end);
                }
                
            }

            bitmap.Save("map.bmp", ImageFormat.Bmp);
        }

        public static void Draw(Bitmap bitmap, Point start, Point end)
        {
            List<Point> line = new List<Point>();
            line.Add(start);

            if (start.X == end.X)
            {
                for (int y = start.Y; y < Math.Abs(end.Y - start.Y); y++)
                {
                    line.Add(new Point(start.X, y));
                }
            }
            else if (start.Y == end.Y)
            {
                for (int x = start.X; x < Math.Abs(end.X - start.X); x++)
                {
                    line.Add(new Point(x, start.Y));
                }
            }
            else
            {
                throw new Exception("Points are not connected");
            }
            line.Add(end);

            foreach (Point point in line)
            {
                bitmap.SetPixel(point.X, point.Y, Color.Red);
            }
        }
    }
}
