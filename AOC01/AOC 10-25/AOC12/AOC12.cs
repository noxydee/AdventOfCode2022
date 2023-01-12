namespace AOC01.AOC_10_25
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    internal class AOC12
    {
        public static Node[,] Nodes = null;

        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC12/TextFile12.txt");
            int xSize = lines.First().Length;
            int ySize = lines.Count();
            Nodes = new Node[xSize, ySize];
            Node startingNode = null;
            Node targetNode = null;
            Bitmap bitmap = new Bitmap(xSize, ySize);

            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>();

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    Nodes[x, y] = new Node(x, y, lines.ElementAt(y).ToCharArray().ElementAt(x));
                    bitmap.SetPixel(x, y, Color.FromArgb(50, (Nodes[x, y].Z * 25 % 255), 50));
                    if (Nodes[x, y].Z == 83)
                    {
                        startingNode = Nodes[x, y];
                        startingNode.Z = 97;
                        bitmap.SetPixel(x, y, Color.FromArgb(0, 255, 0));
                    }
                    else if (Nodes[x, y].Z == 69)
                    {
                        targetNode = Nodes[x, y];
                        targetNode.Z = 122;
                        bitmap.SetPixel(x, y, Color.FromArgb(255, 0, 0));
                    }
                }
            }

            foreach (Node node in Nodes)
            {
                if (IsSuitableChild(node.Z, node.X, node.Y - 1))
                {
                    node.ChildNodes.Add(Nodes[node.X, node.Y - 1]);
                }
                if (IsSuitableChild(node.Z, node.X, node.Y + 1))
                {
                    node.ChildNodes.Add(Nodes[node.X, node.Y + 1]);
                }
                if (IsSuitableChild(node.Z, node.X + 1, node.Y))
                {
                    node.ChildNodes.Add(Nodes[node.X + 1, node.Y]);
                }
                if (IsSuitableChild(node.Z, node.X - 1, node.Y))
                {
                    node.ChildNodes.Add(Nodes[node.X - 1, node.Y]);
                }
            }

            List<Node> possibleStartingNodes = new List<Node>();
            foreach (Node node in Nodes) 
            {
                if (node.Z == 97 && node.X == 0)
                {
                    possibleStartingNodes.Add(node);
                }
            }

            int minPathLenght = int.MaxValue;
            int totalChecks = possibleStartingNodes.Count;
            int currentCheck = 1;
            foreach (Node node in possibleStartingNodes)
            {
                if (!node.IsCave)
                {
                    List<Node> path = GetAStarPath(node, targetNode, bitmap);
                    Console.WriteLine($"{currentCheck}/{possibleStartingNodes.Where(x=> !x.IsCave).Count()}");
                    currentCheck++;

                    if (path != null)
                    {
                        minPathLenght = minPathLenght < path.Count ? minPathLenght : path.Count;
                    }
                }
            }

            Console.WriteLine(minPathLenght);
        }

        public static bool IsSuitableChild(int parentElevation, int childX, int childY)
        {
            try 
            {
                return Nodes[childX, childY].Z - 1 <= parentElevation; 
            } 
            catch(Exception ignore) 
            {
                return false;
            }
        }

        public static List<Node> GetAStarPath(Node startingNode, Node targetNode, Bitmap bitmap = null)
        {
            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>();

            openSet.Add(startingNode);
            foreach (Node child in startingNode.ChildNodes)
            {
                child.CalculateCost(startingNode, targetNode);
                child.Parent = startingNode;
            }

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.OrderBy(x => x.EndCost).First();
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode.Equals(targetNode))
                {
                    Console.WriteLine("Found target node");
                    break;
                }

                foreach (Node child in currentNode.ChildNodes.OrderByDescending(x => x.TotalCost))
                {
                    if (!closedSet.Contains(child))
                    {
                        if (!openSet.Contains(child) || child.GetStartCost(targetNode) < child.StartCost)
                        {
                            child.CalculateCost(currentNode, targetNode);
                            child.Parent = currentNode;

                            if (!openSet.Contains(child))
                            {
                                openSet.Add(child);
                            }
                        }
                    }
                }
            }

            int counter = 0;
            List<Node> path = new List<Node>();
            Node parent = targetNode;
            while (true)
            {
                path.Add(parent);
                parent = parent.Parent;
                counter++;
                if (parent != null && bitmap != null)
                {
                    bitmap.SetPixel(parent.X, parent.Y, Color.FromArgb(230, 200, 0));
                }
                if (parent == null || parent.Equals(startingNode))
                {
                    if (closedSet.All(x => x.Z == 97))
                    {
                        closedSet.ForEach(x => Nodes[x.X, x.Y].IsCave = true);
                    }

                    closedSet.ForEach(x => bitmap.SetPixel(x.X, x.Y, Color.Pink));

                    if (parent == null)
                    {
                        return null;
                    }

                    break;
                }
            }

            if (bitmap != null)
            {
                bitmap.Save("result.bmp", ImageFormat.Bmp);
            }
             
            return path;
        }
    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public double StartCost { get; set; }
        public double EndCost { get; set; }
        public double TotalCost { get; set; } 
        public List<Node> ChildNodes { get; set; }
        public Node Parent { get; set; }
        public bool IsCave = false;

        public Node(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            ChildNodes = new();
        }

        public void CalculateCost(Node start, Node target)
        {
            EndCost = Convert.ToDouble(Math.Abs(X - start.X)) + Convert.ToDouble(Math.Abs(Y - start.Y));
            //EndCost = Math.Sqrt(Math.Pow(Convert.ToDouble(Math.Abs(X - target.X)), 2d) + Math.Pow(Convert.ToDouble(Math.Abs(Y-target.Y)), 2d));
            StartCost = GetStartCost(start);
            TotalCost = EndCost + StartCost;
        }

        public double GetStartCost(Node start)
        {
            return Convert.ToDouble(Math.Abs(X - start.X)) + Convert.ToDouble(Math.Abs(Y - start.Y));
            //return Math.Sqrt(Math.Pow(Convert.ToDouble(Math.Abs(X - start.X)), 2d) + Math.Pow(Convert.ToDouble(Math.Abs(Y - start.Y)), 2d));
        }
    }
}
