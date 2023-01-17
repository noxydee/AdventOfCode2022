namespace AOC01.AOC_10_25.AOC16
{
    using AOC01.AOC_10_25.AOC16.FormView;

    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    internal class AOC16
    {
        public static List<Valve> Valves = new List<Valve>();

        [STAThread]
        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC16/TextFile16_2.txt");

            InputValues(lines.ToList());

            GraphVisualization graphHelper = new GraphVisualization(1000, 1000, Color.Peru, Color.PeachPuff, Valves);
            graphHelper.SaveGraphImage();

            var x = 10;
        }

        private static void InputValues(List<string> lines)
        {
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

                foreach (string connectedValve in values.Skip(9))
                {
                    string connectedValveName = connectedValve.Replace(",", "");

                    Valve childValve = Valves.FirstOrDefault(x => x.Name.Equals(connectedValveName))
                        ?? throw new Exception($"Valve {connectedValveName} not found");

                    valve.ConnectedValves.Add(childValve);
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
