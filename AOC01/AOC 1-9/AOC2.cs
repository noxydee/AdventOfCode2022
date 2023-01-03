namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC2
    {
        public static void Run()
        {
            long totalScore = 0;

            foreach (string line in File.ReadLines("AOC 1-9/InputFiles/TextFile2.txt"))
            {
                char oponent = line[0];
                char myMove = line[2];

                totalScore += getSymbolValue(whatShouldBePicked(oponent, myMove));
                totalScore += getRes(myMove);

            }

            Console.WriteLine(totalScore);
        }

        private static int getRes(char outcome)
        {
            switch (outcome)
            {
                case 'X':
                    return 0;
                case 'Y':
                    return 3;
                case 'Z':
                    return 6;
                default:
                    throw new Exception("Invalid char");
            }
        }

        private static char whatShouldBePicked(char oponent, char outcome)
        {
            if (outcome == 'Y')
                return oponent;
            if (outcome == 'Z')
                return getCorespondingChar(oponent);
            if (outcome == 'X')
                return getCorespondingChar(getCorespondingChar(oponent));

            throw new Exception("Invalid char");
        }

        private static int getResult(char oponent, char player)
        {
            if (getCorespondingChar(oponent) == player)
            {
                return 3;
            }

            if ((oponent == 'A' && player == 'Y') || (oponent == 'B' && player == 'Z') || (oponent == 'C' && player == 'X'))
            {
                return 6;
            }

            return 0;
        }

        private static char getCorespondingChar(char npcInput)
        {
            switch(npcInput)
            {
                case 'A':
                    return 'B';
                case 'B':
                    return 'C';
                case 'C':
                    return 'A';
                default:
                    throw new Exception("Invalid npc char");
            }
        }

        private static int getSymbolValue(char move)
        {
            switch(move)
            {
                case 'A':
                    return 1;
                case 'B':
                    return 2;
                case 'C':
                    return 3;
                default:
                    throw new Exception("Invalid char");
            }
        }
    }
}
