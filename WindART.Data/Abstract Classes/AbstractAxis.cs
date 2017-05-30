using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindART
{
    public abstract class AbstractAxis:IAxis
    {
        protected  double[] _axisValues;
        protected AxisType _axisType;

        public virtual double[] AxisValues
        {
            get { return _axisValues; }
        }
        public abstract double Incrementor
        {
            get;
        }

        public virtual AxisType AxisType
        {
            get { return _axisType; }
        }

        protected abstract void setAxisValues();

        
        
    }
}
