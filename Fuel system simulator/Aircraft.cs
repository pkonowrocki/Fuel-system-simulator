using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuel_system_simulator
{
    static public class Aircraft
    {
        public static double AR = 7.45;
        public static double e = 0.84;
        public static double S = 16.165122;
        public static double g = 9.80665;// m/s2
        public static double M = 711.232836;
        public static double Cd0 = 0.0341;
        public static double speed = 0, pitch=0 , height = 0;
        public static double T0 = 288.15, p0 = 101325, ro0 = 1.225, a0 = 340.294;
        public static double k = 1.4, R = 287.05287, Bt = -0.0065;



        public static double pressure()
        {
            return p0*Math.Pow(temperature()/T0,g/(Bt*R));
        }

        public static double temperature()
        {
            return T0+height*Bt;
        }

        public static double air_density()
        {
            return pressure() / (R * temperature());
        }
    }
}
