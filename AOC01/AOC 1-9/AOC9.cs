namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC9
    {
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 1-9/InputFiles/TextFile9.txt");
            Rope rope = new Rope();

            foreach (string line in lines)
            {
                string[] inputParams = line.Split(" ");

                switch(inputParams[0]) 
                {
                    case "U":
                        rope.Move(Direction.Up, int.Parse(inputParams[1]));
                        break;
                    case "D":
                        rope.Move(Direction.Down, int.Parse(inputParams[1]));
                        break;
                    case "L":
                        rope.Move(Direction.Left, int.Parse(inputParams[1]));
                        break;
                    case "R":
                        rope.Move(Direction.Right, int.Parse(inputParams[1]));
                        break;
                    default:
                        throw new Exception($"Invalid direction {inputParams[0]}");
                }
            }

            Console.WriteLine(rope.PointsVisitedByTail.GroupBy(x=>x.X.ToString() + x.Y.ToString()).Count());
        }
    }

    public class Rope
    {
        public Point Head;
        //public Point Tail;
        public List<Point> TailPoints { get; set; }

        public List<Point> PointsVisitedByTail { get; set; }

        public Rope()
        {
            Head = new Point(0, 0);
            TailPoints = new();
            for (int i = 0; i < 9; i++)
            {
                TailPoints.Add(new Point(0, 0));
            }
            PointsVisitedByTail = new();
            PointsVisitedByTail.Add(new Point(0, 0));
        }

        public void Move(Direction direction, int length)
        {
            for (int i = 0; i < length; i++)
            {
                switch(direction)
                {
                    case Direction.Up:
                        Head.Y++;
                        break;
                    case Direction.Down:
                        Head.Y--;
                        break;
                    case Direction.Left:
                        Head.X--;
                        break;
                    case Direction.Right:
                        Head.X++;
                        break;
                    default:
                        throw new Exception("Invalid direction");
                };

                for(int ii=0;ii<TailPoints.Count;ii++)
                {
                    if (Math.Abs(TailPoints[ii].X - (ii > 0 ? TailPoints[ii - 1].X : Head.X)) > 1 || Math.Abs(TailPoints[ii].Y - (ii > 0 ? TailPoints[ii - 1].Y : Head.Y)) > 1)
                    {
                        UpdateTailPosition(TailPoints[ii], ii > 0 ? TailPoints[ii-1] : Head);
                    }
                }
            }
        }

        private void UpdateTailPosition(Point tail, Point head)
        {
            if (tail.X == head.X)
            {
                tail.Y = head.Y > tail.Y ? ++tail.Y : --tail.Y;
            }
            else if (tail.Y == head.Y)
            {
                tail.X = head.X > tail.X ? ++tail.X : --tail.X;
            }
            else
            {
                tail.Y = head.Y > tail.Y ? ++tail.Y : --tail.Y;
                tail.X = head.X > tail.X ? ++tail.X : --tail.X;
            }

            if(tail.Equals(TailPoints.Last()))
            {
                PointsVisitedByTail.Add(new Point(tail.X, tail.Y));
            }
        }
    }

    public enum Direction
    {
        Up, 
        Down, 
        Left, 
        Right
    }

    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
