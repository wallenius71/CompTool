using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using System.IO;
using WindART.DAL;
using System.Windows.Forms;


namespace WindART
{
    
    public class Session:ISession
    {
            private DataSet _sessionDataSet=new DataSet ("Session");
            private Dictionary<string, ISessionColumnCollection> _columnCollection
               = new Dictionary<string, ISessionColumnCollection>();
            
      
      //properties
            public DataSet SessionDataSet
              {
                  get
                  {
                      return _sessionDataSet;
                  }
              }
            public Dictionary<string,ISessionColumnCollection> ColumnCollections
            {
                  get
                  {                      
                      return _columnCollection;
                  }
          
            }
            
       //public methods

            public bool LoadDatafromFile(string filename, DataSourceType datasourcetype)
        {            
           try 
            {
                
                DataTable thisTable = new DataTable();
                IDataRepository repository = new DataRepository(filename, datasourcetype);
                Console.WriteLine(Path.GetFileName(filename));
                thisTable.TableName = Path.GetFileName(filename);
                thisTable = repository.GetAllData();
                

               //create sessioncolumn collection
                ISessionColumnCollection collection = new SessionColumnCollection(thisTable);
               //add table to dataset after column colection set up

                _sessionDataSet.Tables.Add( thisTable);
               //if date found get missing dates

              
               
               return true;
             }
            catch (Exception e)
            {
                throw e;
            }
            
        }
            

      //private methods 
            private List<ISessionColumn> ProcessColumns(DataTable data)
        {
            //create 
            List<ISessionColumn> result = new List<ISessionColumn>();
            foreach (DataColumn dc in data.Columns)
            {
                ISessionColumn  S = new SessionColumn(dc.Ordinal,dc.ColumnName);
                result.Add(S);
            }
            return result;
        }
        
    }
}
