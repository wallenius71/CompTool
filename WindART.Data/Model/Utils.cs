using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace WindART
{
    [Serializable ]
    public enum DataSetDateStatus
    {
        NotSet=0,
        Found=1,
        FoundMultiple=2,
        FoundNone=3
    }
    public enum AxisFactoryType
    {
        DateTime=0,
        WindDirection=1
    }
    [Serializable ]
    public enum SessionColumnType
    {
        Select=0,
        WSAvg=70,
        WSMin=1,
        WSMax=2,
        WSStd=3,
        WSAvgBulkShear=4,
        WSAvgShear=5,
        WSAvgSingleAxisShear=6,
        Alpha=7,
        WDAvg=10,
        WDMax=11,
        WDMin=12,
        WDStd=13,
        TempAvg=20,
        TempMin=21,
        TempMax=22,
        TempStd=23,
        BPAvg=30,
        BPMin=31,
        BPMax=32,
        BPStd=33,
        BVAvg=40,
        BVMin=41,
        BVMax=42,
        BVStd=43,
        RHAvg=50,
        RHMin=51,
        RHMax=52,
        RHStd=53,
        AirDenAvg=71,
        DateTime=60
        
    }
    public enum DatePartType
    {
        hour = 0,
        minute = 1,
        second = 2,
        hourminute = 3,
        julianday = 4,
        year = 5,
        month = 6,
        milliseconds = 7,
        monthdayyear = 8,
        hourminutesecond = 9
    }
    public enum InSector
    {
        //where a wd value falls in the sensors area 
        Not_Shadowed,
        Shadowed,
        Neither

    }
    public enum CircleDirection
    {
        Clockwise,
        CounterClockwise
    }
    public enum AxisType
    {
        None=0,
        Month=1,
        Hour=2,
        WD=3,
        WS=4, 
        MonthYear=5
    }
    public enum SummaryType
    {
        Month = 1,
        Hour = 2,
        WD = 3,
        WS = 4,
        WDRose=5,
        DataRecovery=6,
        XbyYShear=7,
        SingleAxisShear=8
        
    }
    public enum StationSummarySheetType
    {
        ShearGrid,
        MonthHourSheet,
        WD_WSSheet,
        WindRoseSheet,
        DataRecovery


    }

    public static  class Utils
    {
        public static string GetFolder()
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();

            fd.Description = "Select Folder";
            DialogResult result = fd.ShowDialog();

            if (result == DialogResult.OK)
            {
                return fd.SelectedPath;
            }
            else
            {

                return "";
            }

        }
        public static string GetFile()
        {
            OpenFileDialog fd = new OpenFileDialog();

            fd.Title = "Select File";
            fd.ShowDialog();
            return fd.FileName;
        }
        public static bool AddColtoDataTable<T>(string AddColName, List<T> valArray, DataTable data)
        {
            //add a column of type T to datatable and return true if successful

            try
            {
                //add the new column if it does not exist 

                if (!data.Columns.Contains(AddColName))
                {
                    DataColumn newCol = new DataColumn();
                    newCol.DataType = typeof(T);
                    newCol.ColumnName = AddColName;
                    data.Columns.Add(newCol);

                    int i = 0;
                    foreach (DataRow row in data.Rows)
                    {
                        row.BeginEdit();
                        row[AddColName] = valArray[i];
                        i++;
                    }
                    data.AcceptChanges();
                }
            }

            catch (Exception E)
            {
                MessageBox.Show(E.Message);
                return false;
            }
            return true;

        }
        public static List<T> ExtractDataTableColumn<T>(string ColName, DataTable ParamDT)
        {
            try
            {
                //return a list of values of type determined at run time 
                //collected from a the session datatable 
                List<T> list = new List<T>();
                //Console.WriteLine("DT rows=" + ParamDT.Rows.Count.ToString());
                int RowCount = ParamDT.Rows.Count;

                for (int i = 0; i < RowCount; i++)
                {
                    T temp;
                    if (ParamDT.Rows[i][ColName].GetType().ToString() == "System.DBNull")
                    {
                        temp = default(T);
                    }
                    else
                    {
                        temp = (T)Convert.ChangeType(ParamDT.Rows[i][ColName].ToString(), typeof(T));
                    }
                    list.Add(temp);
                }
                //Console.WriteLine("Sensor Val Count=" + list.Count.ToString());
                return list;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public static double SectorRange(double SectValA, double SectValB, CircleDirection Dir)
        {

            //returns the difference in degrees between 2 points on a circle
            //specify the direction 
            try
            {
                double retVal = 0;
                switch (Dir)
                {
                    case CircleDirection.Clockwise:

                        if (SectValB - SectValA < 0)
                        {
                            retVal = (360 - SectValA) + SectValB;
                        }

                        else
                        {
                            retVal = SectValB - SectValA;
                        }
                        break;

                    case CircleDirection.CounterClockwise:

                        if (SectValA - SectValB < 0)
                        {
                            retVal = (360 - SectValA) + SectValB;
                        }
                        else
                            retVal = SectValA - SectValB;

                        break;
                    default:
                        retVal = 0;
                        break;
                }


                return retVal;
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
                return 0;
            }



        }
        public static int TimeSpanMonths(DateTime startdate, DateTime enddate)
        {
            int months = (13 - startdate.Month) + (((enddate.Year-1) - (startdate.Year))  * 12) + (enddate.Month);
            return months;
        }
        
    }
}
