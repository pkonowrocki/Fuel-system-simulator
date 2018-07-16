using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuel_system_simulator
{
    class FooException : Exception
    {
        public FooException(string message): base(message) { }
        
    }
}
