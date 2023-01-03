namespace AOC01.AOC_10_25
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Numerics;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC11
    {
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/InputFiles/TextFile11.txt");
            List<Monkey> monkeys = new List<Monkey>();

            foreach (string line in lines)
            {
                if (line.StartsWith("Monkey"))
                {
                    Monkey monkey = new(int.Parse(line.Split(" ").Last().Replace(":", "")));
                    monkeys.Add(monkey);
                }
                else if (line.TrimStart().StartsWith("Operation:"))
                {
                    string expression = line.Split("=").Last().Trim();
                    monkeys.Last().Operation = expression;
                }
                else if (line.TrimStart().StartsWith("Starting items:"))
                {
                    List<string> itemNumbers = line.Trim().Replace("Starting items:", "").Split(",").ToList();
                    itemNumbers.ForEach(x => monkeys.Last().Items.Add(new Item(BigInteger.Parse(x))));
                }
                else if (line.TrimStart().StartsWith("Test:"))
                {
                    int divisibleBy = int.Parse(line.Split(" ").Last());
                    monkeys.Last().divisibleBy = divisibleBy;
                }
                else if (line.TrimStart().StartsWith("If true:"))
                {
                    monkeys.Last().TrueMonkeyTarget = int.Parse(line.Split(" ").Last());
                }
                else if (line.TrimStart().StartsWith("If false:"))
                {
                    monkeys.Last().FalseMonkeyTarget = int.Parse(line.Split(" ").Last());
                }
            }

            long LCM = MathHelpers.LeastCommonMultiple(monkeys.Select(x => (long)x.divisibleBy));

            int maxRoundCount = 10000;

            for (int i = 0; i < maxRoundCount; i++)
            {
                foreach (Monkey monkey in monkeys)
                {
                    List<Item> newItemList = monkey.Items.AsEnumerable().ToList();
                    foreach (Item item in monkey.Items)
                    {
                        BigInteger operationResult = monkey.GetOperationResult(item.Value);
                        //operationResult = (int)(operationResult / 10);
                        
                        while (operationResult > LCM)
                        {
                            //operationResult = BigInteger.Subtract(operationResult, LCM);
                            operationResult = BigInteger.Remainder(operationResult, LCM);
                        }

                        item.Value = operationResult;

                        if (operationResult % (ulong)monkey.divisibleBy == 0)
                        {
                            //monkey.Items.Remove(item);
                            monkeys.ElementAt(monkey.TrueMonkeyTarget).Items.Add(item);
                        }
                        else
                        {
                            //monkey.Items.Remove(item);
                            monkeys.ElementAt(monkey.FalseMonkeyTarget).Items.Add(item);
                        }

                        newItemList.Remove(item);

                    }

                    monkey.Items = newItemList;
                }
                
                Console.WriteLine($"Round {i}/{maxRoundCount}");
            }

            var result = monkeys.OrderByDescending(x => x.InspectCount).ElementAt(0).InspectCount * monkeys.OrderByDescending(x => x.InspectCount).ElementAt(1).InspectCount;

            Console.WriteLine(result);
        }
    }

    public class Monkey
    {
        public int Number { get; set; }
        public List<Item> Items { get; set; }
        public string Operation { get; set; }
        public int divisibleBy { get; set; }
        public int TrueMonkeyTarget { get; set; }
        public int FalseMonkeyTarget { get; set; }

        public long InspectCount = 0;

        public Monkey(int number)
        {
            Number = number;
            Items = new List<Item>();
        }

        public BigInteger GetOperationResult(BigInteger itemWorryLevel)
        {
            InspectCount++;

            DataTable dt = new DataTable();
            string expression = Operation.Replace("old", itemWorryLevel.ToString());

            List<string> expressionValues = expression.Split(" ").ToList();

            switch (expressionValues.ElementAt(1))
            {
                case "+":
                    return BigInteger.Add(BigInteger.Parse(expressionValues.First()), BigInteger.Parse(expressionValues.Last()));
                case "*":
                    return BigInteger.Multiply(BigInteger.Parse(expressionValues.First()), BigInteger.Parse(expressionValues.Last()));
                default:
                    throw new Exception($"Not implemented operation {expressionValues.ElementAt(1)}");
            }
            //return (BigInteger)dt.Compute(expression, "");
        }
    }

    public class Item
    {
        public BigInteger Value { get; set; }

        public Item(BigInteger value)
        {
            Value = value;
        }
    }

    public static class MathHelpers
    {
        public static long GreatestCommonDivisor(long a, long b) 
        {
            while (b != 0)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static long LeastCommonMultiple(long a, long b)
            => a / GreatestCommonDivisor(a, b) * b;

        public static long LeastCommonMultiple(this IEnumerable<long> values)
            => values.Aggregate(LeastCommonMultiple);
    }
}
