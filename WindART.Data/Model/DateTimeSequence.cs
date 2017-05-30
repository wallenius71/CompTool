using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WindART.Properties;

namespace WindART
{
    public class DateTimeSequence:IDateTimeSequence 
    {
        private DataTable _data;
        private int _dateIndex;
        public DateTimeSequence(DataTable data,int dateindex)
        {
            _data = data;
            _dateIndex = dateindex;
        }

        public TimeSpan DetectInterval()
        {
            try
            {
                DataView view = _data.AsDataView();
                view.Sort = Settings.Default.TimeStampName + " asc";
                //count each interval occurence
                TimeSpan gap=default(TimeSpan);
                DateTime prevVal=default(DateTime);
                DateTime curVal=default(DateTime);
                DataTableReader reader = new DataTableReader(_data);
                Dictionary<TimeSpan, int> resultCount = new Dictionary<TimeSpan, int>();

                reader.NextResult();
                //create table of counts grouped by gap
                while (reader.Read())
                {
                    curVal = (DateTime)reader[_dateIndex];
                    if (prevVal != default(DateTime))
                    {
                        gap = curVal - prevVal;
                        if (resultCount.ContainsKey(gap))
                        {
                            resultCount[gap] += 1;
                        }
                        else
                        {
                            resultCount.Add(gap, 1);
                        }
                    }
                    prevVal = curVal;
                }
                
                //determine the gap with the greatest count
                int workval=0;
                int maxval=0;
                TimeSpan greatest=default (TimeSpan );
                foreach (KeyValuePair<TimeSpan, int> kv in resultCount)
                {
                    
                    workval=kv.Value ;
                    if (workval > maxval)
                    {
                        maxval = workval;
                        greatest = kv.Key;
                    }
                }
                return greatest;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<DateTime> GetMissingTimeStamps()
        {
            try
            {
                List<DateTime> existing = GetExistingSequence ();
                List<DateTime> expected = GetExpectedSequence ();
                
                
                var result = expected.Except(existing);
                return result.ToList();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DateTime> GetExistingSequence()
        {
            try
            {
                DataView view = new DataView(_data);
                view.Sort = Settings.Default.TimeStampName + " asc";
                List<DateTime> result = new List<DateTime>();
                foreach (DataRowView rowview in view)
                {
                    result.Add((DateTime)rowview[_dateIndex]);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<DateTime> GetExpectedSequence()
        {
            try
            {
                TimeSpan interval = DetectInterval();
                DateTime first = default(DateTime);
                DateTime last = default(DateTime);
                DataView view = new DataView(_data);
                List<DateTime> result = new List<DateTime>();
                view.Sort = Settings.Default.TimeStampName + " asc";

                for (int i = 0; i < view.Count; i++)
                {
                    if (DateTime.TryParse(view[i][_dateIndex].ToString (), out first))
                    { break; }
                }
                last = (DateTime)view[view.Count - 1][_dateIndex];

                DateTime workdate = first;
                result.Add(workdate);
                while (workdate <= last)
                {
                    workdate = workdate + interval;
                    result.Add(workdate);
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}
