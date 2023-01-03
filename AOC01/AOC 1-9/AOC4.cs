namespace AOC01
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC4
    {
        public static void Run()
        {
            int totalContaining = 0;
            int totalOverlaping = 0;

            foreach (string line in File.ReadLines("AOC 1-9/InputFiles/TextFile4.txt"))
            {
                string[] assigments = line.Split(' ', ',');
                Worker firstWorker = new Worker(assigments[0]);
                Worker secondWorker = new Worker(assigments[1]);
                Pair pair = new Pair(firstWorker, secondWorker);

                totalContaining += pair.DoesAssigmentsContainEachOther() ? 1 : 0;
                totalOverlaping += pair.DoesAssigmentsOverlap() ? 1 : 0;
            }

            Console.WriteLine(totalContaining);
            Console.WriteLine(totalOverlaping);
        }
    }

    public class Worker
    {
        public int AssigmentFrom { get; set; }
        public int AssigmentTo { get; set; }

        public Worker(string assigment)
        {
            string[] fromToAssigment = assigment.Split(' ', '-');
            AssigmentFrom = int.Parse(fromToAssigment[0]);
            AssigmentTo = int.Parse(fromToAssigment[1]);
        }
    }

    public class Pair
    {
        private Worker FirstWorker { get; set; }
        private Worker SecondWorker { get; set; }

        public Pair(Worker firstWorker, Worker secondWorker)
        {
            FirstWorker = firstWorker;
            SecondWorker = secondWorker;
        }

        public bool DoesAssigmentsContainEachOther()
        {
            if (FirstWorker.AssigmentFrom >= SecondWorker.AssigmentFrom && FirstWorker.AssigmentTo <= SecondWorker.AssigmentTo)
            {
                return true;
            }
            else if (SecondWorker.AssigmentFrom >= FirstWorker.AssigmentFrom && SecondWorker.AssigmentTo <= FirstWorker.AssigmentTo)
            {
                return true;
            }

            return false;
        }

        public bool DoesAssigmentsOverlap()
        {
            if (FirstWorker.AssigmentFrom < SecondWorker.AssigmentFrom && FirstWorker.AssigmentTo < SecondWorker.AssigmentFrom)
            {
                return false;
            }
            else if (SecondWorker.AssigmentFrom < FirstWorker.AssigmentFrom && SecondWorker.AssigmentTo < FirstWorker.AssigmentFrom)
            {
                return false;
            }

            return true;
        }
    }
}
