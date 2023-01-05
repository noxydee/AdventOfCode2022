namespace AOC01.AOC_10_25.AOC13
{
    using System.Collections.Generic;

    internal class AOC13
    {
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC13/TextFile13_2.txt");
            List<int> indexesInRightOrder = new List<int>();
            int index = 0;
            int totalSum = 0;

            for (int i = 0; i < lines.Count(); i+=3)
            {
                index++;
                Node firstSignal = GetNodeTreeFromLine(lines.ElementAt(i)).Child.First().Value;
                Node secondSignal = GetNodeTreeFromLine(lines.ElementAt(i+1)).Child.First().Value;

                bool isSignalInOrder = IsSignalInOrder(firstSignal, secondSignal);

                if (isSignalInOrder)
                {
                    totalSum += index;
                    indexesInRightOrder.Add(index);
                }
            }

            Console.WriteLine(totalSum);
            Console.WriteLine(string.Join(",", indexesInRightOrder));
        }

        public static bool IsSignalInOrder(Node firstSignal, Node secondSignal)
        {
            int maxSignalSize = firstSignal.Child.Count > secondSignal.Child.Count ? firstSignal.Child.Count : secondSignal.Child.Count;

            for (int i = 0; i < maxSignalSize; i++)
            {
                if (firstSignal.Child.Count-1 < i && secondSignal.Child.Count >= i)
                {
                    return true;
                }
                if (secondSignal.Child.Count-1 < i && firstSignal.Child.Count >= i)
                {
                    return false;
                }

                KeyValuePair<int, Node> firstValue = firstSignal.Child.ElementAt(i);
                KeyValuePair<int, Node> secondValue = secondSignal.Child.ElementAt(i);

                if (firstValue.Key >= 0 && secondValue.Key >= 0)
                {
                    Console.WriteLine($"Compare: {firstValue.Key} vs {secondValue.Key}");

                    if (firstValue.Key < secondValue.Key)
                    {
                        return true;
                    }
                    else if (firstValue.Key > secondValue.Key)
                    {
                        return false;
                    }
                }
                else
                {
                    Node firstSignalNextNode = null;
                    Node secondSignalNextNode = null;
                    if (firstValue.Key != -1)
                    {
                        firstSignalNextNode = new Node();
                        firstSignalNextNode.Child.Add(new KeyValuePair<int, Node>(firstValue.Key, null));
                    }
                    if (secondValue.Key != -1)
                    {
                        secondSignalNextNode = new Node();
                        secondSignalNextNode.Child.Add(new KeyValuePair<int, Node>(secondValue.Key, null));
                    }

                    return IsSignalInOrder(firstSignalNextNode == null ? firstValue.Value : firstSignalNextNode, secondSignalNextNode == null ? secondValue.Value : secondSignalNextNode);
                }
            }

            return true;
        }

        public static List<int> GetValue(Node node, int index)
        {
            while (true)
            {
                if (node.Child.Count > index)
                {
                    return null;
                }

                if (node.Child.ElementAt(index).Key == -1)
                {
                    node = node.Child.ElementAt(index).Value;
                    index = 0;
                }
                else
                {
                    return node.Child.Where(x => x.Key >= 0).Select(x => x.Key).ToList();
                }
            }
        }

        public static Node GetNodeTreeFromLine(string line)
        {
            Node root = new Node();
            Node currentNode = root;

            foreach (char character in line.ToCharArray())
            {
                if (character == '[')
                {
                    Node child = new Node();
                    child.Parent = currentNode;
                    currentNode.Child.Add(new KeyValuePair<int, Node>(-1, child));
                    currentNode = child;
                }
                else if (character == ']')
                {
                    currentNode = currentNode.Parent;
                }
                else if (character == ',')
                {

                }
                else
                {
                    currentNode.Child.Add(new KeyValuePair<int, Node>(character - 48, null));
                }
            }

            return root;
        }
    }

    public class Node
    {
        public List<KeyValuePair<int, Node>> Child { get; set; }
        public Node Parent { get; set; }

        public Node()
        {
            Child = new List<KeyValuePair<int, Node>>();
        }
    }
}
