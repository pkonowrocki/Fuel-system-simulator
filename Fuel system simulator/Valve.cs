using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuel_system_simulator
{
    public class Valve : Part
    {

        Pipeline Left;
        Pipeline Right;
        public enum position { BOTH, LEFT, RIGHT}
        public double diameter;
        public position state { get; set; }
        public double Valveposition() {
            return B;
        }

        private double B = 0.5, gotoB = 0.5;
        private void addLeft(Pipeline pipeline)
        {
            Left = pipeline;
        }
        private void addRight(Pipeline pipeline)
        {
            Right = pipeline;
        }
        public Valve(double D, Pipeline right, Pipeline left)
        {
            addLeft(left);
            addRight(right);
            diameter = D;
            flow = 0;
            pressure_out = 0;
            state = position.BOTH;
        }
        override public void update(double time )
        {
            if (Math.Abs(B-gotoB)> time/5)
            {
                B = B + ((gotoB - B) / Math.Abs(gotoB - B)) *time/ 5;
            }
            else
            {
                B = gotoB;
            }

            switch (state)
            {
                case position.BOTH:
                    gotoB = 0.5;
                    break;
                case position.LEFT:
                    gotoB = 0;
                    break;
                case position.RIGHT:
                    gotoB = 1;
                    break;
            }
            Right.flow =  B*flow;
            Left.flow = (1-B)*flow;
            pressure_out = (1-B)* Left.pressure_out + B * Right.pressure_out + Right.flow_energy() + Left.flow_energy() - Math.Pow(flow / (Math.PI * Math.Pow(diameter, 2) / 4), 2) / 2; ;
        }
    }
}
