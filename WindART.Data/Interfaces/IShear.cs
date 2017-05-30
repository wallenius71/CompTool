using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindART
{
    public interface IShear
    {
       //calculates sheared up wind speed value and returns 
        bool CalculateWindSpeed(double shearht,out ShearCalculationGridCollection grid);
    }
}
