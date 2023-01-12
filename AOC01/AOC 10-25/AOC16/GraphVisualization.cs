namespace AOC01.AOC_10_25.AOC16
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;

    internal class GraphVisualization
    {
        private Bitmap Bitmap { get; set; }
        private Color NodeColor { get; set; }
        private Color LineColor { get; set; }
        private List<Valve> Valves { get; set; }

        private Brush NodeBrush { get; set; }
        private Brush LineBrush { get; set; }

        private int NodeRadius = 100;
        private int LineRadius = 3;

        public GraphVisualization(int xSize, int ySize, Color nodeColor, Color lineColor, List<Valve> valves) 
        {
            Bitmap = new Bitmap(xSize, ySize);
            NodeColor = nodeColor;
            LineColor = lineColor;
            Valves = valves;

            NodeBrush = new SolidBrush(NodeColor);
            LineBrush = new SolidBrush(LineColor);
        }

        public void SaveGraphImage()
        {
            List<Point> points = GetPointsOnCircle(Bitmap.Size.Width / 2, Bitmap.Size.Height / 2, Bitmap.Size.Width / 4, Valves.Count);

            Graphics graphics = Graphics.FromImage(Bitmap);

            foreach (Point point in points)
            {
                graphics.FillEllipse(NodeBrush, point.X, point.Y, NodeRadius, NodeRadius);
            }

            Bitmap result = new Bitmap(Bitmap.Width, Bitmap.Height, graphics);
            result.Save("AOC16.bmp", ImageFormat.Bmp);
        }

        private List<Point> GetPointsOnCircle(int x, int y, int radius, int numerOfPoints)
        {
            List<Point> points = new List<Point>();

            for (int i = 0; i < numerOfPoints; i++)
            {
                double angle = 2 * Math.PI * i / numerOfPoints;

                points.Add(new Point(Convert.ToInt32(x + radius * Math.Cos(angle)), Convert.ToInt32(y + radius * Math.Sin(angle))));
            }

            return points;
        }
    }
}
