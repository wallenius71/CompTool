﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WindART.Properties ;
using System.ComponentModel;
using System.Collections.ObjectModel ;
using System.Collections.Specialized;

namespace WindART
{
    [Serializable]
    public class SessionColumnCollection:ISessionColumnCollection,INotifyPropertyChanged,INotifyCollectionChanged 
    {
        [NonSerialized ]
        private DataTable _data;
        private DataSetDateStatus _dateStatus;
        private bool _duplicatesExist;
        private int _dateIndex;
        public DateTime DataSetStart { get; set; }
        public DateTime DataSetEnd { get; set; }
        private List <ISessionColumn > _columns =new List<ISessionColumn>();
        

        //properties 
        public DataSetDateStatus DateStatus
        {
            get
            {
                return _dateStatus;
            }
            set
            {
                _dateStatus = value;
            }
        }
        public int DateIndex
        {
            get
            {
                return _dateIndex;
            }
            set
            {
                _dateIndex = value;
            }
        }
        public List<ISessionColumn> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
                OnCollectionChanged(NotifyCollectionChangedAction.Reset ,_columns);
            }
        }
        public ISessionColumn this[string colname]
        {
            get
            {

                for (int i = 0; i < _columns.Count; i++)
                {
                    if (_columns[i].ColName == colname)
                    {
                        return _columns[i];
                    }
                    foreach (SessionColumn sc in _columns[i].ChildColumns)
                    {
                        if (sc.ColName == colname) return sc;
                    }

                }
                return null;
            }
            set
            {
                for (int i = 0; i < _columns.Count; i++)
                {
                    if (_columns[i].ColName == colname)
                    {
                        _columns[i] = value;
                        break;
                    }
                }
            }
        }
        public ISessionColumn this[int colindex]
        {
            get
            {
                for (int i = 0; i < _columns.Count; i++)
                {
                    if (_columns[i].ColIndex  == colindex)
                    {
                        return _columns[i];
                    }
                    foreach (SessionColumn sc in _columns[i].ChildColumns)
                    {
                        if (sc.ColIndex  == colindex) 
                            return sc;
                    }

                }
                return null;
            }
            set
            {
               _columns[colindex] = value;
              
            }
        }
        public bool DuplicatesExist
        {
            get
            {
                return _duplicatesExist;
            }
        }

        //constructor
        public SessionColumnCollection(DataTable data)
        {
            _data = data;
            _dateStatus = DataSetDateStatus.NotSet;
            _columns = ProcessRawColumns();
            ProcessDate();
            
        }
        //methods 
        private List <ISessionColumn> ProcessRawColumns()
        {
            //set up colname and index and do some data clean up for each column in dataset
            TimeSpan p;
            DateTime start = DateTime.Now;
            try
            {
                
                List <ISessionColumn> thisList=new List <ISessionColumn>();
                
                foreach (DataColumn dc in _data.Columns)
                {   
                    
                    
                    string thiscolname = dc.ColumnName.Replace(".", "_");
                    SessionColumn column = new SessionColumn(dc.Ordinal, thiscolname);
                    Console.WriteLine(dc.ColumnName + " " + dc.DataType);
                    thisList.Add((SessionColumn)column);
                }
                p = DateTime.Now - start;
                Console.WriteLine("processing columns takes " + p);
                return thisList;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void ProcessDate()
        {
            IProcessDateTime process = new ProcessDate(_data);
            _dateStatus = process.FindDateTimeColumn(out _dateIndex);
            
            //overwrite date col name 
            _data.Columns[_dateIndex].ColumnName = Settings.Default.TimeStampName;
            _columns[_dateIndex].ColName = Settings.Default.TimeStampName;
            IDateTimeSequence sequence = new DateTimeSequence(_data, _dateIndex);
            if (_dateStatus == DataSetDateStatus.Found)
            {
                List<DateTime> datesequence = sequence.GetExistingSequence();
                DataSetStart = datesequence.Min();
                DataSetEnd = datesequence.Max();
            }

        }
        public bool CreateDate(IDateOrder dateorder)
        {
            try
            {
                //get date column name from settings
                string dateColName = Settings.Default.TimeStampName;

                //create object to build date list
                IProcessDateTime process = new ProcessDate(_data);
                List<DateTime> newdateCol = process.BuildDateTimeList(dateorder);
                //add 
                if (Utils.AddColtoDataTable<DateTime>(dateColName, newdateCol, _data))
                {   
                    DataColumn dc = _data.Columns[dateColName];
                    _dateIndex = dc.Ordinal;
                    _columns.Add(new SessionColumn (dc.Ordinal ,dc.ColumnName ));
                    this[dateColName].ColumnType  = SessionColumnType.DateTime;
                    _dateStatus = DataSetDateStatus.Found;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public List<ISessionColumn> WSAvgCols()
        {
            //find ws avg columns
            var wsCols = from ISessionColumn s in _columns.AsEnumerable()
                         where s.ColumnType.Equals(SessionColumnType.WSAvg)
                         select s;

            List<ISessionColumn> result = wsCols.ToList<ISessionColumn>();
            return result;
        }
        public List<ISessionColumn> UpperWSAvgCols(DateTime date)
        {

            List<ISessionColumn> workList=GetColumnsByType(SessionColumnType.WSAvg);
            List<ISessionColumn> result = new List<ISessionColumn>();
            List<double> heightlist = new List<double>();
            //get all heights
            foreach (ISessionColumn col in workList)
            {
                Console.WriteLine("adding " + col.ColName);
                heightlist.Add(col.getConfigAtDate(date).Height);

            }
            //figure out which columns belong to the hist height and add them to the result 
            foreach (ISessionColumn col in workList)
            {
                if (col.getConfigAtDate(date).Height == heightlist.Max() && !col.IsComposite )
                {
                    result.Add(col);
                }

            }
                  
            
            return result;
        }
        public List<ISessionColumn> SecondWSAvgCols(DateTime date)
        {
            List<ISessionColumn> workList = GetColumnsByType(SessionColumnType.WSAvg);
            List<ISessionColumn> result = new List<ISessionColumn>();
            List<double> heightlist = new List<double>();
            //get all heights
            foreach (ISessionColumn col in workList)
            {
                heightlist.Add(col.getConfigAtDate(date).Height);

            }
            
             heightlist.RemoveAll(t=>t==heightlist .Max());
            
                
            //figure out which columns belong to the highest height and add them to the result 
            foreach (ISessionColumn col in workList)
            {
                if (col.getConfigAtDate(date).Height == heightlist.Max() && !col.IsComposite )
                {
                    result.Add(col);
                }

            }


            return result;
        }
        public int UpperWSComp(DateTime date)
        {
            SortedDictionary<double, ISessionColumn> wscomps
                    = this.GetColumnsByType(SessionColumnType.WSAvg, date, true);
            int uppercomp = wscomps[wscomps.Keys.Max()].ColIndex;
            return uppercomp;
        }
        public int LowerWSComp(DateTime date)
        {
            SortedDictionary<double, ISessionColumn> wscomps
                    = this.GetColumnsByType(SessionColumnType.WSAvg, date, true);
            //second from the end 
            int lowercomp = wscomps[wscomps.Keys.ToList()[wscomps.Count -2]].ColIndex;
            return lowercomp;
        }
        public int WDComp(DateTime date)
        {
            List<ISessionColumn> wdcols = GetColumnsByType(SessionColumnType.WDAvg);

            foreach (ISessionColumn col in wdcols)
            {
                if (col.IsComposite)
                    return col.ColIndex;
            }

            throw new ApplicationException("No wind direction composite column found for a configuration starting " + date);
                
                
        }
        public SortedDictionary<double, ISessionColumn> GetColumnsByType(SessionColumnType coltype
            , DateTime date,bool iscomposite)
        {
            try
            {
                SortedDictionary<double, ISessionColumn> resultDictionary
                    = new SortedDictionary<double, ISessionColumn>();
                //find 
                var result = from ISessionColumn s in _columns.AsEnumerable()
                             where s.ColumnType.Equals(coltype) && s.IsComposite.Equals(iscomposite )
                             select s;
                foreach (var v in result)
                {
                    //get config at each date 
                    ISensorConfig config = v.getConfigAtDate(date);

                    if (config != null && !resultDictionary.ContainsKey(config.Height))
                    {
                        resultDictionary.Add(config.Height, (ISessionColumn)v);
                    }
                }

                return resultDictionary;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public SortedDictionary<double, ISessionColumn> GetColumnsByType(SessionColumnType coltype
            , DateTime date)
        {
            try
            {
                SortedDictionary<double, ISessionColumn> resultDictionary 
                    = new SortedDictionary<double, ISessionColumn>();
                //find 
                var result = from ISessionColumn s in _columns.AsEnumerable()
                             where s.ColumnType.Equals(coltype) 
                             select s;
                foreach (var v in result)
                {
                    //get config at each date 
                    ISensorConfig config=v.getConfigAtDate(date);
                    
                    if (config!=null && !resultDictionary.ContainsKey (config.Height ))
                    {
                        resultDictionary.Add(config.Height, (ISessionColumn)v);
                    }
                }
                
                return resultDictionary;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public List<ISessionColumn> GetColumnsByType(SessionColumnType coltype)
        {
           
                List<ISessionColumn> resultList = new List<ISessionColumn>();
                //find columns by type
                //Console.WriteLine("Looking for " + coltype + " in session column collection");
               var result = from ISessionColumn s in _columns.AsEnumerable()
                             where s.ColumnType==coltype 
                             select s;

                Console.WriteLine( coltype + " found  " + result.Count());
                resultList=result.ToList();
                return resultList;
            }




        #region INotifyPropertyChanged Members
        [field:NonSerialized ]
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        #endregion

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        
        private void OnCollectionChanged<T>(NotifyCollectionChangedAction action, IList <T> col)
        {

            if (CollectionChanged != null)
            {
                if (CollectionChanged !=null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs (action,col));

            }

        }
        

        #endregion
    }
       
       

        //TODO:implement a command method that enables further processing only after the 
        //date status is set to found.


        

        

        
    }

