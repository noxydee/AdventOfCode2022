namespace AOC01.AOC_10_25.AOC16
{
    using AOC01.AOC_10_25.AOC16.FormView;

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
        private Brush FontBrush { get; set; }
        private Pen LinePen { get; set; }

        private int NodeRadius = 35;
        private int LineRadius = 3;

        public GraphVisualization(int xSize, int ySize, Color nodeColor, Color lineColor, List<Valve> valves) 
        {
            Bitmap = new Bitmap(xSize, ySize);
            NodeColor = nodeColor;
            LineColor = lineColor;
            Valves = valves;

            NodeBrush = new SolidBrush(NodeColor);
            LineBrush = new SolidBrush(LineColor);
            FontBrush = new SolidBrush(Color.White);
            LinePen = new Pen(LineBrush);
        }

        public void SaveGraphImage()
        {
            List<Point> points = GetPointsOnCircle(Bitmap.Size.Width / 2, Bitmap.Size.Height / 2, Convert.ToInt32(Bitmap.Size.Width / 2.5d), Valves.Count);

            Graphics graphics = Graphics.FromImage(Bitmap);

            for (int i = 0; i < Valves.Count; i++)
            {
                foreach (Valve connectedValve in Valves.ElementAt(i).ConnectedValves)
                {
                    int index = Valves.IndexOf(connectedValve);

                    graphics.DrawLine(LinePen, points[i], points[index]);
                }
            }

            for (int i = 0; i < Valves.Count; i++)
            {
                graphics.FillEllipse(NodeBrush, Convert.ToInt32(points[i].X-NodeRadius/2), Convert.ToInt32(points[i].Y-NodeRadius/2), NodeRadius, NodeRadius);
                graphics.DrawString(Valves[i].Name, SystemFonts.DefaultFont, FontBrush, points[i].X-10, points[i].Y-10);
                graphics.DrawString($"[{Valves[i].Rate}]", SystemFonts.DefaultFont, FontBrush, points[i].X-10, points[i].Y+5);
            }

            //Bitmap result = new Bitmap(Bitmap.Width, Bitmap.Height, graphics);
            Bitmap.Save("AOC16.bmp", ImageFormat.Bmp);
        }

        public void DrawOnForms(PaintEventArgs pe)
        {
            List<Point> points = GetPointsOnCircle(Bitmap.Size.Width / 2, Bitmap.Size.Height / 2, Convert.ToInt32(Bitmap.Size.Width / 2.5d), Valves.Count);

            using (Graphics graphics = pe.Graphics)
            {
                for (int i = 0; i < Valves.Count; i++)
                {
                    foreach (Valve connectedValve in Valves.ElementAt(i).ConnectedValves)
                    {
                        int index = Valves.IndexOf(connectedValve);

                        graphics.DrawLine(LinePen, points[i], points[index]);
                    }
                }

                for (int i = 0; i < Valves.Count; i++)
                {
                    graphics.FillEllipse(NodeBrush, Convert.ToInt32(points[i].X - NodeRadius / 2), Convert.ToInt32(points[i].Y - NodeRadius / 2), NodeRadius, NodeRadius);
                    graphics.DrawString(Valves[i].Name, SystemFonts.DefaultFont, FontBrush, points[i].X - 10, points[i].Y - 10);
                    graphics.DrawString($"[{Valves[i].Rate}]", SystemFonts.DefaultFont, FontBrush, points[i].X - 10, points[i].Y + 5);
                }

                graphics.Save();
            }
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
