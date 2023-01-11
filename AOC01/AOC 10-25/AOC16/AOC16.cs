﻿namespace AOC01.AOC_10_25.AOC16
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class AOC16
    {
        public static List<Valve> Valves = new List<Valve>();

        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC16/TextFile16_2.txt");

            foreach (string line in lines)
            {
                List<string> values = line.Split(" ").ToList();

                Valve valve = new Valve(values[1], int.Parse(values[4].Where(Char.IsDigit).ToArray()));

                Valves.Add(valve);
            }

            foreach (string line in lines)
            {
                List<string> values = line.Split(" ").ToList();

                Valve valve = Valves.First(x => x.Name.Equals(values[1]))
                    ?? throw new Exception($"Valve {values[1]} not found");

                foreach(string connectedValve in values.Skip(9))
                {
                    string connectedValveName = connectedValve.Replace(",", "");
                }
            }
        }
    }

    public class Valve
    {
        public string Name { get; set; }

        public int Rate { get; set; }

        public List<Valve> ConnectedValves { get; set; }

        public Valve(string name, int rate)
        {
            Name = name;
            Rate = rate;
            ConnectedValves = new List<Valve>();
        }
    }
}