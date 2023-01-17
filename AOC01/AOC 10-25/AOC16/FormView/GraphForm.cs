namespace AOC01.AOC_10_25.AOC16.FormView
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class GraphForm : Form
    {
        public List<Valve> Valves { get; set; }

        public GraphForm(List<Valve> valves)
        {
            InitializeComponent();
            Valves = valves;
        }
    }
}
