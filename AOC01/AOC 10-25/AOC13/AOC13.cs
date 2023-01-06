namespace AOC01.AOC_10_25.AOC13
{
    using System.Collections.Generic;
    using System.Reflection.Metadata.Ecma335;
    using System.Reflection.PortableExecutable;
    using System.Runtime.CompilerServices;
    using System.Text;

    using static System.Net.Mime.MediaTypeNames;

    internal static class AOC13
    {
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC13/TextFile13.txt");
            List<int> indexesInRightOrder = new List<int>();
            int index = 0;
            int totalSum = 0;

            for (int i = 0; i < lines.Count(); i+=3)
            {
                index++;

                string firstSignal = lines.ElementAt(i);
                string secondSignal = lines.ElementAt(i+1);

                bool? isSignalInOrder = Compare(firstSignal, secondSignal);

                if (isSignalInOrder != null && isSignalInOrder == true)
                {
                    totalSum += index;
                    indexesInRightOrder.Add(index);
                }
                //Node firstSignal = GetNodeTreeFromLine(lines.ElementAt(i)).Child.First().Value;
                //Node secondSignal = GetNodeTreeFromLine(lines.ElementAt(i+1)).Child.First().Value;

                //bool isSignalInOrder = IsSignalInOrder(firstSignal, secondSignal);

            }

            Console.WriteLine(totalSum);
            Console.WriteLine(string.Join(",", indexesInRightOrder));
        }

        public static bool? Compare(string firstSignal, string secondSignal)
        {
            firstSignal = firstSignal.ReplaceFirst("[", "").ReplaceLast("]", "");
            secondSignal = secondSignal.ReplaceFirst("[", "").ReplaceLast("]", "");

            if (firstSignal.Length == 0 && secondSignal.Length > 0)
            {
                return true;
            }
            if (secondSignal.Length == 0 && firstSignal.Length > 0)
            {
                return false;
            }
            if (firstSignal.Length == 0 && secondSignal.Length == 0)
            {
                return null;
            }

            List<string> firstSignalChildElements = firstSignal.SplitByOuterSymbol(',');
            List<string> secondSignalChildElements = secondSignal.SplitByOuterSymbol(',');

            for (int i = 0; i < Math.Max(firstSignalChildElements.Count, secondSignalChildElements.Count); i++)
            {
                if (firstSignalChildElements.Count <= i)
                {
                    return true;
                }
                if (secondSignalChildElements.Count <= i)
                {
                    return false;
                }

                if (firstSignalChildElements.ElementAt(i).Length == 1 && secondSignalChildElements.ElementAt(i).Contains("["))
                {
                    firstSignalChildElements[i] = firstSignalChildElements[i].SurroundWithBrackets();
                }
                else if (secondSignalChildElements.ElementAt(i).Length == 1 && firstSignalChildElements.ElementAt(i).Contains("["))
                {
                    secondSignalChildElements[i] = secondSignalChildElements[i].SurroundWithBrackets();
                }

                if(firstSignalChildElements.ElementAt(i).Contains("[") && secondSignalChildElements.ElementAt(i).Contains("["))
                {
                    bool? result = Compare(firstSignalChildElements.ElementAt(i), secondSignalChildElements.ElementAt(i));

                    if (result != null)
                    {
                        return (bool)result;
                    }
                }

                if (firstSignalChildElements.ElementAt(i).Length == 1 && secondSignalChildElements.ElementAt(i).Length == 1)
                {
                    int firstValue = int.Parse(firstSignalChildElements.ElementAt(i));
                    int secondValue = int.Parse(secondSignalChildElements.ElementAt(i));

                    Console.WriteLine($"Compare: {firstValue} vs {secondValue}");

                    if (firstValue < secondValue)
                    {
                        return true;
                    }
                    else if (firstValue > secondValue)
                    {
                        return false;
                    }
                }
            }

            return null;
        }

        private static string ReplaceFirst(this string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }

        private static string ReplaceLast(this string text, string search, string replace)
        {
            int pos = text.LastIndexOf(search);

            if (pos == -1)
                return text;

            return text.Remove(pos, search.Length).Insert(pos, replace);
        }

        private static string SurroundWithBrackets(this string text)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append(text);
            builder.Append("]");

            return builder.ToString();
        }

        private static List<string> SplitByOuterSymbol(this string text, char separator)
        {
            List<string> results = new List<string>();

            string element = "";
            int nestLevel = 0;
            foreach (char character in text.ToCharArray())
            {
                if (character == separator && nestLevel == 0)
                {
                    results.Add(element);
                    element = "";
                    continue;
                }
                else if (character == '[')
                {
                    nestLevel++;
                }
                else if (character == ']')
                {
                    nestLevel--;
                }

                element += character;

            }

            results.Add(element);

            return results;
        }

        public static bool IsSignalInOrder(Node firstSignal, Node secondSignal)
        {
            int maxSignalSize = 0;
            
            if (firstSignal.Child == null || secondSignal.Child == null)
            {
                maxSignalSize = 1;
            }
            else
            {
                maxSignalSize = firstSignal.Child.Count > secondSignal.Child.Count ? firstSignal.Child.Count : secondSignal.Child.Count;
            }

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
                    if (firstValue.Key != -1)
                    {
                        firstSignal.Child[i] = new KeyValuePair<int, Node>(-1, new Node(firstValue.Key));
                    }
                    if (secondValue.Key != -1)
                    {
                        secondSignal.Child[i] = new KeyValuePair<int, Node>(-1, new Node(secondValue.Key));
                    }

                    return IsSignalInOrder(firstSignal.Child.ElementAt(i).Value, secondSignal.Child.ElementAt(i).Value);
                }
            }

            if (firstSignal.Parent == null)
            {
                return true;
            }
            

            int firstSignalNextIndex = firstSignal.Parent.Child.IndexOf(firstSignal.Parent.Child.FirstOrDefault(x => x.Value == firstSignal));
            int secondSignalNextIndex = secondSignal.Parent.Child.IndexOf(secondSignal.Parent.Child.FirstOrDefault(x => x.Value == secondSignal));

            if (secondSignal.Parent == null || secondSignal.Parent.Child.Count <= secondSignalNextIndex + 1)
            {
                return false;
            }

            Node firstNextParentNode = firstSignal.Parent.Child.ElementAt(firstSignalNextIndex + 1).Value ?? new Node(firstSignal.Parent.Child.ElementAt(firstSignalNextIndex + 1).Key);
            Node secondNextParentNode = secondSignal.Parent.Child.ElementAt(secondSignalNextIndex + 1).Value ?? new Node(secondSignal.Parent.Child.ElementAt(secondSignalNextIndex + 1).Key);

            return IsSignalInOrder(firstNextParentNode, secondNextParentNode);
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

        public Node(int value)
        {
            Child = new List<KeyValuePair<int, Node>>();
            Child.Add(new KeyValuePair<int, Node>(value, null));
        }
    }
}
