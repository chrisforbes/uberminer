using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uberminer
{
    // quick hack
    public class Vector3
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string  ToString()
        {
            return string.Format("{0}, {1}, {2}", X, Y, Z);
        }
    }
}
