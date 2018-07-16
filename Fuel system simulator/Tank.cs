using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Fuel_system_simulator
{
    public class Tank : Part
    {

        public double mass;
        double h, A;

        public Tank()
        {
            A = 0.14384 / 2;
            h = 1;
            //h = 0.5;
        }
        public double mass_lbs()
        {
            return mass * 2.20462262;
        }
        override public void update(double time )
        {
            pressure_out = Aircraft.pressure() + density * Aircraft.g * h ;
            h = (A * h - flow * time) / A;
            mass = A * h * density;
        }
        public double fuel_gal()
        {
            return 26 * h;
        }
       
    }
}
