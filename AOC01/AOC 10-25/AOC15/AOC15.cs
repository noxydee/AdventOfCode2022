namespace AOC01.AOC_10_25.AOC15
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal class AOC15
    {
        private static List<Sensor> Sensors = new List<Sensor>();
        private static long CoordinatesMaxValue = 4000000;

        private static Bitmap bitmap = new Bitmap(30, 30);

        public static void Run()
        {
            IEnumerable<string> lines = File.ReadLines("AOC 10-25/AOC15/TextFile15.txt");

            foreach (string line in lines)
            {
                List<long> coordinates = Regex.Matches(line, @"\d+").Select(m => long.Parse(m.Value)).ToList();
                long sensorX = coordinates[0];
                long sensorY = coordinates[1];
                long beaconX = coordinates[2];
                long beaconY = coordinates[3];

                Sensors.Add(new Sensor(sensorX, sensorY, beaconX, beaconY));
            }

            Sensor bottomLeft = null;
            Sensor topRight = null;
            Sensor topLeft = null;
            Sensor bottomRight = null;

            foreach(Sensor sensor in Sensors) 
            {
                foreach (Sensor sensor2 in Sensors)
                {
                    if (sensor.LinearB1 + 2 == sensor2.LinearB3)
                    {
                        Console.WriteLine("Found 1");
                        topRight = (Sensor)sensor2.Clone();
                        bottomLeft = (Sensor)sensor.Clone();

                        //Draw(topRight);
                        //Draw(bottomLeft);
                    }

                    if (sensor.LinearB4 + 2 == sensor2.LinearB2)
                    {
                        Console.WriteLine("Found 2");
                        topLeft = (Sensor)sensor2.Clone();
                        bottomRight = (Sensor)sensor.Clone();

                        //Draw(topLeft);
                        //Draw(bottomRight);
                    }
                }
            }

            //bitmap.SetPixel(14, 11, Color.White);
            //bitmap.SetPixel((int)topRight.SensorX, (int)topRight.SensorY, Color.White);
            //bitmap.SetPixel((int)bottomRight.SensorX, (int)bottomRight.SensorY, Color.White);
            //bitmap.SetPixel((int)bottomLeft.SensorX, (int)bottomLeft.SensorY, Color.White);
            //bitmap.SetPixel((int)topLeft.SensorX, (int)topLeft.SensorY, Color.White);

            //bitmap.Save("AOC15.bmp", ImageFormat.Bmp);

            //FOR 
            // sensor bottomLeft and topRight
            // y=-x+b
            //FOR
            // sensor bottomRight and topLeft
            // y=x+b

            long xCoordinate = ((topRight.LinearB3 - bottomRight.LinearB4) / 2);
            long yCoordinate = bottomRight.LinearB4 + 1;

            int rightIndex = 0;
            List<Range> topLeftRanges = topLeft.CoverRanges.OrderBy(x => x.Y).ToList();
            List<Range> topRightRanges = topRight.CoverRanges.OrderBy(x => x.Y).ToList();
            rightIndex = topRightRanges.IndexOf(topRightRanges.FirstOrDefault(x => x.Y == topLeftRanges.First().Y));

            for (int leftIndex = 0; leftIndex < topLeftRanges.Count; leftIndex++)
            {
                if (topLeftRanges[leftIndex].To == topRightRanges[rightIndex].From)
                {
                    Console.WriteLine($"LeftTo={topLeftRanges[leftIndex].To} RightFrom={topRightRanges[rightIndex].From} LeftY={topLeftRanges[leftIndex].Y} RightY={topRightRanges[rightIndex].Y}");

                    BigInteger xCord = new BigInteger(topLeftRanges[leftIndex].To);
                    BigInteger yCord = new BigInteger(topLeftRanges[leftIndex].Y - 1);
                    BigInteger multi = new BigInteger(4000000);
                    BigInteger result = BigInteger.Multiply(xCord, multi);
                    result = BigInteger.Add(result, yCord);

                    Console.WriteLine(result);
                    break;
                }
                rightIndex++;
            }

            //BigInteger xCord = new BigInteger(xCoordinate);
            //BigInteger yCord = new BigInteger(yCoordinate);
            //BigInteger multi = new BigInteger(4000000);
            //BigInteger result = BigInteger.Multiply(xCord, multi);
            //result = BigInteger.Add(result, yCord);

            //Console.WriteLine(result);

            //List<ThreadStart> threadStarters = new List<ThreadStart>();
            //List<Thread> threads = new List<Thread>();
            //foreach(Sensor sensor in Sensors)
            //{
            //    threadStarters.Add(() => RunSearchOnSensors(sensor));
            //    threads.Add(new Thread(threadStarters.Last()));
            //}

            //threads.ForEach(x => x.Start());
            //Console.ReadKey();

            /*foreach (Sensor sensor in Sensors.Skip(7))
            {
                Console.WriteLine($"Processing: sensor {Sensors.IndexOf(sensor)}");
                foreach (Range range in sensor.CoverRanges)
                {
                    Console.WriteLine($"Processing: range {sensor.CoverRanges.IndexOf(range)}/{sensor.CoverRanges.Count}");
                    List<Range> otherRanges = GetRanges(range.Y, range.From);

                    if (otherRanges.Count > 0 && otherRanges.Any(x => range.To + 2 == x.From)) 
                    {
                        BigInteger result = BigInteger.Add(BigInteger.Multiply(BigInteger.Parse((range.To + 1).ToString()), BigInteger.Parse(CoordinatesMaxValue.ToString())), BigInteger.Parse(range.Y.ToString()));
                        Console.WriteLine($"Found result: x={range.To+1} y={range.Y} | result={result}");
                    }
                    
                }
            }*/

            //foreach (Range range in topLeft.CoverRanges.Skip(500000))
            //{
            //    foreach (Range range2 in topRight.CoverRanges.Where(x => x.Y == range.Y))
            //    {
            //        if (range.To == range2.From && range.Y == range2.Y)
            //        {
            //            Console.WriteLine($"x={range.To} y={range.Y}");
            //            var x = 10;
            //        }
            //    }

            //    Console.WriteLine($"Search: {topLeft.CoverRanges.IndexOf(range)}/{topLeft.CoverRanges.Count - 510000}");
            //}
        }

        private static void Draw(Sensor sensor)
        {
            Random n= new Random();
            var blue = n.Next(255);
            var green = n.Next(255);
            var red = n.Next(255);
            foreach (Range range in sensor.CoverRanges)
            {
                for (long x = range.From >=0 ? range.From : 0; x <= range.To; x++)
                {
                    bitmap.SetPixel((int)x, (int)range.Y < 0 ? 0 : (int)range.Y, Color.FromArgb(red, green, blue));
                }
            }
        }

        private static void RunSearchOnSensors(Sensor sensor)
        {
            Console.WriteLine($"Processing: sensor {Sensors.IndexOf(sensor)}");
            foreach (Range range in sensor.CoverRanges)
            {
                Console.WriteLine($"Processing: range {sensor.CoverRanges.IndexOf(range)}/{sensor.CoverRanges.Count}");
                List<Range> otherRanges = GetRanges(range.Y, range.From);

                if (otherRanges.Count > 0 && otherRanges.Any(x => range.To + 2 == x.From))
                {
                    BigInteger result = BigInteger.Add(BigInteger.Multiply(BigInteger.Parse((range.To + 1).ToString()), BigInteger.Parse(CoordinatesMaxValue.ToString())), BigInteger.Parse(range.Y.ToString()));
                    Console.WriteLine($"Found result: x={range.To + 1} y={range.Y} | result={result}");
                }

            }
        }

        private static List<Range> GetRanges(long height, long from)
        {
            List<Range> ranges = new List<Range>();

            foreach (Sensor sensor in Sensors.Where(x => x.MinY <= height && x.MaxX >= height))
            {
                ranges.AddRange(sensor.CoverRanges.Where(x => x.Y == height));
            }

            return ranges;
        }
    }

    public class Sensor
    {
        public long SensorX { get; set; }
        public long SensorY { get; set; }

        public long BeaconX { get; set; }
        public long BeaconY { get; set; }

        public long Radius { get; set; }

        public long MaxY { get; set; }
        public long MinY { get; set; }
        public long MaxX { get; set; }
        public long MinX { get; set; }

        public List<Range> CoverRanges { get; set; }

        public long LinearB1 { get; set; }
        public long LinearB2 { get; set; }
        public long LinearB3 { get; set; }
        public long LinearB4 { get; set; }

        public Sensor(long sensorX, long sensorY, long beaconX, long beaconY)
        {
            this.SensorX = sensorX;
            this.SensorY = sensorY;
            this.BeaconX = beaconX;
            this.BeaconY = beaconY;

            Radius = Math.Abs(sensorX - beaconX) + Math.Abs(sensorY - beaconY);

            MaxX = SensorX + Radius;
            MinX = SensorX - Radius;

            MaxY = SensorY + Radius;
            MinY = SensorY - Radius;

            LinearB1 = MaxY + SensorX;
            LinearB2 = SensorY - MaxX;
            LinearB3 = MinY + SensorX;
            LinearB4 = SensorY - MinX;

            SetCoverRanges();
        }

        private void SetCoverRanges()
        {
            CoverRanges = new List<Range>();

            int depth = 0;
            for (long y = SensorY; y <= SensorY + Radius; y++)
            {
                CoverRanges.Add(new Range(SensorX - Radius + depth, SensorX + Radius - depth, SensorY + depth));
                depth++;
            }

            depth = 0;
            for (long y = SensorY; y >= SensorY - Radius; y--)
            {
                CoverRanges.Add(new Range(SensorX - Radius + depth, SensorX + Radius - depth, SensorY - depth));
                depth++;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class Range
    {
        public long From { get; set; }
        public long To { get; set; }

        public long Y { get; set; }

        public Range(long from, long to, long y)
        {
            From = from;
            To = to;
            Y = y;
        }
    }


}
