using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Components.GPS
{
    public class Coordinates
    {
        public double Latitude { get; internal set; }

        public double Longitude { get; internal set; }

        public string Address { get; internal set; }
    }
}
