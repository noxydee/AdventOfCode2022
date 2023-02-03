namespace AOC01.AOC_10_25.AOC17
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC17
    {
        public static Color BackgroundColor = Color.FromArgb(20,20,20);
        public static int MaxDepth = 50;

        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC17/TextFile17_2.txt");
            List<Rock> rocks = new List<Rock>();
            rocks.Add(new Rock(RockType.minus));
            rocks.Add(new Rock(RockType.plus));
            rocks.Add(new Rock(RockType.lshape));
            rocks.Add(new Rock(RockType.line));
            rocks.Add(new Rock(RockType.cube));

            Bitmap bmp = new Bitmap(7, MaxDepth);
            for (int x = 0; x < 7; x++)
            {
                for(int y=0;y<MaxDepth;y++)
                {
                    bmp.SetPixel(x, y, BackgroundColor);
                }
            }


            int elementCount = 200;

            for(int i = 0;i < elementCount; i++)
            {
                Rock currentRock = CreateDeepCopy<Rock>(rocks.ElementAt(i % 5));
                Console.WriteLine($"Starting x={currentRock.Coordinates.First().X} y={currentRock.Coordinates.First().Y}");

                foreach (char character in lines.First().ToCharArray())
                {
                    switch (character) 
                    {
                        case '>':
                            currentRock.PushDown(bmp);
                            break;
                        case '<':
                            currentRock.PushLeft(bmp);
                            break;
                        default:
                            throw new Exception($"Direction {character} not handeled");
                    }

                    if (!currentRock.PushDown(bmp))
                    {
                        currentRock.Draw(bmp);
                        break;
                    }
                }
            }

            bmp.Save("AOC17Result.bmp");
            Console.WriteLine(lines.First());
        }

        public static T CreateDeepCopy<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(ms);
            }
        }
    }

    [Serializable]
    public class Rock
    {
        public List<Point> Coordinates { get; set; }
        public Color Color { get; set; }

        public Rock(RockType type)
        {
            Coordinates = GetPoints(type);
            Color = Color.Red;
        }

        public void Draw(Bitmap bitmap)
        {
            foreach (Point point in Coordinates)
            {
                Console.WriteLine($"Coloring x={point.X} y={point.Y}");
                bitmap.SetPixel(point.X, point.Y, Color);
            }
        }

        public bool PushRight(Bitmap bitmap)
        {
            if (CanGoInDirection(bitmap, Direction.Right))
            {
                foreach (Point point in Coordinates)
                {
                    point.X++;
                }
            }

            return true;
        }

        public bool PushLeft(Bitmap bitmap)
        {
            if (CanGoInDirection(bitmap, Direction.Left))
            {
                foreach (Point point in Coordinates)
                {
                    point.X--;
                }
            }

            return true;
        }

        public bool PushDown(Bitmap bitmap)
        {
            if (CanGoInDirection(bitmap, Direction.Down))
            {
                foreach (Point point in Coordinates)
                {
                    point.Y++;
                }

                return true;
            }

            return false;
        }

        private bool CanGoInDirection(Bitmap bitmap, Direction direction)
        {
            foreach (Point point in Coordinates)
            {
                switch(direction)
                {
                    case Direction.Down:
                        if (point.Y+1>=AOC17.MaxDepth || bitmap.GetPixel(point.X, point.Y + 1) == Color)
                        {
                            return false;
                        }
                        break;
                    case Direction.Left:
                        if (point.X-1 < 0 || bitmap.GetPixel(point.X-1, point.Y) == Color)
                        {
                            return false;
                        }
                        break;
                    case Direction.Right:
                        if (point.X+1 > 7 || bitmap.GetPixel(point.X+1, point.Y) == Color)
                        {
                            return false;
                        }
                        break;
                    default:
                        throw new Exception("Direction not handeled");
                }
                
            }
            return true;
        }

        private List<Point> GetPoints(RockType type)
        {
            List<Point> points = new List<Point>();

            switch (type)
            {
                case RockType.minus:
                    points.Add(new Point(2, 0));
                    points.Add(new Point(3, 0));
                    points.Add(new Point(4, 0));
                    points.Add(new Point(5, 0));
                    return points;
                case RockType.plus:
                    points.Add(new Point(3, 1));
                    points.Add(new Point(2, 1));
                    points.Add(new Point(3, 2));
                    points.Add(new Point(3, 0));
                    points.Add(new Point(4, 1));
                    return points;
                case RockType.line:
                    points.Add(new Point(2, 0));
                    points.Add(new Point(2, 1));
                    points.Add(new Point(2, 2));
                    points.Add(new Point(2, 3));
                    return points;
                case RockType.cube:
                    points.Add(new Point(2, 0));
                    points.Add(new Point(2, 1));
                    points.Add(new Point(3, 0));
                    points.Add(new Point(3, 1));
                    return points;
                case RockType.lshape:
                    points.Add(new Point(2, 2));
                    points.Add(new Point(3, 2));
                    points.Add(new Point(4, 2));
                    points.Add(new Point(4, 1));
                    points.Add(new Point(4, 0));
                    return points;
                default:
                    throw new Exception("Shape not handeled");

            }

            return null;
        }
    }

    public enum RockType
    {
        minus,
        plus,
        lshape,
        cube,
        line
    }
}
