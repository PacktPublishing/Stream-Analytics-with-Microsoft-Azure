using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendPhoneSensorDataToAzure
{
    class Accelerometer
    {
        public string ID { get; set; }

        public string Coordinate_X { get; set; }

        public string Coordinate_Y { get; set; }

        public string Coordinate_Z { get; set; }
    }
}
