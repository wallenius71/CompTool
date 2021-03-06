﻿#undef CHILDREN
//#define CHILDREN
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindART.DAL;
using System.Data;
using WindART.Properties;
using System.IO;

namespace WindART
{
    public static class InitSettings
    {
       
       //public static string testFolderPath = @"C:\Users\Jesse\Desktop";
        public static string testFolderPath = @"\\bp1\public\JesseRichards\REAL_DATA\Shear Matrix Testing\";
        public static DataTable data;
        public static ISessionColumnCollection collection;

        #region testing columns
        public static int originaluppercomp = 9;
        public static int originallowercomp = 10;
        public static int originalUpperSTDComp = 22;
        public static int originalLowerSTDComp = 23;
        public static int originalUpperMaxComp = 30;
        public static int originalLowerMaxComp = 31;
        public static int original2by24WS = 15;
        public static int originalbulkshearWS = 11;
        public static int originalwdcomp = 8;
        #endregion

        
        public static int uppervalA = 2;
        public static int upperSTDA = 6;
        public static int upperMaxA = 5;
        public static int uppervalB = 7;
        public static int upperSTDB = 8;
        public static int upperMaxB = 9;
        public static int lowervalA = 10;
        public static int lowerSTDA = 12;
        public static int lowerMaxA = 13;
        public static int lowervalB = 15;
        public static int lowerSTDB = 15;
        public static int lowerMaxB = 16;
        public static int upperwd = 3;
        public static int lowerwd = 11;

        public static DateTime start = DateTime.Parse("05/12/04 14:30");
        public static DateTime end = DateTime.Parse("03/31/10 23:50");

        public static double UpperWSHt = 60;
        public static double LowerWSHt = 30;
        public static double UpperWS1Ornt = 315;
        public static double UpperWS2Ornt = 135;
        public static double LowerWS1Ornt =315;
        public static double LowerWS2Ornt = 135;
        public static string filename = "0505.csv";
        public static string thisSite = "0505";
	
        public static int temp = 29;
        public static int bp = 27;
        public static int airDen = 16;

        static InitSettings()
        {
            try
            {

                IDataRepository repository
                       = new DataRepository(string.Format(@"{0}{1}", testFolderPath, filename)
                            , DataSourceType.CSV);
                data = repository.GetAllData();
                collection = new SessionColumnCollection(data);

                #region column set up
                //set up columns


#if (CHILDREN)

                //upper Max A
                SensorConfig upperMAXAconfig = new SensorConfig();
                upperMAXAconfig.Height = UpperWSHt;
                upperMAXAconfig.StartDate = start;
                upperMAXAconfig.EndDate = end;
                collection[upperMaxA].ColumnType = SessionColumnType.WSMax;
                collection[upperMaxA].addConfig(upperMAXAconfig);


                //Upper std A
                SensorConfig upperstdAconfig = new SensorConfig();
                upperstdAconfig.Height = UpperWSHt;
                upperstdAconfig.StartDate = start;
                upperstdAconfig.EndDate = end;
                collection[upperSTDA].ColumnType = SessionColumnType.WSStd;
                collection[upperSTDA].addConfig(upperstdAconfig);
#endif

                //WSavg A
                IWindSpeedConfig config1 = new WindSpeedConfig();
                config1.Height = UpperWSHt;
                config1.Orientation = UpperWS1Ornt;
                config1.HeightUnits = "m";
                config1.StartDate = start;
                config1.EndDate = end;

                collection[uppervalA].ColumnType = SessionColumnType.WSAvg;
                collection[uppervalA].addConfig(config1);

                //children
#if(CHILDREN)
                collection[uppervalA].ChildColumns.Add(collection[upperMaxA]);
                collection[uppervalA].ChildColumns.Add(collection[upperSTDA]);


                //upper Max B
                SensorConfig upperMAXBconfig = new SensorConfig();
                upperMAXBconfig.Height = UpperWSHt;
                upperMAXBconfig.StartDate = start;
                upperMAXBconfig.EndDate = end;
                collection[upperMaxB].ColumnType = SessionColumnType.WSMax;
                collection[upperMaxB].addConfig(upperMAXBconfig);


                //Upper std B
                SensorConfig upperstdBconfig = new SensorConfig();
                upperstdBconfig.Height = UpperWSHt;
                upperstdBconfig.StartDate = start;
                upperstdBconfig.EndDate = end;
                collection[upperSTDB].ColumnType = SessionColumnType.WSStd;
                collection[upperSTDB].addConfig(upperstdBconfig);
#endif
                //WSAvg B
                IWindSpeedConfig config2 = new WindSpeedConfig();
                config2.Height = UpperWSHt;
                config2.Orientation = UpperWS2Ornt;
                config2.HeightUnits = "m";
                config2.StartDate = start;
                config2.EndDate = end;
                collection[uppervalB].ColumnType = SessionColumnType.WSAvg;
                collection[uppervalB].addConfig(config2);

                //child cols
#if (CHILDREN)
                collection[uppervalB].ChildColumns.Add(collection[upperSTDB]);
                collection[uppervalB].ChildColumns.Add(collection[upperMaxB]);

                //***** Lower ******

                //lower Max A
                SensorConfig lowerMAXAconfig = new SensorConfig();
                lowerMAXAconfig.Height = LowerWSHt;
                lowerMAXAconfig.StartDate = start;
                lowerMAXAconfig.EndDate = end;
                collection[lowerMaxA].ColumnType = SessionColumnType.WSMax;
                collection[lowerMaxA].addConfig(lowerMAXAconfig);


                //lower std A
                SensorConfig lowerstdAconfig = new SensorConfig();
                lowerstdAconfig.Height = UpperWSHt;
                lowerstdAconfig.StartDate = start;
                lowerstdAconfig.EndDate = end;
                collection[lowerSTDA].ColumnType = SessionColumnType.WSStd;
                collection[lowerSTDA].addConfig(upperstdAconfig);
#endif
                //lower ws A
                IWindSpeedConfig config3 = new WindSpeedConfig();
                config3.Height = LowerWSHt;
                config3.Orientation = LowerWS1Ornt;
                config3.HeightUnits = "m";
                config3.StartDate = start;
                config3.EndDate = end;
                collection[lowervalA].ColumnType = SessionColumnType.WSAvg;
                collection[lowervalA].addConfig(config3);
                //children

#if(CHILDREN)
                collection[lowervalA].ChildColumns.Add(collection[lowerSTDA]);
                collection[lowervalA].ChildColumns.Add(collection[lowerMaxA]);

                //lower Max B
                SensorConfig lowerMAXBconfig = new SensorConfig();
                lowerMAXBconfig.Height = LowerWSHt;
                lowerMAXBconfig.StartDate = start;
                lowerMAXBconfig.EndDate = end;
                collection[lowerMaxB].ColumnType = SessionColumnType.WSMax;
                collection[lowerMaxB].addConfig(lowerMAXAconfig);


                //lower std B
                SensorConfig lowerstdBconfig = new SensorConfig();
                lowerstdBconfig.Height = UpperWSHt;
                lowerstdBconfig.StartDate = start;
                lowerstdBconfig.EndDate = end;
                collection[lowerSTDB].ColumnType = SessionColumnType.WSStd;
                collection[lowerSTDB].addConfig(upperstdBconfig);
#endif
                //lower ws B 
                IWindSpeedConfig config4 = new WindSpeedConfig();
                config4.Height = LowerWSHt;
                config4.Orientation = LowerWS2Ornt;
                config4.HeightUnits = "m";
                config4.StartDate = start;
                config4.EndDate = end;
                collection[lowervalB].ColumnType = SessionColumnType.WSAvg;
                collection[lowervalB].addConfig(config4);
                ////children
#if (CHILDREN)
                collection[lowervalB].ChildColumns.Add(collection[lowerMaxB]);
                collection[lowervalB].ChildColumns.Add(collection[lowerSTDB]);
#endif

                //WD
                ISensorConfig config5 = new SensorConfig();
                config5.Height = 49;
                config5.Orientation = 10;
                config5.HeightUnits = "m";
                config5.StartDate = start;
                config5.EndDate = end;
                collection[upperwd].ColumnType = SessionColumnType.WDAvg;
                collection[upperwd].addConfig(config5);

                ISensorConfig config6 = new SensorConfig();
                config6.Height = 37;
                config6.Orientation = 10;
                config6.HeightUnits = "m";
                config6.StartDate = start;
                config6.EndDate = end;
                collection[lowerwd].ColumnType = SessionColumnType.WDAvg;
                collection[lowerwd].addConfig(config6);

                #endregion set up columns
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
        [TestFixture]
        public class TestDAL
        {
            string _testDrivePath;
            [SetUp]
            public void Init()
            {
                _testDrivePath = InitSettings.testFolderPath;
            }
            [Test]
            public void TestDataOrderFactory()
            {


                IDataOrderFactory dataorderfactory
                    = new DataOrderFactory(string.Format(@"{0}\TestFiles\WindARTTest.xls", _testDrivePath),
                        DataSourceType.XL2003);
                IDataOrder dataorder = dataorderfactory.getDataOrder();
                Assert.IsInstanceOf (typeof(ExcelDataOrder), dataorder);

                dataorderfactory = new DataOrderFactory(string.Format(@"{0}\TestFiles\8994Test.xlsx", _testDrivePath),
                    DataSourceType.XL2007);
                dataorder = dataorderfactory.getDataOrder();
                Assert.IsInstanceOf(typeof(ExcelDataOrder), dataorder);

                dataorderfactory = new DataOrderFactory(string.Format(@"{0}\TestFiles\WindARTTest.CSV", _testDrivePath)
                    , DataSourceType.CSV);
                dataorder = dataorderfactory.getDataOrder();
                Assert.IsInstanceOf(typeof(TextDataOrder), dataorder);

                dataorderfactory = new DataOrderFactory(string.Format(@"{0}\TestFiles\WindARTTest.TXT", _testDrivePath), DataSourceType.TXT);
                dataorder = dataorderfactory.getDataOrder();
                Assert.IsInstanceOf(typeof(TextDataOrder), dataorder);


                //dataorderfactory = new DataOrderFactory(DataSourceType.SQL2005 );
                //dataorder = dataorderfactory.getDataOrder();
                //Assert.IsInstanceOfType(typeof(DbDataOrder), dataorder);

                
            }

            [Test]
            public void TestOSIPIDataOrder()
            {//dataorderfactory returns an instance of DBDataOrder
                IDataOrderFactory dataorderfactory = new DataOrderFactory(@"10.96.3.138", DataSourceType.OSIPI);
                IDataOrder dataorder = dataorderfactory.getDataOrder();
                Assert.IsInstanceOf(typeof(PiDBDataOrder), dataorder);
                
            }

            [Test]
            public void TestSQL2005dataOrder()
            {//dataorderfactory returns an instance of DBDataOrder
                IDataOrderFactory dataorderfactory = new DataOrderFactory(@"10.128.10.8", DataSourceType.SQL2005 );
                IDataOrder dataorder = dataorderfactory.getDataOrder();
                Assert.IsInstanceOf(typeof(SQL2005DataOrder ), dataorder);
                
            }

            
            [Test]
            public void TestExcelDataOrder()
            {
                IDataOrder exceldataorder = new ExcelDataOrder(string.Format(@"{0}\TestFiles\1927.xlsx", _testDrivePath),
                        DataSourceType.XL2007);
                Assert.IsInstanceOf(typeof(ExcelDataOrder), exceldataorder);
            }
            [Test]
            public void TestExcel2003DataRepository()
            {
                IDataRepository repository
                    = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTest.xls", _testDrivePath), DataSourceType.XL2003);

                Assert.Greater(repository.GetAllData().Rows.Count, 0);
            }
            [Test]
            public void TestExcel2007DataRepository()
            {
                IDataRepository repository
                    = new DataRepository(string.Format(@"{0}\TestFiles\8894Test.xlsx", _testDrivePath), 
                        DataSourceType.XL2007);

                Assert.Greater(repository.GetAllData().Rows.Count, 0);
            }
            [Test]
            public void TestCSVDataRepository()
            {
                IDataRepository repository
                    = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTest.csv", _testDrivePath), DataSourceType.CSV);

                Assert.Greater(repository.GetAllData().Rows.Count, 0);
            }
            [Test]
            public void TestTXTDataRepository()
            {
                IDataRepository repository
                    = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTest.txt", _testDrivePath), DataSourceType.TXT);

                Assert.Greater(repository.GetAllData().Rows.Count, 0);
            }
            
           [Test]
            public void GetCustomDataFromSQLDataRepository()
            {
                string sql =@"select * from processfile where filestatus=4";
               IDataRepository repository
                    = new DataRepository(@"10.128.10.8",DataSourceType.SQL2005 ,
                    @"Windart_JesseRichards",@"metteam555");
               DataTable dt=repository.GetData (sql);
                Assert.Greater(dt.Rows.Count, 0);
                Console.WriteLine(dt.Rows.Count);

            } 
        }
        [TestFixture]
        public class TestUtils
        {
            [Test]
            public void TestMonths()
            {
                int test=Utils.TimeSpanMonths (DateTime.Parse("01/02/06"),DateTime.Parse("10/05/09"));
                Assert.AreEqual(46, test);
                int test1 = Utils.TimeSpanMonths(DateTime.Parse("02/02/05"), DateTime.Parse("10/05/05"));
                Assert.AreEqual(9, test1);
                int test2 = Utils.TimeSpanMonths(DateTime.Parse("01/02/05"), DateTime.Parse("1/05/05"));
                Assert.AreEqual(1, test2);
                int test3 = Utils.TimeSpanMonths(DateTime.Parse("01/02/05"), DateTime.Parse("12/05/06"));
                Assert.AreEqual(24, test3);



            }
        }
        [TestFixture]
        public class TestSessionColumnCollection
        {//julian dates in 2=year,3=day,4=hourminute...23 columns
           
            DataTable cleanData=null;
           
            DataTable julianDateData=null;

            //[TestFixtureSetUp]
            //public  void init()
            //{
            //    testFolderPath =  InitSettings.testFolderPath;
            //    IDataRepository GapandDuprepository
            //        = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTestGapsandDups.csv", testFolderPath),
            //            DataSourceType.CSV);
            //    gapAndDupData = GapandDuprepository.GetAllData();
            //    IDataRepository JulianDateRepository
            //        = new DataRepository(string.Format(@"{0}\TestFiles\502Test.csv", testFolderPath),
            //            DataSourceType.CSV );
            //    julianDateData = JulianDateRepository.GetAllData();

            //    IDataRepository CleanRepository
            //        = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTest.csv", testFolderPath),
            //            DataSourceType.CSV);
            //    cleanData = CleanRepository.GetAllData();

            //}
            [Test]
            public void TestSessionColumnSetupJulianDate()
            {   
                
                //date is not found and column collection is correct
                ISessionColumnCollection collection = new SessionColumnCollection(julianDateData);
                Assert.AreEqual(DataSetDateStatus.FoundNone, collection.DateStatus);
                Assert.AreEqual(23, collection.Columns.Count);
                //build the date and cehck column and date status again
                BuildJulianDate julian = new BuildJulianDate();
                julian.year.index = 2;
                julian.doy.index = 3;
                julian.hoursminutes.index  = 4;
                Assert.IsTrue (collection.CreateDate(julian));
                Assert.AreEqual(24, collection.Columns.Count);
                Assert.AreEqual(DataSetDateStatus.Found, collection.DateStatus);
                Assert.IsInstanceOf(typeof(ISessionColumn),collection[Settings.Default.TimeStampName] );
                Assert.AreEqual(SessionColumnType.DateTime, collection[Settings.Default.TimeStampName].ColumnType);

                
                
            }
            [Test]
            public void TestRetreivingColumnByType()
            {
                ISessionColumnCollection collection = new SessionColumnCollection(cleanData);
                collection.Columns[7].ColumnType = SessionColumnType.WDAvg;
                collection.Columns[15].ColumnType = SessionColumnType.WDAvg;

                List<ISessionColumn > test= collection.GetColumnsByType(SessionColumnType.WDAvg);
                Assert.AreEqual(test.Count, 2);
            }
            [Test]
            public void TestGettingUpperWSCols()
            {   

                SessionColumnCollection colxn=(SessionColumnCollection )InitSettings .collection;
                DateTime date=(DateTime)InitSettings.data.Rows[0][0];
                Console.WriteLine("date is " + date);
                List<ISessionColumn> cols = colxn.UpperWSAvgCols(date);
                foreach (ISessionColumn col in cols)
                {
                    Console.WriteLine("found upper sensor height " + col.getConfigAtDate (date).Height);
                }
            }
            [Test]
            public void TestGettingSecondWSCols()
            {

                SessionColumnCollection colxn = (SessionColumnCollection)InitSettings.collection;
                DateTime date = (DateTime)InitSettings.data.Rows[0][0];
                Console.WriteLine("date is " + date);
                List<ISessionColumn> cols = colxn.SecondWSAvgCols (date);
                foreach (ISessionColumn col in cols)
                {
                    Console.WriteLine("found second sensor height " + col.getConfigAtDate(date).Height);
                }
            }
            [Test]
            public void TestDirectCompRetrieve()
            {
                DataView view= InitSettings.data.AsDataView();
                    view.Sort = "DateTime";
                    DateTime start = (DateTime)view[0][0];

                WindDirectionComposite wdcomp = new WindDirectionComposite(InitSettings.collection, InitSettings.data);
                wdcomp.CalculateComposites();
                WindSpeedComposite wscomp = new WindSpeedComposite(InitSettings.collection, InitSettings.data);
                wscomp.CalculateComposites();

                Assert.AreEqual(31, InitSettings.collection.UpperWSComp(start));
            }
            [Test]
            public void PrintoutColumnTypes()
            {
                //print out the types imported by ado.net
                foreach (DataColumn c in InitSettings.data.Columns)
                {
                    Console.WriteLine(c.ColumnName  + " " + c.DataType);
                }
            }
            
        }
        [TestFixture]
        public class TestDateTimeProcessing
        {
            string testFolderPath = InitSettings.testFolderPath;

            DataTable gaps = new DataTable();

            [TestFixtureSetUp]
            public void init()
            {
                
                IDataRepository repository
                    = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTestGapsandDups.csv", testFolderPath)
                        , DataSourceType.CSV);

                gaps = repository.GetAllData();
            }
            [Test]
            public void TestMissingTimeStamps()
            {
                IDateTimeSequence  process = new DateTimeSequence (gaps,23);
                int cnt=process.GetMissingTimeStamps().Count;
                Console.WriteLine("Missing Timestamps " + cnt);
                Assert.Greater(cnt, 0);

            }
            [Test]
            public void TestGetExistingSequence()
            {
                IDateTimeSequence sequence = new DateTimeSequence(gaps, 23);
                
                int cnt = sequence.GetExistingSequence().Count;
                Console.WriteLine("Existing Sequence Count " + cnt);
                Assert.Greater(cnt, 0);
            }
            [Test]
            public void TestGetExpectedSequence()
            {
                IDateTimeSequence sequence = new DateTimeSequence(gaps, 23);
                int result = sequence.GetExpectedSequence().Count;
                Console.WriteLine("Expected TimeStamps count " + result);
                Assert.Greater(result,0);
            }
            [Test]
            public void TestCreatingdateTimeSequence()
            {
                IDateTimeSequence sequence = new DateTimeSequence(gaps, 23);
                Assert.IsInstanceOf (typeof(DateTimeSequence), sequence);
            }
            [Test]
            public void TestDetectInterval()
            {
                IDateTimeSequence sequence = new DateTimeSequence(gaps, 23);
                TimeSpan s=TimeSpan .FromMinutes (10);
                TimeSpan result = sequence.DetectInterval();
                Console.WriteLine("Interval Detected " + result);
                Assert.AreEqual(s, result);

            }
            [Test]
            public void TestDataPrepFillMisssings()
            {
                IFillMissingDate data = new DataPrep(gaps, 23);
                int countBefore=gaps.Rows .Count;
                IDateTimeSequence sequence = new DateTimeSequence(gaps, 23);
                List<DateTime> missing = sequence.GetMissingTimeStamps();
                data.FillMissingdates(missing);
                Console.WriteLine("Fill missing Dates , after: " + missing.Count + " before: " + countBefore);
                Assert.Greater(missing.Count, countBefore);
                
            }
            [Test]
            public void TestFindDuplicateRows()
            {
                DataView result = new DataView();
                IDuplicateTimeStamps duplicates = new DuplicateTimeStamps(gaps, 23);
                result = duplicates.GetDuplicateDateView();
                Console.WriteLine("Duplicaterow count " + result.Count);
                Assert.Greater(result.Count, 0);
            }
            [Test]
            public void TestGetDistinctDuplicateCount()
            {
                IDuplicateTimeStamps duplicates = new DuplicateTimeStamps(gaps, 23);
                int result = duplicates.GetDistinctDuplicates().Count;
                Console.WriteLine("Distinct Duplicate Count " + result);
                Assert.Greater(result, 0);
            }

        }
        [TestFixture]
        public class TestDataRecovery
        {
            string testFolderPath = InitSettings.testFolderPath;
            DataTable gaps = new DataTable();

            [TestFixtureSetUp]
            public void init()
            {
                IDataRepository repository
                    = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTestGapsandDups.csv", testFolderPath)
                        , DataSourceType.CSV);

                gaps = repository.GetAllData();
            }

            [Test]
            public void TestGetNonMissingRecordCount()
            {
                IDataRecovery recov = new DataRecovery(gaps);
                int result = recov.GetNonMissingRecords(0);
                int answer = gaps.Rows.Count - 1;
                Assert.AreEqual(answer, result);
            }
            [Test]
            public void TestGetNonMissingRecordCountbyDates()
            {
                IDataRecovery recov = new DataRecovery(gaps);

                DateTime start = DateTime.Parse("05/01/08");
                DateTime end = DateTime.Parse("06/01/08");
                int  result = recov.GetNonMissingRecords (0, 23, start, end);
                Assert.AreEqual(4471, result);
                
            }
            [Test]
            public void TestGetNonMissingRecordCountbyValues()
            {
                IDataRecovery recov = new DataRecovery(gaps);
                
                double result = recov.GetNonMissingRecords(0, 7, 112,113);
                Assert.AreEqual(15, result);

            }
            [Test]
            public void TestGetNonMissingRate()
            {
                IDataRecovery recov = new DataRecovery(gaps);
                double result = recov.GetNonMissingRate(0);
                Console.WriteLine("Test rate " + result);
                double answer = double.Parse ("99.98");
                Assert.AreEqual(answer, result);
            }
            [Test]
            public void TestGetNonMissingRatebyDates()
            {
                IDataRecovery recov = new DataRecovery(gaps);

                DateTime start=DateTime.Parse("05/01/08");
                DateTime end=DateTime.Parse("06/01/08");

                double result = recov.GetNonMissingRate(0,23,start,end);
                Assert.AreEqual(100, result);


            }
            [Test]
            public void TestGetNonMissingRatebyValues()
            {
                IDataRecovery recov = new DataRecovery(gaps);
                
                double result = recov.GetNonMissingRate(0, 7, 112,113);
                Assert.AreEqual(93.75, result);

            }
            
        
        }
        [TestFixture]
        public class TestConfig
        {
            string testFolderPath =  InitSettings.testFolderPath;
            DataTable gaps = new DataTable();
            [TestFixtureSetUp]
            public void init()
            {
                IDataRepository repository
                    = new DataRepository(string.Format(@"{0}\TestFiles\WindARTTestGapsandDups.csv", testFolderPath)
                        , DataSourceType.CSV);

                gaps = repository.GetAllData();
            }
            [Test]
            public void TestColumnTypeEntry()
            {
                ISessionColumnCollection collection = new SessionColumnCollection(gaps);
                SessionColumnType result = collection["50mWS#1"].ColumnType;
                SessionColumnType coltype = SessionColumnType.WSAvg ;
                Assert.AreEqual(coltype, result);
            }
            [Test]
            public void TestAddSensorConfig()
            {
                //add a sensor config to a session column
                ISessionColumn column = new SessionColumn(1, "test");
                ISensorConfig config = new SensorConfig();
                config.StartDate = DateTime.Parse("1/1/01");
                config.Height = 20;
                config.HeightUnits = "m";
                config.Multiplier = .765;
                config.Offset = .30;

                ISensorConfig config1 = new SensorConfig();
                config1.StartDate = DateTime.Parse("2/1/01");
                config1.Height = 20;
                config1.HeightUnits = "m";
                config1.Multiplier = .768;
                config1.Offset = .32;

                column.Configs.Add(config);
                column.Configs.Add(config1);

                Assert.AreEqual(2,column.Configs.Count);
                Assert.AreEqual(.768, column.Configs[1].Multiplier);
            }
            [Test]
            public void AddWSConfig()
            {
                ISessionColumn column = new SessionColumn(1, "test");
                column.ColumnType = SessionColumnType.WSAvg ;
                //get the right sensor config typr for the columnn
                ISensorConfigFactory factory = new SensorConfigFactory();
                WindSpeedConfig  config = (WindSpeedConfig)factory.CreateConfigType(column.ColumnType);
                config.Orientation = 200;
                column.Configs.Add(config);
                WindSpeedConfig result=(WindSpeedConfig)column.Configs[0];
                Console.WriteLine("good sector start " + result.GoodSector.SectorStart );
                Console.WriteLine("good sector end " + result.GoodSector .SectorEnd );
                Console.WriteLine("shadow sector start " + result.ShadowSector .SectorStart);
                Console.WriteLine("shadow sector end " + result.ShadowSector.SectorEnd);
                
            }
            [Test]
            public void AddMultipleConfigs()
            {
                ISessionColumn column = new SessionColumn(1, "test");
                column.ColumnType = SessionColumnType.WSAvg ;
                WindSpeedConfig config = new WindSpeedConfig();
                config.Orientation = 100;
                column.Configs.Add(config);

                WindSpeedConfig config1 = new WindSpeedConfig();
                config1.Orientation = 200;
                column.Configs.Add(config1);

                WindSpeedConfig result = (WindSpeedConfig)column.Configs[0];
                Console.WriteLine("good sector start " + result.GoodSector.SectorStart);
                Console.WriteLine("good sector end " + result.GoodSector.SectorEnd);
                Console.WriteLine("shadow sector start " + result.ShadowSector.SectorStart);
                Console.WriteLine("shadow sector end " + result.ShadowSector.SectorEnd);

                WindSpeedConfig result1 = (WindSpeedConfig)column.Configs[1];
                Console.WriteLine("\n good sector 1 start " + result1.GoodSector.SectorStart);
                Console.WriteLine("good sector 1 end " + result1.GoodSector.SectorEnd);
                Console.WriteLine("shadow sector 1 start " + result1.ShadowSector.SectorStart);
                Console.WriteLine("shadow sector 1 end " + result1.ShadowSector.SectorEnd);
                Console.WriteLine("\n changing orientation \n");
                
                column.Configs[1].Orientation = 300;
                Console.WriteLine("good sector 1 start " + result1.GoodSector.SectorStart);
                Console.WriteLine("good sector 1 end " + result1.GoodSector.SectorEnd);
                Console.WriteLine("shadow sector 1 start " + result1.ShadowSector.SectorStart);
                Console.WriteLine("shadow sector 1 end " + result1.ShadowSector.SectorEnd);
            }
            
            
        }
        [TestFixture]
        public class TestComposites
        {
            ISessionColumnCollection collection=InitSettings.collection ;
            DataTable data=InitSettings .data;
            
            [Test]
            public void TestWDComposite()
            {
                //foreach (ISessionColumn c in InitSettings . collection.Columns)
                //{
                //    Console.WriteLine(c.ColIndex + " " + c.ColName + " type " + InitSettings . data.Columns[c.ColIndex].DataType);
                //}
                //test creating wd composite
                WindDirectionComposite composite = new WindDirectionComposite(InitSettings.collection ,InitSettings . data);
                composite.CalculateComposites();

                //check to see if all of the columns are there 
                
                //check each composite against the one exisiting in the test file
                
                List<ISessionColumn> wdcompcol 
                    = collection.GetColumnsByType(SessionColumnType.WDAvg);
                int compcol = 0;
                int originalcomp = InitSettings.originalwdcomp;

                foreach (ISessionColumn c in wdcompcol)
                {
                    Console.WriteLine("is composite " + c.IsComposite);
                    if (c.IsComposite)
                    {   
                        compcol = c.ColIndex;
                        Console.WriteLine("compindex " + compcol);
                        break;
                    }
                    
                }
                foreach (ISessionColumn c in collection.Columns)
                {
                    Console.WriteLine(c.ColIndex + " " + c.ColName);
                }
                
                DataView view = data.AsDataView();
                view.Sort = Settings.Default.TimeStampName;
                
                int cnt = 0;
                bool fail=false;
                double outoWDComp=0;
                double outthisWDComp=0;
                string ocompStr;
                string compStr;
                foreach(DataRowView row in view)
                {
                    ocompStr = row[originalcomp].ToString();
                    compStr = row[compcol].ToString();

                    if ((double.TryParse(ocompStr, out outoWDComp)) && (Double.TryParse(compStr, out outthisWDComp)))
                    {
                        //Console.WriteLine(row[8] + " comp " + row[compcol]);
                        if (Math.Abs(Math.Round(outoWDComp, 2) - Math.Round(outthisWDComp, 2)) > 0)
                        {
                            Console.WriteLine("II: ws comp values don't match at " + row[0].ToString() + " correct :"
                              + Math.Round(outoWDComp, 2) + " calculated : "
                              + Math.Round(outthisWDComp, 2) + " correct cnt= " + cnt);
                            fail = true;
                        }
                        cnt++;
                    }
                    else
                    {
                        Console.WriteLine(" Date " + row[0] + " original comp " + row[originalcomp].ToString());
                    }

                }
                Assert.IsFalse(fail);
                
                              

            }
            [Test]
            public void TestCompositePair()
            {   
                
                IConfigCollection configs = new HeightConfigCollection(InitSettings . collection);
                List<HeightConfig> ht = configs.GetConfigs().ConvertAll<HeightConfig>(t => (HeightConfig)t);
                Assert.Greater(ht[0].Columns.Count,0);
                
               
            }
            [Test]
            public void TestWSComposite()
            {
                ISessionColumnCollection collection= InitSettings.collection;
                DataTable data = InitSettings.data;
                IComposite wdcomp = new WindDirectionComposite(collection, data);
                wdcomp.CalculateComposites();
                IComposite wscomp = new WindSpeedComposite(collection, data);
                Assert.IsNotNull(wscomp);
                Assert.IsTrue (wscomp.CalculateComposites());

                int originaluppercomp = InitSettings.originaluppercomp;
                int originallowercomp = InitSettings.originallowercomp;
                
                
                int uppervalA = InitSettings .uppervalA ;
               
                int uppervalB = InitSettings .uppervalB ;
               
                
                int lowervalA = InitSettings .lowervalA ;
                

               
                int lowervalB = InitSettings.lowervalB;
#if(CHILDREN)
                int originalupperMAXComp=InitSettings.originalUpperMaxComp ;
                int originalupperSTDComp=InitSettings.originalUpperSTDComp ;
                int originallowerMAXComp=InitSettings.originalLowerMaxComp ;
                int originallowerSTDComp=InitSettings.originalLowerSTDComp ;
                int upperMaxA = InitSettings.upperMaxA;
                int upperStdA = InitSettings.upperSTDA;
                int upperMaxB = InitSettings.upperMaxB;
                int upperStdB = InitSettings.upperSTDB;
                int lowerMaxA = InitSettings.lowerMaxA;
                int lowerStdA = InitSettings.lowerSTDA;
                int lowerMaxB = InitSettings.lowerMaxA;
                int lowerStdB = -InitSettings.lowerMaxB;
#endif

                int wdcompcol = collection["WDAvgComposite"].ColIndex;

                //assigned ws comps
                SortedDictionary <double,ISessionColumn > wscomps=collection .GetColumnsByType (SessionColumnType .WSAvg ,
                  InitSettings .start ,true);
                Console.WriteLine("ws comps found in test " + wscomps.Count);
                int lowercomp=wscomps.Values.ToList ()[0].ColIndex ;
                Console.WriteLine(" lower comp index in test " + lowercomp);
                int uppercomp=wscomps.Values.ToList()[1].ColIndex;
                Console.WriteLine(" upper comp index in test " + uppercomp);
#if (CHILDREN)
                //assigned std comps 
                SortedDictionary<double, ISessionColumn> STDcomps = collection.GetColumnsByType(SessionColumnType.WSStd,
                  InitSettings.start, true);
                Console.WriteLine("STD comps found in test " + wscomps.Count);
                int lowerSTDcomp = STDcomps.Values.ToList()[0].ColIndex;
                Console.WriteLine(" lower STD comp index in test " + lowerSTDcomp);
                int upperSTDcomp = STDcomps.Values.ToList()[1].ColIndex;
                Console.WriteLine(" upper STD comp index in test " + upperSTDcomp);

                //assigned max comps 
                SortedDictionary<double, ISessionColumn> Maxcomps = collection.GetColumnsByType(SessionColumnType.WSMax,
                  InitSettings.start, true);
                Console.WriteLine("Max comps found in test " + wscomps.Count);
                int lowerMaxcomp = Maxcomps.Values.ToList()[0].ColIndex;
                Console.WriteLine(" lower max comp index in test " + lowerMaxcomp);
                int upperMaxcomp = Maxcomps.Values.ToList()[1].ColIndex;
                Console.WriteLine(" upper max comp index in test " + upperMaxcomp);
#endif 
                DataView view = data.AsDataView();
                view.Sort = "DateTime asc";
                int cnt = 0;
                bool fail1 = false;
                double cut = .01;
                double outOriginalComp;
                double outUpperComp;
                
                foreach (DataRowView drv in view)
                {
                    if (Double.TryParse(drv[originaluppercomp].ToString(), out outOriginalComp) &&
                        Double.TryParse(drv[uppercomp].ToString(), out outUpperComp))
                    {
                        if (Math.Abs(Math.Round(((double)drv[originaluppercomp ]), 3) - Math.Round(((double)drv[uppercomp]), 3)) > cut)
                        {
                            Console.WriteLine("I: ws comp values don't match at "  + drv[0].ToString() 
                              + " correct:" + Math.Round(outOriginalComp, 3) + " calculated:"
                              + Math.Round(outUpperComp, 3) + "  A:" + Math.Round(((double)drv[uppervalA]), 3)
                              + "  B:" + Math.Round(((double)drv[uppervalB]), 3)
                              + " wd " + (double)drv[wdcompcol]);
                            fail1 = true;
                        }
#if(CHILDREN)
                    if ((Math.Abs(Math.Round(((double)drv[originalupperMAXComp]), 3) - Math.Round(((double)drv[upperMaxcomp ]), 3)) > cut))
                    {
                        Console.WriteLine("I: Max comp values don't match at " + drv[0].ToString() + " correct:"
                          + Math.Round(((double)drv[originalupperMAXComp]), 2) + " calculated:"
                          + Math.Round(((double)drv[upperMaxcomp]), 2) + "  A:" + Math.Round(((double)drv[upperMaxA ]), 2)
                          + "  B:" + Math.Round(((double)drv[upperMaxB ]), 2) + " comp val " + Math.Round(((double)drv[uppercomp]), 2));
                        
                        fail1 = true;
                    }

                    if ((Math.Abs(Math.Round(((double)drv[originalupperSTDComp ]), 3) - Math.Round(((double)drv[upperSTDcomp ]), 3)) > cut))
                    {
                        Console.WriteLine("I: STD comp values don't match at " + drv[0].ToString() + " correct:"
                          + Math.Round(((double)drv[originalupperSTDComp]), 2) + " calculated:"
                          + Math.Round(((double)drv[upperSTDcomp]), 2) + "  A:" + Math.Round(((double)drv[upperStdA ]), 2)
                          + "  B:" + Math.Round(((double)drv[upperStdB]), 2) + " comp val " + Math.Round(((double)drv[uppercomp]), 2));

                        fail1 = true;
                    }
#endif
                        cnt++;
                    }
                    else
                    {
                        Console.WriteLine("Date " +  " skipped : original comp val :" + drv[originaluppercomp].ToString());
                    }
                }
                bool fail2 = false;
                cnt = 0;
                //double outOriginalLowerComp;
                //double outLowerComp;
                foreach (DataRowView drv in view)
                {

                    if (Math.Abs(Math.Round(((double)drv[originallowercomp]), 3) - Math.Round(((double)drv[lowercomp]), 3)) > cut)
                    {
                        Console.WriteLine("II: ws comp values don't match at " + drv[0].ToString() + " correct:"
                          + Math.Round(((double)drv[originallowercomp]), 2) + " calculated:" + Math.Round(((double)drv[lowercomp]), 2)
                          + "  A:" + Math.Round(((double)drv[lowervalA]), 2)
                          + "  B:" + Math.Round(((double)drv[lowervalB]), 2)
                          + " wd " + (double)drv[wdcompcol]);
                        fail2 = true;
                    }
# if (CHILDREN)
                    if (Math.Abs(Math.Round(((double)drv[originallowerMAXComp]), 3) - Math.Round(((double)drv[lowerMaxcomp]), 3)) > cut)
                    {
                        Console.WriteLine("I: Max comp values don't match at " + drv[0].ToString() + " correct:"
                          + Math.Round(((double)drv[originallowerMAXComp]), 2) + " calculated:"
                          + Math.Round(((double)drv[lowerMaxcomp]), 2) + "  A:" + Math.Round(((double)drv[lowerMaxA]), 2)
                          + "  B:" + Math.Round(((double)drv[lowerMaxB]), 2) + " comp val " + Math.Round(((double)drv[lowercomp]), 2));

                        fail1 = true;
                    }

                    if (Math.Abs(Math.Round(((double)drv[originallowerSTDComp]), 3) - Math.Round(((double)drv[lowerSTDcomp]), 3)) > cut)
                    {
                        Console.WriteLine("I: STD comp values don't match at " + drv[0].ToString() + " correct:"
                          + Math.Round(((double)drv[originallowerSTDComp]), 2) + " calculated:"
                          + Math.Round(((double)drv[lowerSTDcomp]), 2) + "  A:" + Math.Round(((double)drv[upperStdA]), 2)
                          + "  B:" + Math.Round(((double)drv[upperStdB]), 2) + " comp val " + Math.Round(((double)drv[lowercomp]), 2));

                        fail1 = true;
                    }
                    cnt++;
#endif
                }
                Console.WriteLine(cnt + " Rows parsed, lower failed " + fail2);
                Assert.IsFalse(fail1 || fail2 );

            }
        }
        [TestFixture]
        public class TestShear
        {
            ISessionColumnCollection collection=InitSettings .collection ;
            DataTable data=InitSettings .data ;
            

            [TestFixtureSetUp]
            public void init()
            {
                //calculate WD comps
                IComposite wdcomp = new WindDirectionComposite(collection, data);
                wdcomp.CalculateComposites();

                //calculate WS Comps
                IComposite wscomp = new WindSpeedComposite(collection, data);
                wscomp.CalculateComposites();
            }
            [Test]
            public void TestBulkShear()
            {
                
                IShear bulkshear = new BulkShear(collection, data.AsDataView());
                ShearCalculationGridCollection grid;
                Assert.IsTrue(bulkshear.CalculateWindSpeed (80,out grid));
                
                //get new bulk shear column 
                List<ISessionColumn> shearcol=collection.GetColumnsByType (SessionColumnType .WSAvgBulkShear );
                Assert.AreEqual(1,shearcol .Count,"bulk shear column not created or named incorrectly" );
                int shearidx=shearcol[0].ColIndex;

                DataView bulksheardata = data.AsDataView();
                bulksheardata.Sort = collection[collection.DateIndex].ColName;
                
                int bulkidx = InitSettings.originalbulkshearWS;
                bool fail=false;
                foreach (DataRowView row in bulksheardata)
                {
                   // Console.WriteLine(" bulk shear " + "  " + row[11] + " " + row[shearidx].ToString());
                    if (Math.Abs(Math.Round(((double)row[bulkidx ]), 3) - Math.Round(((double)row[shearidx]), 3)) > .01)
                    {
                        //Assert.Fail("val does not match");
                        Console.WriteLine(" bulk shear values don't match at " + row[0].ToString() + " correct :"
                          + Math.Round(((double)row[bulkidx ]), 4) + " calculated : "
                          + Math.Round(((double)row[shearidx]), 4) + " wd " + double.Parse(row[InitSettings .originalwdcomp].ToString ())
                          );
                        fail = true;
                    }

                } 
                Assert.IsFalse(fail);

            }
            [Test]
            public void Test1by24Shear()
            {
                HourShear shear = new HourShear(collection, data.AsDataView());
                
                HourAxis thisHourAxis = new HourAxis();
                shear.Xaxis = thisHourAxis;
               

                ShearCalculationGridCollection grid;
                shear.CalculateWindSpeed(80, out grid);


                //get new shear column 
                List<ISessionColumn> shearcol = collection.GetColumnsByType(SessionColumnType.WSAvgSingleAxisShear);
                Assert.AreEqual(1, shearcol.Count);
                int shearidx = shearcol[0].ColIndex;

                int uppercomp = collection.UpperWSComp(InitSettings.start);
                int lowercomp = collection.LowerWSComp(InitSettings.end);

                int wdindex = collection["WDAvgComposite"].ColIndex;
                int originalshear = InitSettings.original2by24WS;
                //sort data
                DataView XbyYshear = data.AsDataView();
                XbyYshear.Sort = collection[collection.DateIndex].ColName;

                Console.WriteLine("shear idx " + shearidx);

                //foreach (ISessionColumn c in collection.Columns)
                //{
                //    Console.WriteLine(c.ColIndex + " " + c.ColName);
                //}
                bool fail = false;

                foreach (DataRowView row in XbyYshear)
                {

                    if (Math.Abs(Math.Round(double.Parse(row[originalshear].ToString()), 3)
                        - Math.Round(double.Parse(row[shearidx].ToString()), 3)) > .01)
                    {
                        Console.WriteLine(row[0].ToString() + " correct :"
                          + Math.Round(Convert.ToDouble(row[originalshear]), 3) + " calculated : "
                          + Math.Round(double.Parse(row[shearidx].ToString()), 3) + "  wd "
                          + double.Parse(row[wdindex].ToString())
                          + " UpperComp " + double.Parse(row[uppercomp].ToString())
                          + "  Lower Comp " + double.Parse(row[lowercomp].ToString())
                          + " hour " + thisHourAxis.AxisValues[thisHourAxis.ReturnAxisValue((DateTime)row[0])]);
                        fail = true;

                    }


                }
                Assert.IsFalse(fail);
            }
            [Test]
            public void Test12by24Shear()
            {
                MonthbyHourShear shear = new MonthbyHourShear(collection, data.AsDataView());
                MonthAxis thisMonthAxis = new MonthAxis();
                HourAxis thisHourAxis = new HourAxis();
                shear.Xaxis = new MonthAxis();
                shear.Yaxis = new HourAxis();

                ShearCalculationGridCollection grid;
                shear.CalculateWindSpeed(80,out grid);

                
                //get new shear column 
                //List<ISessionColumn> shearcol = collection.GetColumnsByType(SessionColumnType.WSAvgShear );
                //Assert.AreEqual(1, shearcol.Count);
                //int shearidx = shearcol[0].ColIndex;

                //int uppercomp = collection.UpperWSComp(InitSettings.start);
                //int lowercomp = collection.LowerWSComp(InitSettings.end);

                //int wdindex = collection["WDAvgComposite"].ColIndex ;
                //int originalshear = InitSettings .original2by24WS;
                
                //sort data
                DataView XbyYshear = data.AsDataView();
                //XbyYshear.Sort = collection[collection.DateIndex].ColName;
                
                //Console.WriteLine("shear idx " + shearidx);

                //foreach (ISessionColumn c in collection.Columns)
                //{
                //    Console.WriteLine(c.ColIndex + " " + c.ColName);
                //}
                //bool fail = false;
                
                //foreach (DataRowView row in XbyYshear)
                //{

                //    if (Math.Abs(Math.Round(double.Parse(row[originalshear].ToString ()), 3) 
                //        - Math.Round(double.Parse (row[shearidx].ToString ()), 3)) > .01)
                //    {
                //        Console.WriteLine(row[0].ToString() + " correct :"
                //          + Math.Round(Convert.ToDouble ( row[originalshear]), 3) + " calculated : "
                //          + Math.Round(double.Parse(row[shearidx].ToString()), 3) + "  wd " 
                //          + double.Parse(row[wdindex].ToString())
                //          + " UpperComp " + double.Parse(row[uppercomp].ToString())
                //          + "  Lower Comp " + double.Parse(row[lowercomp].ToString ())
                //          + " Month " + thisMonthAxis.AxisValues[thisMonthAxis.ReturnAxisValue((DateTime)row[0])]
                //          + " hour " + thisHourAxis.AxisValues[thisHourAxis.ReturnAxisValue((DateTime)row[0])]);
                //        fail = true;

                //    }

                   
                //}
                //Assert.IsFalse(fail);
            }
            [Test]
            public void TestSensorRanking()
            {
                

                SortedDictionary <double,ISessionColumn> test
                    = collection.GetColumnsByType(SessionColumnType.WSAvg, DateTime.Parse("02/01/08"), true);
                Assert.AreEqual(2, test.Count, "wrong ranked ws comp count");
                Console.WriteLine("column count after ws and wd comps " + data.Columns.Count); 

            }
            [Test]
            public void TestCoincidentValuesCount()
            {
                //11590 missing 
                //76320 total 
                //64730 answer
                
                //ICoincidentValues coincident = new CoincidentValues(new List<int>() {InitSettings .originaluppercomp  ,InitSettings .originallowercomp   }, data);
                //Assert.AreEqual(64730, coincident.GetValues ()[9].Count);

                int uppercomp = collection.UpperWSComp(InitSettings.start);
                int lowercomp = collection.LowerWSComp(InitSettings.start);

                ICoincidentValues coincident = new CoincidentValues(new List<int>() 
                { uppercomp, lowercomp }, data.AsDataView ());
                Console.WriteLine(coincident.GetValues()[uppercomp  ].Count );
                
                
            }
            [Test]
            public void TestCoincidentDataSet()
            {
                var result = data.AsEnumerable().Count(c => c.Field<double>(22) >= 0 & c.Field<double>(23)>=0 & c.Field<double>(24)>=0);
                Console.WriteLine(result);

            }
   

        }
        [TestFixture]
        public class TestAxis
        {
            [Test]
            public void TestReturnAxisValues()
            {
                WindDirectionAxis  WdAxis = new WindDirectionAxis(20);
                Assert.AreEqual(18, WdAxis.AxisValues.Length);
                WdAxis.BinWidth = 30;
                Assert.AreEqual(12, WdAxis.AxisValues.Length );

                MonthAxis dateAxis = new MonthAxis();
                Assert.AreEqual(12, dateAxis.AxisValues.Length);
                Assert.AreEqual(10,dateAxis.ReturnAxisValue(DateTime.Parse("11/01/09")));

                HourAxis hourAxis = new HourAxis();
                Assert.AreEqual(24, hourAxis.AxisValues.Length);
                Assert.AreEqual(1, hourAxis.ReturnAxisValue(DateTime.Parse("11/01/09 01:00:00")));

                WindDirectionComposite  wdcomp = new WindDirectionComposite(InitSettings.collection, InitSettings.data);
                wdcomp.CalculateComposites();
                WindSpeedComposite comp=new WindSpeedComposite (InitSettings.collection ,InitSettings .data);
                comp.CalculateComposites ();

                SortedDictionary<double, ISessionColumn> wscomps
                    = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvg,
                 InitSettings.start, true);
                int uppercomp = wscomps.Values.ToList()[1].ColIndex;

                WindSpeedAxis wsaxis = new WindSpeedAxis(2, InitSettings.data.AsDataView(), uppercomp );
                Assert.AreEqual(0, wsaxis.GetRangeStart(0));
                Assert.AreEqual(1, wsaxis.GetRangeEnd(0));
                Assert.AreEqual(2, wsaxis.Incrementor);
                Assert.AreEqual(21, wsaxis.GetRangeStart(11));
                Assert.AreEqual(23, wsaxis.GetRangeEnd(11));

                
            }
            [Test]
            public void TestMonthYearAxis()
            {
                MonthYearAxis moyraxis = new MonthYearAxis(DateTime.Parse("01/01/03"), DateTime.Parse("2/27/05"));
                Assert.AreEqual(26, moyraxis.AxisValues.Length);
                Assert.AreEqual(2004, moyraxis.ReturnAxisValueYear(13));
                Assert.AreEqual(2005, moyraxis.ReturnAxisValueYear(24));
                Assert.AreEqual("2/2004", moyraxis.ReturnAxisValueHeader(13));
                foreach (int i in moyraxis.AxisValues)
                {
                    Console.WriteLine(" i " + i + " " + moyraxis.ReturnAxisValueHeader(i));
                }
            }
        }
        [TestFixture]
        public class TestFileExport
        {
            public IComposite wdcomposite = new WindDirectionComposite(InitSettings.collection, InitSettings.data);
            public IComposite wscomposite = new WindSpeedComposite(InitSettings.collection, InitSettings.data);
            public MonthbyHourShear Shear = new MonthbyHourShear(InitSettings.collection, InitSettings.data.AsDataView());
            public BulkShear BulkShear = new BulkShear(InitSettings.collection, InitSettings.data.AsDataView());
            public HourShear hourShear = new HourShear(InitSettings.collection, InitSettings.data.AsDataView());
            [Test]
            public void Export12by24CSV()
            {
                //calculate WD comps
                wdcomposite.CalculateComposites();

                //calculate WS Comps
                wscomposite.CalculateComposites();

                //IShear workbulkshear = new BulkShear(InitSettings.collection, InitSettings.data.AsDataView());
                //ShearCalculationGridCollection grid;
                //workbulkshear.CalculateWindSpeed(80,out grid);


                Shear.Xaxis = new MonthAxis();
                Shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection ogrid;
                Shear.CalculateWindSpeed(80, out ogrid);
                Shear.CalculateWindSpeed(84, out ogrid);
                //Shear.CalculateWindSpeed(120, out ogrid);

                #region assign column variables
#if(CHILDREN)
                int upperMaxA = InitSettings.upperMaxA;
                int upperStdA = InitSettings.upperSTDA;
                int upperMaxB = InitSettings.upperMaxB;
                int upperStdB = InitSettings.upperSTDB;
                int lowerMaxA = InitSettings.lowerMaxA;
                int lowerStdA = InitSettings.lowerSTDA;
                int lowerMaxB = InitSettings.lowerMaxB;
                int lowerStdB = InitSettings.lowerSTDB;

#endif
                int uppervalA = InitSettings.uppervalA;
                int uppervalB = InitSettings.uppervalB;
                int lowervalA = InitSettings.lowervalA;
                int lowervalB = InitSettings.lowervalB;

                int upperwd = InitSettings.upperwd;
                int lowerwd = InitSettings.lowerwd;

                int wdcompcol = InitSettings.collection["WDAvgComposite"].ColIndex;

                //assigned ws comps
                SortedDictionary<double, ISessionColumn> wscomps
                    = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvg,
                  InitSettings.start, true);

                int lowercomp = wscomps.Values.ToList()[0].ColIndex;
                int uppercomp = wscomps.Values.ToList()[1].ColIndex;


                //assigned std comps 
#if (CHILDREN)
                SortedDictionary<double, ISessionColumn> STDcomps = InitSettings.collection.GetColumnsByType(SessionColumnType.WSStd,
                  InitSettings.start, true);
                int lowerSTDcomp = STDcomps.Values.ToList()[0].ColIndex;
                int upperSTDcomp = STDcomps.Values.ToList()[1].ColIndex;


                //assigned max comps 
                SortedDictionary<double, ISessionColumn> Maxcomps = InitSettings.collection.GetColumnsByType(SessionColumnType.WSMax,
                  InitSettings.start, true);
                int lowerMaxcomp = Maxcomps.Values.ToList()[0].ColIndex;
                int upperMaxcomp = Maxcomps.Values.ToList()[1].ColIndex;

#endif
                // shear column 
                List<ISessionColumn> shearcol = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvgShear);
                int shear = shearcol[0].ColIndex;
                int shear1 = shearcol[1].ColIndex;
                // int shear2 = shearcol[2].ColIndex;

                // bulk shear column 
                //List<ISessionColumn> bulkshearcol = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvgBulkShear);
                //int shear = bulkshearcol[0].ColIndex;

                #endregion                 #region printcols
                List<int> printcols = new List<int>();

                #region printcols
                printcols.Add(0);
                printcols.Add(uppervalA);
#if(CHILDREN)
                printcols.Add(upperMaxA);
                printcols.Add(upperStdA);
#endif

                printcols.Add(uppervalB);
#if(CHILDREN)

                printcols.Add(upperMaxB);
                printcols.Add(upperStdB);
#endif
                printcols.Add(lowervalA);

#if(CHILDREN)

                printcols.Add(lowerMaxA);
                printcols.Add(lowerStdA);
#endif
                printcols.Add(lowervalB);

                printcols.Add(upperwd);
                printcols.Add(lowerwd);
                printcols.Add(wdcompcol);
                printcols.Add(uppercomp);
                printcols.Add(lowercomp);

#if(CHILDREN)


                printcols.Add(lowerMaxB);
                printcols.Add(lowerStdB);
                printcols.Add(lowerMaxcomp);
                printcols.Add(lowerSTDcomp);
                printcols.Add(upperMaxcomp);
                printcols.Add(upperSTDcomp);
#endif


                int bp = InitSettings.bp;
                printcols.Add(bp);

                int temp = InitSettings.temp;
                printcols.Add(temp);

                //int airden = InitSettings.airDen;
                //printcols.Add(airden);

                printcols.Add(30);
                printcols.Add(31);
                printcols.Add(32);
                printcols.Add(33);
                printcols.Add(28);
                //printcols.Add(21);
                //printcols.Add(15);



                //printcols.Add(bulkshear);
                printcols.Add(shear);
                printcols.Add(shear1);
                //printcols.Add(shear2);
                #endregion printcols

                string thissite = InitSettings.thisSite;

                string testdate = DateTime.Now.ToShortDateString();
                testdate = testdate.Replace("/", "");

                IExportFile file = new ExportFile(InitSettings.collection,
                    InitSettings.data.AsDataView(), printcols);

                file.OutputFile(InitSettings.testFolderPath + @"\TestFiles\" + thissite + "_WindART_AllData_12by24" + testdate + ".csv");

                string outputpath = InitSettings.testFolderPath;
                outputpath += @"\TestFiles\" + thissite + "_StnSummary_" + testdate + ".xlsx";
                XbyYShearStationSummary summary = new XbyYShearStationSummary(InitSettings.collection,
                    InitSettings.data.AsDataView(), 30, 10, 2, ogrid);
                summary.CreateReport(outputpath);
            }
            [Test]
            public void ExportBulkCSV()
            {
                //calculate WD comps
                wdcomposite.CalculateComposites();

                //calculate WS Comps
                wscomposite.CalculateComposites();

                ShearCalculationGridCollection ogrid;
                BulkShear.CalculateWindSpeed(80, out ogrid);

                #region assign column variables
#if(CHILDREN)
                int upperMaxA = InitSettings.upperMaxA;
                int upperStdA = InitSettings.upperSTDA;
                int upperMaxB = InitSettings.upperMaxB;
                int upperStdB = InitSettings.upperSTDB;
                int lowerMaxA = InitSettings.lowerMaxA;
                int lowerStdA = InitSettings.lowerSTDA;
                int lowerMaxB = InitSettings.lowerMaxB;
                int lowerStdB = InitSettings.lowerSTDB;
#endif
                int uppervalA = InitSettings.uppervalA;
                int uppervalB = InitSettings.uppervalB;
                int lowervalA = InitSettings.lowervalA;
                int lowervalB = InitSettings.lowervalB;

                int upperwd = InitSettings.upperwd;
                int lowerwd = InitSettings.lowerwd;

                int wdcompcol = InitSettings.collection["WDAvgComposite"].ColIndex;

                //assigned ws comps
                SortedDictionary<double, ISessionColumn> wscomps
                    = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvg,
                  InitSettings.start, true);

                int lowercomp = wscomps.Values.ToList()[0].ColIndex;
                int uppercomp = wscomps.Values.ToList()[1].ColIndex;


                //assigned std comps 
#if (CHILDREN)
                SortedDictionary<double, ISessionColumn> STDcomps = InitSettings.collection.GetColumnsByType(SessionColumnType.WSStd,
                  InitSettings.start, true);
                int lowerSTDcomp = STDcomps.Values.ToList()[0].ColIndex;
                int upperSTDcomp = STDcomps.Values.ToList()[1].ColIndex;


                //assigned max comps 
                SortedDictionary<double, ISessionColumn> Maxcomps = InitSettings.collection.GetColumnsByType(SessionColumnType.WSMax,
                  InitSettings.start, true);
                int lowerMaxcomp = Maxcomps.Values.ToList()[0].ColIndex;
                int upperMaxcomp = Maxcomps.Values.ToList()[1].ColIndex;

#endif
                // shear column 
                List<ISessionColumn> shearcol
                    = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvgBulkShear);
                int shear = shearcol[0].ColIndex;

                #endregion
                #region printcols
                List<int> printcols = new List<int>();


                printcols.Add(0);
                printcols.Add(uppervalA);
#if(CHILDREN)
                printcols.Add(upperMaxA);
                printcols.Add(upperStdA);
#endif

                printcols.Add(uppervalB);
#if(CHILDREN)

                printcols.Add(upperMaxB);
                printcols.Add(upperStdB);
#endif
                printcols.Add(lowervalA);

#if(CHILDREN)

                printcols.Add(lowerMaxA);
                printcols.Add(lowerStdA);
#endif
                printcols.Add(lowervalB);

#if(CHILDREN)

                printcols.Add(lowerMaxB);
                printcols.Add(lowerStdB);
                printcols.Add(lowerMaxcomp);
                printcols.Add(lowerSTDcomp);
                printcols.Add(upperMaxcomp);
                printcols.Add(upperSTDcomp);
#endif
                printcols.Add(upperwd);
                printcols.Add(lowerwd);
                printcols.Add(wdcompcol);

                printcols.Add(lowercomp);


                printcols.Add(uppercomp);


                printcols.Add(shear);
                #endregion

                string thissite = InitSettings.thisSite;

                string testdate = DateTime.Now.ToShortDateString();
                testdate = testdate.Replace("/", "");

                // IExportFile file = new ExportFile(InitSettings.collection,
                //    InitSettings.data.AsDataView(), printcols);

                // file.OutputFile(@"P:\JesseRichards\WindArt\C#\TestFiles\" + thissite + "_WindART_AllData_Bulk" + testdate + ".csv");

                string outputpath = InitSettings.testFolderPath;
                outputpath += @"\TestFiles\" + thissite + "_WindART_BulkStnSummary_" + testdate + ".xlsx";
                BulkShearStationSummary stationSummary = new BulkShearStationSummary(InitSettings.collection,
                    InitSettings.data.AsDataView(), 30, 10, 2);
                stationSummary.CreateReport(outputpath);
            }
            [Test]
            public void Export1by24CSV()
            {
                //calculate WD comps
                wdcomposite.CalculateComposites();

                //calculate WS Comps
                wscomposite.CalculateComposites();


                hourShear.Xaxis = new HourAxis();
                ShearCalculationGridCollection ogrid;
                hourShear.CalculateWindSpeed(80, out ogrid);

                #region assign column variables
#if(CHILDREN)
                int upperMaxA = InitSettings.upperMaxA;
                int upperStdA = InitSettings.upperSTDA;
                int upperMaxB = InitSettings.upperMaxB;
                int upperStdB = InitSettings.upperSTDB;
                int lowerMaxA = InitSettings.lowerMaxA;
                int lowerStdA = InitSettings.lowerSTDA;
                int lowerMaxB = InitSettings.lowerMaxB;
                int lowerStdB = InitSettings.lowerSTDB;
#endif
                int uppervalA = InitSettings.uppervalA;
                int uppervalB = InitSettings.uppervalB;
                int lowervalA = InitSettings.lowervalA;
                int lowervalB = InitSettings.lowervalB;

                int upperwd = InitSettings.upperwd;
                int lowerwd = InitSettings.lowerwd;

                int wdcompcol = InitSettings.collection["WDAvgComposite"].ColIndex;

                //assigned ws comps
                SortedDictionary<double, ISessionColumn> wscomps
                    = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvg,
                  InitSettings.start, true);

                int lowercomp = wscomps.Values.ToList()[0].ColIndex;
                int uppercomp = wscomps.Values.ToList()[1].ColIndex;


                //assigned std comps 
#if (CHILDREN)
                SortedDictionary<double, ISessionColumn> STDcomps = InitSettings.collection.GetColumnsByType(SessionColumnType.WSStd,
                  InitSettings.start, true);
                int lowerSTDcomp = STDcomps.Values.ToList()[0].ColIndex;
                int upperSTDcomp = STDcomps.Values.ToList()[1].ColIndex;


                //assigned max comps 
                SortedDictionary<double, ISessionColumn> Maxcomps = InitSettings.collection.GetColumnsByType(SessionColumnType.WSMax,
                  InitSettings.start, true);
                int lowerMaxcomp = Maxcomps.Values.ToList()[0].ColIndex;
                int upperMaxcomp = Maxcomps.Values.ToList()[1].ColIndex;

#endif
                // shear column 
                List<ISessionColumn> shearcol
                    = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvgSingleAxisShear);
                int shear = shearcol[0].ColIndex;

                #endregion

                # region printcols
                List<int> printcols = new List<int>();


                printcols.Add(0);
                printcols.Add(uppervalA);
#if(CHILDREN)
                printcols.Add(upperMaxA);
                printcols.Add(upperStdA);
#endif

                printcols.Add(uppervalB);
#if(CHILDREN)

                printcols.Add(upperMaxB);
                printcols.Add(upperStdB);
#endif
                printcols.Add(lowervalA);

#if(CHILDREN)

                printcols.Add(lowerMaxA);
                printcols.Add(lowerStdA);
#endif
                printcols.Add(lowervalB);

#if(CHILDREN)

                printcols.Add(lowerMaxB);
                printcols.Add(lowerStdB);
                printcols.Add(lowerMaxcomp);
                printcols.Add(lowerSTDcomp);
                printcols.Add(upperMaxcomp);
                printcols.Add(upperSTDcomp);
#endif
                printcols.Add(upperwd);
                printcols.Add(lowerwd);
                printcols.Add(wdcompcol);

                printcols.Add(lowercomp);


                printcols.Add(uppercomp);


                printcols.Add(shear);
                #endregion printcols

                string thissite = InitSettings.thisSite;

                string testdate = DateTime.Now.ToShortDateString();
                testdate = testdate.Replace("/", "");

                IExportFile file = new ExportFile(InitSettings.collection,
                    InitSettings.data.AsDataView(), printcols);

                file.OutputFile(@"P:\JesseRichards\WindArt\C#\TestFiles\" + thissite + "_WindART_AllData_1by24_" + testdate + ".csv");

                string outputpath = InitSettings.testFolderPath;
                outputpath += @"\TestFiles\" + thissite + "_WindART_1by24_StnSummary_" + testdate + ".xlsx";
                SingleAxisShearStationSummary stationSummary = new SingleAxisShearStationSummary(InitSettings.collection,
                    InitSettings.data.AsDataView(), 30, 10, 2, ogrid);
                stationSummary.CreateReport(outputpath);
            }
            [Test]
            public void ExportExcel12by24StationSummary()
            {
                string thissite = "0505";
                string testdate = DateTime.Now.ToShortDateString();
                testdate = testdate.Replace("/", "");

                string outputpath = InitSettings.testFolderPath;
                outputpath += @"\TestFiles\" + thissite + "_StnSummary_" + testdate + ".xlsx";
                Console.WriteLine(outputpath);

                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();
                Shear.Xaxis = new MonthAxis();
                Shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection grid;
                Shear.CalculateWindSpeed(120, out grid);

                XbyYShearStationSummary stationSummary = new XbyYShearStationSummary(InitSettings.collection,
                    InitSettings.data.AsDataView(), 30, 10, 2, grid);
                stationSummary.CreateReport(outputpath);

            }
            [Test]
            public void ExportExcel12by24StationSummaryUsingArrayofActions()
            {
                string thissite = "0505";
                string testdate = DateTime.Now.ToShortDateString();
                testdate = testdate.Replace("/", "");

                string outputpath = InitSettings.testFolderPath;
                outputpath += @"\TestFiles\" + thissite + "_StnSummary_" + testdate + ".xlsx";
                Console.WriteLine(outputpath);

                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();
                Shear.Xaxis = new MonthAxis();
                Shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection grid;
                Shear.CalculateWindSpeed(120, out grid);

                List<StationSummarySheetType > whatToRun = new List<StationSummarySheetType >();
                whatToRun.Add(StationSummarySheetType.ShearGrid);

                XbyYShearStationSummary stationSummary = new XbyYShearStationSummary(InitSettings.collection,
                    InitSettings.data.AsDataView(), 30, 10, 2, grid,whatToRun );

                Assert.IsNotNull(stationSummary, "shear stn summary class not being instantiated ");
                Assert.IsNotNull(outputpath,"outputpath not being set");
                

                
                stationSummary.CreateReport(outputpath);

            }
        }
        [TestFixture]
        public class TestSummaryGrid
        {
            IComposite wdcomposite = new WindDirectionComposite(InitSettings.collection, InitSettings.data);
            
            IComposite wscomposite = new WindSpeedComposite(InitSettings.collection, InitSettings.data);
            MonthbyHourShear  shear = new MonthbyHourShear(InitSettings.collection, InitSettings.data.AsDataView ());
            

            [Test]
            public void TestWindDirectionSummaryGrid()
            {
                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();
                shear.Xaxis = new MonthAxis();
                shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection ogrid;
                shear.CalculateWindSpeed(80,out ogrid);
                
                AbstractSummaryGrid grid = new SummaryGrid(InitSettings.collection, InitSettings.data,
                    SessionColumnType.WSAvgShear, new WindDirectionAxis(30),SummaryType.WD );
                Assert.IsNotNull(grid);

                List<List<SummaryGridColumn>> result = grid.CreateGrid();
                Assert.IsNotNull(result);
                Console.WriteLine("grid has " + result.Count + " rows");
                string output = string.Empty;
                foreach (List<SummaryGridColumn> c in result)
                {
                    foreach (SummaryGridColumn col in c)
                    {
                        output += " " + Math.Round(Double.Parse(col.Value.ToString()), 3);
                    }
                    output += "\n";
                }
                Console.WriteLine(output);
            }
             [Test]
            public void TestWindRoseSummaryGrid()
            {
                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();
                shear.Xaxis = new MonthAxis();
                shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection ogrid;
                shear.CalculateWindSpeed(80, out ogrid);
                
                AbstractSummaryGrid grid = new SummaryGrid(InitSettings.collection, InitSettings.data,
                    SessionColumnType.WSAvgShear, new WindDirectionAxis(10),SummaryType.WDRose  );
                Assert.IsNotNull(grid);

                List<List<SummaryGridColumn>> result = grid.CreateGrid();
                Assert.IsNotNull(result);
                Console.WriteLine("grid has " + result.Count + " rows");
                string output = string.Empty;
                foreach (List<SummaryGridColumn> c in result)
                {
                    foreach (SummaryGridColumn col in c)
                    {
                        output += " " + Math.Round(Double.Parse(col.Value.ToString()), 3);
                    }
                    output += "\n";
                }
                Console.WriteLine(output);
            }
            [Test]
            public void TestWindSpeedSummary()
            {
                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();
                SortedDictionary<double, ISessionColumn> wscomps
                    = InitSettings.collection.GetColumnsByType(SessionColumnType.WSAvg,
                 InitSettings.start, true);
                int uppercomp = wscomps.Values.ToList()[1].ColIndex;
                shear.Xaxis = new MonthAxis();
                shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection ogrid;
                shear.CalculateWindSpeed(80,out ogrid);

                AbstractSummaryGrid grid = new SummaryGrid(InitSettings.collection, InitSettings.data,
                    SessionColumnType.WSAvgShear, new WindSpeedAxis(2,InitSettings.data.AsDataView (),uppercomp ),SummaryType.WS );
                Assert.IsNotNull(grid);

                List<List<SummaryGridColumn>> result = grid.CreateGrid();
                Assert.IsNotNull(result);
                Console.WriteLine("grid has " + result.Count + " rows");
                string output = string.Empty;
                foreach (List<SummaryGridColumn> c in result)
                {
                    foreach (SummaryGridColumn col in c)
                    {
                        output += " " + Math.Round(Double.Parse(col.Value.ToString()), 3);
                    }
                    output += "\n";
                }
                Console.WriteLine(output);
            }
            [Test]
            public void TestHourSummary()
            {
                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();
                
                shear.Xaxis = new MonthAxis();
                shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection ogrid;
                shear.CalculateWindSpeed(80,out ogrid);

                AbstractSummaryGrid grid = new SummaryGrid(InitSettings.collection, InitSettings.data,
                    SessionColumnType.WSAvgShear, new HourAxis(),SummaryType.Hour );
                Assert.IsNotNull(grid);

                List<List<SummaryGridColumn>> result = grid.CreateGrid();
                Assert.IsNotNull(result);
                Console.WriteLine("grid has " + result.Count + " rows");
                string output = string.Empty;
                double outdouble = 0;
                foreach (List<SummaryGridColumn> c in result)
                {
                    foreach (SummaryGridColumn col in c)
                    {   if (double.TryParse(col.Value.ToString(),out outdouble))
                        output+= " " + Math.Round(outdouble,3);
                    }
                    output += "\n";
                }
                Console.WriteLine(output);
            }
            [Test]
            public void TestMonthSummary()
            {
                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();

                shear.Xaxis = new MonthAxis();
                shear.Yaxis = new HourAxis();
                ShearCalculationGridCollection ogrid;
                shear.CalculateWindSpeed(80, out ogrid);

                AbstractSummaryGrid grid = new SummaryGrid(InitSettings.collection, InitSettings.data,
                    SessionColumnType.WSAvgShear, new MonthAxis(),SummaryType.Month );
                Assert.IsNotNull(grid);

                List<List<SummaryGridColumn>> result = grid.CreateGrid();
                Assert.IsNotNull(result);
                Console.WriteLine("grid has " + result.Count + " rows");
                string output = string.Empty;
                double outdouble = 0;
                foreach (List<SummaryGridColumn> c in result)
                {
                    foreach (SummaryGridColumn col in c)
                    {
                        if (double.TryParse(col.Value.ToString(), out outdouble))
                            output += " " + Math.Round(outdouble, 3);
                    }
                    output += "\n";
                }
                Console.WriteLine(output);
            }
            [Test]
            public void TestDataRecoverySummary()
            {
                wdcomposite.CalculateComposites();
                wscomposite.CalculateComposites();

                shear.Xaxis = new MonthAxis();
                shear.Yaxis = new HourAxis();

                ShearCalculationGridCollection ogrid;
                shear.CalculateWindSpeed(80,out ogrid);

                Grid grid
                    =new Grid(new CreateDataRecoveryGrid 
                        (InitSettings .collection ,InitSettings .data.AsDataView ()));

                List<List<SummaryGridColumn>> result = grid.CreateGrid();
                Assert.IsNotNull(result);
                Console.WriteLine("grid has " + result.Count + " rows");

                string output = string.Empty;
                
                foreach (List<SummaryGridColumn> c in result)
                {
                    //Console.WriteLine("column count " + c.Count);
                    foreach (SummaryGridColumn col in c)
                    {

                        output += " " + col.Value;
                       
                    }
                    output += "\n";
                }
                Console.WriteLine(output);
            }

        
        }

}

