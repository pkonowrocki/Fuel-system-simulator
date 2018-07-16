using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Windows;

namespace Fuel_system_simulator
{
    public class Pipeline : Part
    {

        double length;
        double diameter { get; }
        double eps = 0;
        public Pipeline(double L, double D, Part part, double _eps = 0)
        {
            input = part;
            length = L;
            diameter = D;
            flow = 0;
            pressure_out = 0;
            eps = _eps;
        }


        private double Re()
        {
            return 4 * density * flow / (viscosity*Math.PI*diameter);
        }

        private double fD()
        {
            if (Re() < 2000)
            {
                return 64 / Re();
            }
            else
            {
                double x = 1;
                double y = 0;
                while (Math.Abs(x-y)>0.0000001)
                {
                    y = x;
                    x = -2 * Math.Log10(eps/(3.7*diameter)+2.51 * y / Re() );
                }
                return 1 / (x * x);
            }
        }

        override public void update(double time )
        {
            input.flow = flow;
            pressure_out = input.pressure_out - fD() * length * 8 * density * Math.Pow(flow,2) / (Math.Pow(diameter, 5)*Math.Pow(Math.PI,2));
        }

        public double flow_energy()
        {
            return Math.Pow(flow / (Math.PI * Math.Pow(diameter, 2) / 4), 2) / 2 ;
        }

    }
}
