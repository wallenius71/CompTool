using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindART
{
    public abstract class AbstractCreateGridAlgorithm:ICreateGridAlgorithm 
    {
        protected List<XbyYCoincidentRow> _filtereddata;
        protected double _upperwsht;
        protected double _lowerwsht;
        protected double _shearht;
        protected IAxis _axis;
        protected SessionColumnType _sheartype;


        public virtual List<List<SummaryGridColumn>> CreateGrid()
        {
            
                List<List<SummaryGridColumn>> workgrid = new List<List<SummaryGridColumn>>(_axis.AxisValues.Length);
                for (int i = 0; i < _axis.AxisValues.Length+1; i++)
                {
                    //create a row and add it to the grid

                    List<SummaryGridColumn> row = new List<SummaryGridColumn>();
                    string axisheader = " bin ";
                    if (_axis.AxisType == AxisType.WD || _axis.AxisType == AxisType.WS)
                    {
                        axisheader = " bin " + _axis.Incrementor + " deg";
                    }
                    

                    //column headings
                    if (i == 0)
                    {
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn(_axis.AxisType  + axisheader )));
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn("Count")));
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn("Freq")));
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn("Average of " + _upperwsht + "m WS Comp")));
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn("Average of  " + _lowerwsht + "m WS Comp")));
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn("Average of " + _shearht + "m " + _sheartype)));
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn("Alpha (" + _lowerwsht + "m to " + _upperwsht + "m)")));
                        row.Add(new SummaryGridColumn(new ExplicitValueGridColumn("Alpha " + _upperwsht + "m to " + _shearht + "m)")));
                    }
                    else
                    {
                        //filter data for this axis element
                        List<XbyYCoincidentRow> thisRowsValues = new List<XbyYCoincidentRow>();
                        thisRowsValues = FilterRows(i-1);
                        //fill ws lists
                        List<double> upperWS = new List<double>(_filtereddata.Count);
                        List<double> lowerWS = new List<double>(_filtereddata.Count);
                        List<double> shear = new List<double>(_filtereddata.Count);
                        foreach (XbyYCoincidentRow r in thisRowsValues)
                        {
                            upperWS.Add(r.UpperWS);
                            lowerWS.Add(r.LowerWS);
                            shear.Add(r.Shear);
                        }
                        //left axis
                        row.Add(new SummaryGridColumn(new AxisValueGridColumn(_axis, i-1)));
                        // count
                        row.Add(new SummaryGridColumn(new CountGridColumn(thisRowsValues)));
                        //frequency
                        row.Add(new SummaryGridColumn(new FrequencyGridColumn(thisRowsValues.Count, _filtereddata.Count)));
                        //upper ws 
                        row.Add(new SummaryGridColumn(new AverageGridColumn(upperWS)));
                        //lower ws
                        row.Add(new SummaryGridColumn(new AverageGridColumn(lowerWS)));
                        //sheared ws 
                        row.Add(new SummaryGridColumn(new AverageGridColumn(shear)));
                        //lower alpha
                        row.Add(new SummaryGridColumn(new AlphaGridColumn(upperWS.Average(), lowerWS.Average(), 
                            _upperwsht, _lowerwsht)));
                        //"upper alpha
                        row.Add(new SummaryGridColumn(new AlphaGridColumn(shear.Average(), upperWS.Average(),
                            _shearht, _upperwsht)));
                    

                    
                    upperWS.Clear();
                    lowerWS.Clear();
                    shear.Clear();
                    }
                    workgrid.Add(row);
                }
            return workgrid;
                
            
        }
        protected abstract List<XbyYCoincidentRow> FilterRows(int axiselement);

        
    }
}
