using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WindART.Properties;

namespace WindART
{
    class CoincidentValues:ICoincidentValues 
    {
        DataView _data;
        List<int> _colindex;

        public CoincidentValues(List<int> colindex, DataView data)
        {
            _colindex = colindex;
            _data = data;
        }
        public CoincidentValues(List<int> colindex, DataTable data)
        {
            _colindex = colindex;
            _data = data.AsDataView ();
        }
        public Dictionary<int,List<double>> GetValues()
        {
            //filter by values
            try
            {
                _data.Sort = Settings.Default.TimeStampName;
                DateTime t = DateTime.Now;
                string missing = Settings.Default.MissingValue.ToString();
                bool Skip;
                
               Dictionary<int,List<double>> result = new Dictionary<int, List<double>>();

               //initialize output lists
               foreach (int i in _colindex)
               {
                   List<double> thisList = new List<double>();
                   result.Add(i, thisList);
                  
                   Console.WriteLine("initialized dictionary entry key is " + i);
               }

                // fill if no elements are set to missing 
                
                
                double[] workarry=new double[_colindex.Count ];
                int arridx = 0;
                Console.WriteLine(" dataset count before coincident eval " + _data.Count);
                foreach(DataRowView row in _data)
                {
                    
                    Skip = false;
                    arridx = 0;
                    double usewdVal;
                    foreach(int i in _colindex)
                    {
                        if (double.TryParse(row[i].ToString(), out usewdVal))
                        {
                            workarry[arridx] = usewdVal ;
                            if (workarry[arridx] < 0)
                            {
                                //Console.WriteLine(row[0] + " idx " + i + " bad value " + workarry[arridx]);
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
                        int localcnt = 0;
                        foreach(int i in _colindex )
                        {
                            //Console.WriteLine("Coincident values: " + workarry [localcnt]);
                            result[i].Add(workarry[localcnt ]);
                            localcnt++;
                            
                        }
                        
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
