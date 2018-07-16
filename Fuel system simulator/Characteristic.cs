using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuel_system_simulator
{
    struct point
    {
        public double x;//{ get; set; }
        public double y;// { get; set; }
        public double z;// { get; set; }
        public double t;
        public point(double a, double b=0, double c=0, double d=0)
        {
            x = a;
            y = b;
            z = c;
            t = d;
        }
    }

    class Characteristic
    {
        private System.IO.StreamReader file;
        public List<point> list = new List<point>();
        public Characteristic(string file_name)
        {
            file = new System.IO.StreamReader(file_name);
           
            string line;
            while ((line=file.ReadLine()) != null)
            {
                
                string[] splitted = line.Split('\t');
               
            
                if (splitted.Length == 2)
                {
                    list.Add(new point(double.Parse(splitted[0]), double.Parse(splitted[1])));
                }
                if (splitted.Length == 3)
                {
                    list.Add(new point(double.Parse(splitted[0]), double.Parse(splitted[1]), double.Parse(splitted[2])));
                }
                if (splitted.Length == 4)
                {
                    list.Add(new point(double.Parse(splitted[0]), double.Parse(splitted[1]), double.Parse(splitted[2]), double.Parse(splitted[3])));
                }

            }
           
        }
        public double get_yvalue(double x)
        {
            int index = find_clostest(x);
            if (index==0)
            {
                return list[index].y;
            }
            else
            {
                return (list[index].y - list[index - 1].y) * (x - list[index - 1].x) / (list[index].x - list[index - 1].x) + list[index - 1].y;
            }
           
        }
        public double get_zvalue(double x)
        {
            int index = find_clostest(x);
            if (index == 0)
            {
                return list[index].z;
            }
            else
            {
                return ((list[index].z - list[index - 1].z) * (x - list[index - 1].x) / (list[index].x - list[index - 1].x) + list[index - 1].z) ;
            }

        }
        public double get_tvalue(double x)
        {
            int index = find_clostest(x);
            if (index == 0)
            {
                return list[index].t ;
            }
            else
            {
                return ((list[index].t - list[index - 1].t) * (x - list[index - 1].x) / (list[index].x - list[index - 1].x) + list[index - 1].t) ;
            }

        }

        private int find_clostest(double x)
        {
            int index;
            if( (index=list.BinarySearch(new point(x), new pointComparer())) <0 )
            {
                return ~index;
            }
            else
            {
                return index;
            }
        }
    }

    class pointComparer : IComparer<point>
    {
        public int Compare(point x, point y)
        {
            return x.x.CompareTo(y.x);
        }
    }
}
