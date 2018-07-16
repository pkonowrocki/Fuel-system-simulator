using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Fuel_system_simulator.Aircraft;

namespace Fuel_system_simulator
{
    /// <summary>
    /// Logika interakcji dla klasy Settings.xaml
    /// </summary>
    public partial class Settings : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        //private double _new_dt = MainWindow.dt;
        public double new_dt { get; set; }
        public double new_stoptime { get; set; }
        public double new_AR { get; set; }
        public double new_e { get; set; }
        public bool simulate { get; set; }

        public Settings()
        {
            new_dt = MainWindow.dt;
            new_stoptime = 0;
            new_AR = AR;
            new_e = e;
            simulate = MainWindow.simulate;
            this.DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.dt = new_dt;
            AR = new_AR;
            Aircraft.e = new_e;
            if (new_stoptime > 0) MainWindow.stoptime=new_stoptime;
            MainWindow.simulate = simulate;
            this.Hide();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
