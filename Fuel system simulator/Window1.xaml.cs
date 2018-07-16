using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Symulator_systemu_paliwa.Valve;
using System.Windows.Threading;
using System.ComponentModel;

namespace Symulator_systemu_paliwa
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    public partial class ainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        bool state = false;
        double t = 0;
        static public ImageSource DisplayedImgRightTank { get; set; }
        static string a;

        static Tank rightTank = new Tank("kiedystododam.txt", 100000, a, "/img/Right tank");
        static Pipeline rT2V = new Pipeline(1, 0.04, rightTank);
        static Tank leftTank = new Tank("kiedystododam.txt", 100000, a, "/img/Left tank");
        static Pipeline lT2V = new Pipeline(1, 0.04, leftTank);
        static Valve valve = new Valve(0.04, rT2V, lT2V);
        static Pipeline v2p = new Pipeline(1, 0.04, valve);
        static Pump pump = new Pump(v2p);
        static Pipeline p2e = new Pipeline(1, 0.04, pump);
        static Engine engine = new Engine(p2e);
        Characteristic profile;
        Boolean ifchecked = false;

        public MainWindow()
        {
            InitializeComponent();
           


        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (!state)
            {
                state = !state;
                Aircraft.init();
                Start.Content = "Pause";
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(40);
                dispatcherTimer.Start();
                engine.setPump(pump);
                engine.addTank(rightTank);
                engine.addTank(leftTank);
            }
            else
            {
                state = !state;
                Start.Content = "Start";
                dispatcherTimer.Stop();
            }
            
            

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (state)
            {
                t+=0.01;
                if (ifchecked)
                {
                    Speed.Value = profile.get_zvalue(t);
                    Pitch.Value = profile.get_yvalue(t);
                }
                Part.time_graph.Enqueue(t);
                engine.update(0.01);
                p2e.update(0.01);
                pump.update(0.01);
                v2p.update(0.01);
                valve.update(0.01);
                lT2V.update(0.01);
                rT2V.update(0.01);
                leftTank.update(0.01);
                rightTank.update(0.01);   
            }
        }

       

       

        private void Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextSpeed.Text = e.NewValue.ToString();
            engine.speed = e.NewValue* 0.514444444;
            engine.old_speed = e.OldValue* 0.514444444;
            if (engine.speed >3.6 ) {
                if (Pitch.Value > 1.5*Math.Asin(3.6 / engine.speed) * 180 / Math.PI)
                    Pitch.Value = 1.5*Math.Asin(3.6 / engine.speed) * 180 / Math.PI;
                if (Pitch.Value < -1.5*Math.Asin(3.6 / engine.speed) * 180 / Math.PI)
                    Pitch.Value = -1.5*Math.Asin(3.6 / engine.speed) * 180 / Math.PI;
                Pitch.Maximum = 1.5*Math.Asin(3.6 / engine.speed) * 180 / Math.PI;
                Pitch.Minimum = -Pitch.Maximum;
            }
            else
            {
                Pitch.Maximum = 45;
                Pitch.Minimum = -Pitch.Maximum;
            }
            

        }

        private void Pitch_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextROC.Text = e.NewValue.ToString();
            engine.pitch = e.NewValue;
        }

       

        

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Speed.Value = Double.Parse(TextSpeed.Text);
                Pitch.Value = Double.Parse(TextROC.Text);
            }

        }

        
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            FileBtn.IsEnabled = true;
            ifchecked = true;
            
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            FileBtn.IsEnabled = false;
            ifchecked = false;
        }

        private void FileBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt)|*.txt";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                profile = new Characteristic(filename);
                filecheck.Content = "Flight profile " + filename;

            }
        }

        
    }
    
}
