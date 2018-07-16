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
using static Fuel_system_simulator.Valve;
using System.Windows.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Fuel_system_simulator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        bool state = false;
        public static double t = 0;
        Savetofile savetofile = new Savetofile();
        Settings settings = new Settings();
        public static Tank rightTank = new Tank();
        public static Pipeline rT2V = new Pipeline(1, 0.04, rightTank);
        public static Tank leftTank = new Tank();
        public static Pipeline lT2V = new Pipeline(1, 0.04, leftTank);
        public static Valve valve = new Valve(0.04, rT2V, lT2V);
        public static Pipeline v2p = new Pipeline(1, 0.04, valve);
        public static Pump pump = new Pump(v2p);
        public static Pipeline p2e = new Pipeline(1, 0.04, pump);
        public static Engine engine = new Engine(p2e);
        public static double dt = 0.04;
        public static double stoptime;
        public static bool simulate = true;
        Characteristic profile;
        public bool FileChecked { get; set; }
        

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ImgUpdate();
            GaugesUpdate();

        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (!state)
            {
                state = !state;
                Start.Content = "Pause";
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = TimeSpan.FromMilliseconds(dt*1000);
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

            if (t > stoptime && stoptime!=0)
            {
                Start_Click(this, e);
            }
            if (state)
            {
                try
                {
                    
                    t += dt;
                    if (FileChecked)
                    {
                        Speed.Value = profile.get_zvalue(t);
                        Pitch.Value = profile.get_yvalue(t);
                    }

                    if (simulate == true)
                    {
                        PartsUpdate(dt);
                        ImgUpdate();
                        GaugesUpdate();
                        
                    }

                    if (savetofile.SaveBool)
                        savetofile.Save();   
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source , MessageBoxButton.OK, MessageBoxImage.Error);
                    ResetMenu(sender, e);
                }
               
            }
        }
        private void PartsUpdate(double time)
        {
            engine.update(time);
            p2e.update(time);
            pump.update(time);
            v2p.update(time);
            valve.update(time);
            lT2V.update(time);
            rT2V.update(time);
            leftTank.update(time);
            rightTank.update(time);
            
        }
        private void ImgUpdate()
        {
            
                double RT = rightTank.fuel_gal() / 26 * 100;
                if (RT > 95) ImgRightTank.Source = (ImageSource)FindResource("RT100");
                else if (RT > 85) ImgRightTank.Source = (ImageSource)FindResource("RT90");
                else if (RT > 75) ImgRightTank.Source = (ImageSource)FindResource("RT80");
                else if (RT > 65) ImgRightTank.Source = (ImageSource)FindResource("RT70");
                else if (RT > 55) ImgRightTank.Source = (ImageSource)FindResource("RT60");
                else if (RT > 45) ImgRightTank.Source = (ImageSource)FindResource("RT50");
                else if (RT > 35) ImgRightTank.Source = (ImageSource)FindResource("RT40");
                else if (RT > 25) ImgRightTank.Source = (ImageSource)FindResource("RT30");
                else if (RT > 15) ImgRightTank.Source = (ImageSource)FindResource("RT20");
                else if (RT > 5) ImgRightTank.Source = (ImageSource)FindResource("RT10");
                else if (RT > 0) ImgRightTank.Source = (ImageSource)FindResource("RT0");

                double LT = leftTank.fuel_gal() / 26 * 100;
                if (LT > 95) ImgLeftTank.Source = (ImageSource)FindResource("LT100");
                else if (LT > 85) ImgLeftTank.Source = (ImageSource)FindResource("LT90");
                else if (LT > 75) ImgLeftTank.Source = (ImageSource)FindResource("LT80");
                else if (LT > 65) ImgLeftTank.Source = (ImageSource)FindResource("LT70");
                else if (LT > 55) ImgLeftTank.Source = (ImageSource)FindResource("LT60");
                else if (LT > 45) ImgLeftTank.Source = (ImageSource)FindResource("LT50");
                else if (LT > 35) ImgLeftTank.Source = (ImageSource)FindResource("LT40");
                else if (LT > 25) ImgLeftTank.Source = (ImageSource)FindResource("LT30");
                else if (LT > 15) ImgLeftTank.Source = (ImageSource)FindResource("LT20");
                else if (LT > 5) ImgLeftTank.Source = (ImageSource)FindResource("LT10");
                else if (LT > 0) ImgLeftTank.Source = (ImageSource)FindResource("LT0");
               //else if (LT <= 0) ImgLeftTank.Visible = false;

               double CP = p2e.flow_galh();
                int nr = (int)(t / dt) % 3;
                if (CP > 0) ImgCenterPipe.Source = (ImageSource)FindResource("CP" + nr.ToString());
               //if (CP <= 0) ImgCenterPipe.Visible = false;

               double RP = rT2V.flow_galh();
                if (RP > 0) ImgRightPipe.Source = (ImageSource)FindResource("RP" + nr.ToString());
               //if (CP <= 0) ImgCenterPipe.Visible = false;

               double LP = lT2V.flow_galh();
                if (LP > 0) ImgLeftPipe.Source = (ImageSource)FindResource("LP" + (2 - nr).ToString());
               //if (CP <= 0) ImgCenterPipe.Visible = false;
        }

        private void GaugesUpdate()
        {
            //GaugeSpeed.CurrentValue = engine.speed_knots();
            GaugeSpeed.CurrentValue = engine.TAS_knots(); //engine.speedTAS();
            GaugeFlow.CurrentValue = engine.flow_galh();
            GaugeFuelLeft.CurrentValue = leftTank.fuel_gal();
            GaugeFuelRight.CurrentValue = rightTank.fuel_gal();
            GaugeVerticalSpeed.CurrentValue = engine.speed_vert();
            GaugeAlt.CurrentValue = engine.height_ft();
        }

        private void Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
  
            Aircraft.speed = e.NewValue * 0.514444444;
            //engine.old_speed = e.OldValue * 0.514444444;

        }

        private void Pitch_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Aircraft.pitch = e.NewValue;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Speed.Value = Double.Parse(TextSpeed.Text);
                Pitch.Value = Double.Parse(TextROC.Text);
            }

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
                filecheck.Content = "Flight profile " + "impotred";

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            valve.state = position.LEFT;
            ImgValve.Source = (ImageSource)FindResource("VL");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            valve.state = position.BOTH;
            ImgValve.Source = (ImageSource)FindResource("VC");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            valve.state = position.RIGHT;
            ImgValve.Source = (ImageSource)FindResource("VR");
        }

        private void OpenMenuSave(object sender, RoutedEventArgs e)
        {
            
            savetofile.Hide();
            savetofile.Show();
            
        }

        

        private void ResetMenu(object sender, EventArgs e)
        {
            t=0;
            state = false;
            Start.Content = "Start";
            dispatcherTimer.Stop();
            rightTank = new Tank();
            rT2V = new Pipeline(1, 0.04, rightTank);
            leftTank = new Tank();
            lT2V = new Pipeline(1, 0.04, leftTank);
            valve = new Valve(0.04, rT2V, lT2V);
            v2p = new Pipeline(1, 0.04, valve);
            pump = new Pump(v2p);
            p2e = new Pipeline(1, 0.04, pump);
            engine = new Engine(p2e);
            ImgUpdate();
            GaugesUpdate();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            savetofile.Close();
            settings.Close();

        }

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            settings.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            while (t < 3600) { 
                if (t > stoptime && stoptime != 0)
                {
                    Start_Click(this, e);
                }
            if (state)
            {
                try
                {

                    t += dt;
                    if (FileChecked)
                    {
                        Speed.Value = profile.get_zvalue(t);
                        Pitch.Value = profile.get_yvalue(t);
                    }

                    if (simulate == true)
                    {
                        PartsUpdate(dt);
                        ImgUpdate();
                        GaugesUpdate();

                    }

                    if (savetofile.SaveBool)
                        savetofile.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                    ResetMenu(sender, e);
                }

            }
        }
        }
    }

}
