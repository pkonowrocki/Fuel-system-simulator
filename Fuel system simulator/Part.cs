using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fuel_system_simulator
{
    public class Part
    {
        public Part input { set; get; }
        protected static double density = 804; //km/m3
        protected static double viscosity = 0.000002;
        public double pressure_out{ set; get; } //Pa
        public double flow { set; get; } //m3/s

        public double flow_galh()
        {
            return 0.264172052 * 60 * 60000 * flow;
        }

        public  double pressure_kPa()
        {
            return pressure_out/1000;
        }

        public void cavitation_check()
        {
            if (pressure_out < 0) throw new FooException("W systemie nastapil efekt kawitacji");
        }

        virtual public void update(double time=0.4)
        {
            input.flow = flow;
            pressure_out = input.pressure_out;
        }

    }
}
