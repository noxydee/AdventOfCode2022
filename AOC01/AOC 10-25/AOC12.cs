namespace AOC01.AOC_10_25
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    internal class AOC12
    {
        public static Node[,] Nodes = null;

        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/InputFiles/TextFile12_2.txt");
            int xSize = lines.First().Length;
            int ySize = lines.Count();
            Nodes = new Node[xSize, ySize];
            Node startingNode = null;
            Node targetNode = null;

            List<Node> openSet = new List<Node>();
            List<Node> closedSet = new List<Node>();

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    Nodes[x, y] = new Node(x, y, lines.ElementAt(y).ToCharArray().ElementAt(x));
                    if (Nodes[x, y].Z == 83)
                    {
                        startingNode = Nodes[x, y];
                        startingNode.Z = 97;
                    }
                    else if (Nodes[x, y].Z == 69)
                    {
                        targetNode = Nodes[x, y];
                        targetNode.Z = 122;
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

                //DrawMap(xSize, ySize, closedSet);

                if (currentNode.Equals(targetNode))
                {
                    Console.WriteLine("Found target node");
                    break;
                }

                foreach (Node child in currentNode.ChildNodes.OrderByDescending(x=>x.TotalCost))
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
                if (parent.Equals(startingNode))
                {
                    break;
                }    
            }

            DrawMap(xSize, ySize, path);

            Console.WriteLine(counter);
        }

        public static void DrawMap(int xSize, int ySize, List<Node> closedSet)
        {
            Console.Clear();
            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {
                    if (closedSet.Contains(Nodes[x,y]))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write($"[{(char)Nodes[x, y].Z}]");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine();
            }

            Thread.Sleep(1000);
        }

        public static void SetPossibleChildNodes(Node node)
        {
            Node down = Nodes[node.X, node.Y + 1];
            Node up = Nodes[node.X, node.Y - 1];
            Node right = Nodes[node.X + 1, node.Y];
            Node left = Nodes[node.X - 1, node.Y];
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

        public Node(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            ChildNodes = new();
        }

        public void CalculateCost(Node start, Node target)
        {
            EndCost = Math.Sqrt(Math.Pow(Convert.ToDouble(Math.Abs(X - target.X)), 2d) + Math.Pow(Convert.ToDouble(Math.Abs(Y-target.Y)), 2d));
            StartCost = GetStartCost(start);
            TotalCost = EndCost + StartCost;
        }

        public double GetStartCost(Node start)
        {
            return Math.Sqrt(Math.Pow(Convert.ToDouble(Math.Abs(X - start.X)), 2d) + Math.Pow(Convert.ToDouble(Math.Abs(Y - start.Y)), 2d));
        }
    }
}
