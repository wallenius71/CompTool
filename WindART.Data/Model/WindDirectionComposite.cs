using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WindART.Properties;

namespace WindART
{
    public class WindDirectionComposite:IComposite
    {
        private DataTable _data;
        private ISessionColumnCollection _columns;

        public delegate void ProgressEventHandler(string msg);
        public event ProgressEventHandler NewCompositeColumnAdded;
        public event ProgressEventHandler DeterminingWindDirectionCompositeValues;
        public event ProgressEventHandler CompletedWindDirectionCompositeValues;

        public WindDirectionComposite(ISessionColumnCollection columns, DataTable data)
        {
            _data = data;
            _columns = columns;
        }
        public bool CalculateComposites()
        {
            try
            {
                //**** get list 
                List<ISessionColumn> initialColList = _columns.GetColumnsByType(SessionColumnType.WDAvg);
                if (initialColList.Count == 0)
                {
                    Console.WriteLine("no cols found");
                    return false;
                }
                //add the columns to the datatable to be populated. pass the list of existing wd cols because 
                //if one wd has more child columns than the other they all must be created for the code to 
                //execue without error 

                new WindDirectionCompColumns(_columns, _data).Add(initialColList);
                if(NewCompositeColumnAdded !=null)
                NewCompositeColumnAdded("Wind Direction Composite Column Added");

                string CompColName = Settings.Default.CompColName;
                double missingVal = Settings.Default.MissingValue;

                string compColPrefix = string.Empty;
                string ChildColPrefix = string.Empty;
                SortedDictionary<double, ISessionColumn> LocalWdCols;
                double outPutCatch;
                double childOutputVal;
                DataView view = _data.AsDataView();
                view.Sort = _columns[_columns.DateIndex].ColName + " asc";

                Console.WriteLine(_columns[_columns.DateIndex].ColName + " asc");
                if(DeterminingWindDirectionCompositeValues !=null)
                DeterminingWindDirectionCompositeValues("Assigning Wind Direction Composite Values");
                foreach (DataRowView row in view)
                {
                    //Console.WriteLine("date " + DateTime.FromOADate (double.Parse(row[_columns.DateIndex].ToString())));

                    //get list of columns sorted desc by height at this date
                     LocalWdCols= _columns.GetColumnsByType(SessionColumnType.WDAvg, (DateTime)row[_columns.DateIndex]);

                     //set the wd comp column to the first valid value from heighest ht to lowest
                     //Console.WriteLine("WD Columns found " + LocalWdCols.Count);
                    row.BeginEdit();
                    foreach (KeyValuePair<double, ISessionColumn> kv in LocalWdCols.Reverse())
                    {   
                        compColPrefix = Enum.GetName(typeof(SessionColumnType), kv.Value.ColumnType);
                        
                        //evaluate parent column 
                        //if it is not missing and evaluates to a double use the value
                        if (double.TryParse(row[kv.Value.ColIndex].ToString(), out outPutCatch) )
                            
                        {
                            if (outPutCatch >= 0)
                            {

                                //set parent comp 
                                
                                row[compColPrefix + CompColName] = outPutCatch;

                                //assign child column values  of the selected parent column to the comp child column  
                                foreach (ISessionColumn child in kv.Value.ChildColumns)
                                {
                                    ChildColPrefix = Enum.GetName(typeof(SessionColumnType), child.ColumnType);
                                    if (double.TryParse(row[child.ColIndex].ToString(), out childOutputVal))
                                    {
                                        row[ChildColPrefix + CompColName] = childOutputVal;
                                    }
                                    else
                                    {
                                        row[ChildColPrefix + CompColName] = missingVal;
                                    }
                                }
                                break;
                            }
                        }

                        
                        //nothing valid found set parent and child comp column values  to missing 
                        row[compColPrefix + CompColName] = missingVal;
                        foreach (ISessionColumn child in kv.Value.ChildColumns)
                        {
                            //Console.WriteLine("System setting wd comp to missing; thinks lower wd val is " + outPutCatch);
                            string prefix = Enum.GetName(typeof(SessionColumnType), child.ColumnType);
                            row[prefix + CompColName] = missingVal;
                        }
                        
                    }
                    row.EndEdit();
                }
                _data.AcceptChanges();
                if(CompletedWindDirectionCompositeValues !=null)
                CompletedWindDirectionCompositeValues("Completed Generating Wind Direction Composites");
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
            
                      
        }
       
        
    }
}
