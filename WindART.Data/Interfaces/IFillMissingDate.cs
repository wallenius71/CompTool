﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WindART
{
    public interface IFillMissingDate
    {
        bool FillMissingdates(List<DateTime> missing);
    }
}
