﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WindART
{
    public interface ISessionColumnCollection
    {
        DataSetDateStatus DateStatus
        {
            get;
            set;
        }
        int DateIndex
        {
            get;
            set;
        }
        List <ISessionColumn> Columns
        {
            get;
            set;
        }
        ISessionColumn this[string colname]
        {
            get;
            set;
        }
        ISessionColumn this[int colindex]
        {
            get;
            set;
        }
        SortedDictionary<double, ISessionColumn> GetColumnsByType(SessionColumnType coltype,DateTime timestamp);
        List<ISessionColumn> GetColumnsByType(SessionColumnType coltype);
        SortedDictionary<double, ISessionColumn> GetColumnsByType(SessionColumnType coltype
            , DateTime date, bool iscomposite);
        bool CreateDate(IDateOrder dateorder);
        int UpperWSComp(DateTime date);
        int LowerWSComp(DateTime date);
        

    }
}
