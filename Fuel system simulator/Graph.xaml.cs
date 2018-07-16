using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Symulator_systemu_paliwa
{
    /// <summary>
    /// Logika interakcji dla klasy Graph.xaml
    /// </summary>
    public partial class Graph : Window
    {
        public Queue<double> values = new Queue<double>();
        public Graph()
        {
            InitializeComponent();
        }

        public void draw()
        {
            linegraph.Plot((Part.time_graph).Skip(Part.time_graph.Count - values.Count ),values);
        }

        private void GraphForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
