using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GemBox.Spreadsheet;

namespace WindART
{
    public abstract class AbstractExcelWorkSheet
    {
        public abstract ExcelWorksheet  BuildWorkSheet();
    }
}
