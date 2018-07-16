using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fuel_system_simulator
{
    public class Pump :Part
    {
        double power { set; get; } //W
        Characteristic QpPu = new Characteristic("pump.txt");
        public Pump(Part part)
        {
            input = part;
            flow = 0;
            pressure_out = 0;
            power = 0;
        }

        override public void update(double time )
        {
            input.flow = flow;
            pressure_out = input.pressure_out + QpPu.get_zvalue(flow);

        }
        public double power_kW()
        {
            return power / 1000;
        }
        public void iter(double x)
        {
            power = 5*QpPu.get_yvalue(x);
        }


        

    }
}
