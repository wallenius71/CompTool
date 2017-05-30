using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindART
{
    public interface IAxis
    {
        double[] AxisValues { get; }
        double Incrementor { get;}
        AxisType AxisType { get;}
        
    }
}
