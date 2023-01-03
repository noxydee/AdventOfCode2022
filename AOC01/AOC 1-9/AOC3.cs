namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC3
    {
        public static void Run()
        {
            List<Rucksack> rucksacks = new List<Rucksack>();

            foreach (string line in File.ReadLines("AOC 1-9/InputFiles/TextFile3.txt"))
            {
                Rucksack rucksack = new Rucksack(line);
                rucksacks.Add(rucksack);
            }

            int sumOfBadges = 0;
            int rucksacksChecked = 0;

            for (int i = 0; i < rucksacks.Count; i += 3)
            {
                rucksacksChecked += 3;
                Rucksack firstRucksack = rucksacks[i];
                Rucksack secondRucksack = rucksacks[i + 1];
                Rucksack thirdRucksack = rucksacks[i + 2];

                char badge = firstRucksack.WholeBag.Where(x => secondRucksack.WholeBag.Where(y => thirdRucksack.WholeBag.Contains(y)).Contains(x)).First();

                int move = (int)badge <= 90 ? 38 : 96;

                sumOfBadges += (int)(badge) - move;
            }

            Console.WriteLine(sumOfBadges);
            Console.WriteLine(rucksacksChecked);
        }
    }

    public class Rucksack
    {
        public List<char> WholeBag { get; set; }
        public List<char> LeftCompartment { get; set; }
        public List<char> RightCompartment { get; set; }
        public char DuplicatedChar { get; set; }
        public int DucplicatedCharValue { get; set; }

        public Rucksack(string input)
        {
            LeftCompartment = new();
            RightCompartment = new();
            WholeBag = new();
            WholeBag = input.ToCharArray().ToList();
            LeftCompartment = input.Substring(0, input.Length / 2).ToCharArray().ToList();
            RightCompartment = input.Substring((input.Length / 2), (input.Length / 2)).ToCharArray().ToList();

            DuplicatedChar = LeftCompartment.Where(x => RightCompartment.Contains(x)).First();

            int move = (int)DuplicatedChar <= 90 ? 38 : 96;
            
            DucplicatedCharValue = (int)(DuplicatedChar) - move;
        }
    }
}
