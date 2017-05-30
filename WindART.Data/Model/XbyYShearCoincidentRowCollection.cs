using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WindART.Properties ;

namespace WindART
{
    public class XbyYShearCoincidentRowCollection
    {
        SortedDictionary<double, int> _colindex;
        DataView _data;
        int _dateindex;

        public XbyYShearCoincidentRowCollection(int dateindex, SortedDictionary<double, int> wsindex, DataView data)
        {
            _data = data;
            _colindex = wsindex;
            _dateindex = dateindex;
        }

        public List<XbyYCoincidentRow> GetRows(int wdindex)
        {

            try
            {
                _data.Sort = Settings.Default.TimeStampName;
                DateTime t = DateTime.Now;
                string missing = Settings.Default.MissingValue.ToString();
                bool Skip;

                List<XbyYCoincidentRow> result = new List<XbyYCoincidentRow>();

                // fill if no elements are set to missing 
                double[] workarry = new double[_colindex.Count];
                int arridx = 0;
                double usewdVal;
                bool done = true;

                foreach (DataRowView row in _data)
                {
                    //Console.WriteLine(row["DateTime"]);
                    //Console.WriteLine("inside coincident data row iteration");
                    Skip = false;
                    arridx = 0;

                    //if none of these are less than zero then add to the coincident collection below 
                    //Console.WriteLine("ws columns stored in coincident val class:" + _colindex.Values.Count);
                    foreach (int i in _colindex.Values)
                    {
                        //if parse is successful pass the first check
                        if (double.TryParse(row[i].ToString(), out usewdVal))
                        {
                            //if value is ge 0 then pass second check 
                            workarry[arridx] = usewdVal;
                            if (workarry[arridx] < 0)
                            {
                                Skip = true;
                                break;
                            }
                        }
                        else
                        {                            
                            Skip = true;
                            break;
                        }
                        arridx++;
                    }

                    if (!Skip)
                    {

                        if (done)
                        {
                            Console.WriteLine("Date " + row[_dateindex] +
                                " lower " + row[_colindex[_colindex.Keys.Min()]]
                                + " upper " + row[_colindex[_colindex.Keys.Max()]]
                                + " wd " + row[wdindex].GetType());
                            done = false;
                        }

                        XbyYCoincidentRow thisRow = new XbyYCoincidentRow();
                        thisRow.Date = (DateTime)row[_dateindex];
                        thisRow.LowerWS = double.Parse(row[_colindex[_colindex.Keys.Min()]].ToString());
                        thisRow.UpperWS = double.Parse(row[_colindex[_colindex.Keys.Max()]].ToString());
                        thisRow.WD = double.Parse(row[wdindex].ToString());
                        result.Add(thisRow);

                        

                    }

                    

                }
                DateTime end = DateTime.Now;
                TimeSpan dur = end - t;
                Console.WriteLine("Coincident Values takes " + dur);
                Console.WriteLine("wd count: " + result.Count(c => c.WD >= 0));
                Console.WriteLine("upper ws count: " + result.Count(c => c.UpperWS >= 0));
                Console.WriteLine("lower ws count: " + result.Count(c => c.LowerWS >= 0));
                if (result.Count == 0)
                {
                    Console.WriteLine("No Coincident rows were found");
                    return default(List<XbyYCoincidentRow>);
                }
                else
                    return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<XbyYCoincidentRow> GetRows(int wdindex, int shearindex)
        {

            try
            {
                _data.Sort = Settings.Default.TimeStampName;
                DateTime t = DateTime.Now;
                string missing = Settings.Default.MissingValue.ToString();
                bool Skip;

                List<XbyYCoincidentRow> result = new List<XbyYCoincidentRow>();



                // fill if no elements are set to missing 
                double[] workarry = new double[_colindex.Count];
                int arridx = 0;
                foreach (DataRowView row in _data)
                {

                    Skip = false;
                    arridx = 0;
                    //if none of these are less than zero then add to the coincident collection below 
                    foreach (int i in _colindex.Values)
                    {
                        workarry[arridx] = (double)row[i];
                        if (workarry[arridx] < 0)
                        {
                            Skip = true;
                            break;
                        }
                        arridx++;
                    }
                    if (!Skip)
                    {


                        //Console.WriteLine("Coincident values: " + workarry [localcnt]);
                        XbyYCoincidentRow thisRow = new XbyYCoincidentRow();
                        thisRow.Date = DateTime.Parse(row[_dateindex].ToString());
                        thisRow.LowerWS = double.Parse (row[_colindex[_colindex.Keys.Min()]].ToString ());
                        thisRow.UpperWS = double.Parse(row[_colindex[_colindex.Keys.Max()]].ToString());
                        thisRow.WD = double.Parse(row[wdindex].ToString());
                        thisRow.Shear = double.Parse(row[shearindex].ToString());
                        result.Add(thisRow);


                    }


                }
                DateTime end = DateTime.Now;
                TimeSpan dur = end - t;
                Console.WriteLine("Coincident Values takes " + dur);
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        
    }
}