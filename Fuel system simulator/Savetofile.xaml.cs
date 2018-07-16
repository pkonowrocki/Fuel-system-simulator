using System;
using System.Collections.Generic;
using System.IO;
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


namespace Fuel_system_simulator
{
    /// <summary>
    /// Logika interakcji dla klasy Savetofile.xaml
    /// </summary>
    public partial class Savetofile : Window 
    {
        string filename;
        StreamWriter file;
        public Boolean SaveBool = false;
        public delegate double Del();
        List<CheckBox> list = new List<CheckBox>();
        Dictionary<CheckBox, Del> dictionary = new Dictionary<CheckBox, Del>();
        Boolean title = true;

        public Savetofile()
        {
            InitializeComponent();
            dictionary.Add(AircraftHeightCheck, new Del(MainWindow.engine.height_ft));
            dictionary.Add(TASCheck, new Del(MainWindow.engine.CAStoTAS));
            dictionary.Add(AtmPressureCheck, new Del(Aircraft.pressure));
            dictionary.Add(AircraftWeightCheck, new Del(MainWindow.engine.OverallMass));

            dictionary.Add(EngineFlowCheck, new Del(MainWindow.engine.flow_galh));
            dictionary.Add(EnginePressureCheck, new Del(MainWindow.engine.pressure_kPa));
            dictionary.Add(EnginePowerCheck, new Del(MainWindow.engine.power_kW));

            dictionary.Add(PumpFlowCheck, new Del(MainWindow.pump.flow_galh));
            dictionary.Add(PumpPressureCheck, new Del(MainWindow.pump.pressure_kPa));
            dictionary.Add(PumpPowerCheck, new Del(MainWindow.pump.power_kW));

            dictionary.Add(ValveFlowCheck, new Del(MainWindow.valve.flow_galh));
            dictionary.Add(ValvePressureCheck, new Del(MainWindow.valve.pressure_kPa));
            dictionary.Add(ValvePositionCheck, new Del(MainWindow.valve.Valveposition));

            dictionary.Add(LeftTankFlowCheck, new Del(MainWindow.leftTank.flow_galh));
            dictionary.Add(LeftTankPressureCheck, new Del(MainWindow.leftTank.pressure_kPa));
            dictionary.Add(LeftTankWeightCheck, new Del(MainWindow.leftTank.mass_lbs));

            dictionary.Add(RightTankFlowCheck, new Del(MainWindow.rightTank.flow_galh));
            dictionary.Add(RightTankPressureCheck, new Del(MainWindow.rightTank.pressure_kPa));
            dictionary.Add(RightTankWeightCheck, new Del(MainWindow.rightTank.mass_lbs));
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter = "CSV Files (*.csv)|*.csv";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                this.filename = dlg.FileName;
                file = new StreamWriter(filename);
                

            }
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            SaveBool = !SaveBool;
            SaveBtn.IsEnabled = SaveBool;
        }

        public async void Save()
        {
            string line = "";
            if (title)
            {
                await file.WriteAsync("\"Time\", ");
                foreach (KeyValuePair<CheckBox, Del> entry in dictionary)
                {

                    if ((bool)entry.Key.IsChecked) line = line + " \"" + entry.Key.Content + "\",";  
                }
                line = line.Remove(line.Length - 1);
                await file.WriteLineAsync(line);
                file.Flush();
                line = "";
                title = false;
            }

            await file.WriteAsync("\"" + MainWindow.t + "\", ");
            foreach (KeyValuePair<CheckBox, Del> entry in dictionary)
            {
                if ((bool)entry.Key.IsChecked) line = line + " \"" + entry.Value.Invoke() + "\",";
            }
            line = line.Remove(line.Length - 1);
            await file.WriteLineAsync(line);
            file.Flush();
            line = "";
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
