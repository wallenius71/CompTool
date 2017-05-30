using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindART.DAL
{
    public class DataOrderFactory:IDataOrderFactory 
    {
        private DataSourceType _dataType;
        private string _dataSource;
        private string _uid;
        private string _password;

        public  DataOrderFactory(string datasource,DataSourceType dataType)
        {
            Console.WriteLine(datasource );
            _dataType = dataType;
            _dataSource = datasource;
        }
        public DataOrderFactory(DataSourceType dataType)
        {
            _dataType = dataType;
           
        }
        public DataOrderFactory(string datasource, DataSourceType dataType,string uid,string password)
        {
            Console.WriteLine(datasource);
            _dataType = dataType;
            _dataSource = datasource;
            _uid = uid;
            _password = password;
        }
        public IDataOrder getDataOrder()
        {           
            try
            {

                IDataOrder dataorder = null;
                switch (_dataType)
                {
                    case DataSourceType.XL2003:
                        dataorder = new ExcelDataOrder(_dataSource, _dataType );
                        break;
                    case DataSourceType.XL2007:
                        dataorder = new ExcelDataOrder(_dataSource, _dataType);
                        break;
                    case DataSourceType.CSV:
                        dataorder = new TextDataOrder(_dataSource, _dataType);
                        break;
                    case DataSourceType.TXT:
                        dataorder = new TextDataOrder(_dataSource, _dataType);
                        break;
                    case DataSourceType.PRN:
                        dataorder = new TextDataOrder(_dataSource, _dataType);
                        break;

                    case DataSourceType.OSIPI:
                        dataorder = new PiDBDataOrder(_dataSource, _dataType,_uid,_password);
                        break;
                    case DataSourceType.SQL2005 :
                        dataorder = new SQL2005DataOrder(_dataSource, _dataType, _uid, _password);
                        break;
                    
                    case DataSourceType.WindART_PI :
                        dataorder = new PiDBDataOrder(_dataType);
                        break;
                    
                    case DataSourceType.WindART_SQL :
                        dataorder = new SQL2005DataOrder (_dataType);
                    break;
                }
                
                return dataorder;
            }
            catch (Exception e)
            {
                throw;
            }


        }

        
    }
}
