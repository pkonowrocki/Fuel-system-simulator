using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Fuel_system_simulator
{
    public class Engine : Part
    {





        static List<Tank> tanks= new List<Tank>();
        Pump pump;
        public double old_speed;
        double power, heat_c = 42800000, uu = 0.3, old_overall_mass = Aircraft.M;
        double flow_before;

        public void addTank(Tank tank)
        {
            tanks.Add(tank);
        }
        public void setPump(Pump pump)
        {
            this.pump = pump;

        }

        public Engine(Part part)
        {

            input = part;
            power = 0;
            flow = 0;
            pressure_out = 0;
        }
        public double TAS_knots()
        {
            return CAStoTAS() * 1.94384449;
        }


        public double speed_knots()
        {
            return Aircraft.speed * 1.94384449;
        }


        public double height_ft()
        {
            return Aircraft.height * 3.2808399/100;
        }

        public double CAStoTAS()
        {
            double u = (Aircraft.k - 1) / Aircraft.k;
            double q = 1 + u * Aircraft.ro0 * Aircraft.speed * Aircraft.speed / (2 * Aircraft.p0);
            q = Math.Pow(q, 1.0 / u);
            q = q - 1;
            q = 1 + q * Aircraft.p0 / Aircraft.pressure();
            q = Math.Pow(q, u);
            q = q - 1;
            q = q * 2 * Aircraft.pressure() / (Aircraft.air_density() * u);
            return Math.Sqrt(q);
            
            //return Math.Sqrt( (2 * Aircraft.pressure()) / (u * Aircraft.air_density()) * ( Math.Pow(1 + Aircraft.p0 * (Math.Pow(1 + u * Aircraft.ro0 * Aircraft.speed * Aircraft.speed / (1 * Aircraft.p0), 1 / u) - 1) / Aircraft.pressure(), u) - 1)  );

        }
        public double power_kW()
        {
            return power / 1000;
        }
        public double OverallMass()
        {
            double overall_mass = 0;
            foreach (Tank atank in tanks)
            {
                overall_mass =overall_mass+atank.mass;
            }
            return overall_mass + Aircraft.M;
        }

        public double power_req(double time=0.4)
        {
            
            

            double first=0, second=0, Ep=0, Ek=0;

            

                
                double speedTAS = CAStoTAS();
                double overall_mass = OverallMass();
                
                Ep = overall_mass * Aircraft.g * speedTAS * Math.Sin(Aircraft.pitch * Math.PI / 180);

                Ek = 0.5* (overall_mass * speedTAS * speedTAS - old_overall_mass * old_speed * old_speed) / time; 

                old_overall_mass = overall_mass;

                old_speed = speedTAS;

                first = 0.5 * Aircraft.air_density() * Math.Pow(speedTAS, 3) * Aircraft.S * Aircraft.Cd0;
                
                if (Aircraft.speed != 0) 
                    second = Math.Pow(overall_mass * Aircraft.g, 2) / (0.5 * Aircraft.air_density() * speedTAS * Aircraft.S * Math.PI * Aircraft.e * Aircraft.AR);


            if (first + second + Ep + Ek < 0) return 0;
            
            return power = first + second + Ep + Ek;
        }

        public void iter(double time)
        {
            flow_before = -1;
            int i = 0;
            while (Math.Abs(flow - flow_before) > 1e-1)
            {
                i++;
                flow_before = flow;
                flow = (power_req(time) + pump.power_kW()*1000) / (uu*density * heat_c );
                pump.iter(flow);
            }
        }

        public override void update(double time)
        {
            iter(time);
            input.flow = flow;
            pressure_out = input.pressure_out;
            Aircraft.height = Aircraft.height + time* Aircraft.speed * Math.Sin(Aircraft.pitch * Math.PI / 180);
        }

        public double speed_vert()
        {
            return Aircraft.speed * Math.Sin(Aircraft.pitch * Math.PI / 180) * 196.850394 / 100;
        }





    }
}
