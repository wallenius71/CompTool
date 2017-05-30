using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WindART;
using WindART.DAL;
using System.Data;
using System.ComponentModel;
using System.Windows.Controls;
using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.Reflection;
using System.Media; 





namespace WindART_UI
{
    public class Window1ViewModel:ViewModelBase,IDropTarget,INotifyCollectionChanged 
    {
        #region fields
        RelayCommand _loadFileCommand;
        RelayCommand _processCommand;
        RelayCommand _LoadConfigCommand;
        RelayCommand _OutputFileCommand;
        RelayCommand _setOutputFileLocationCommand;
        RelayCommand _bulkEditColTypeCommand;
        RelayCommand _saveConfigCommand;
        RelayCommand _loadPersistedConfigCommand;


        DataTable data;
        ObservableCollection<ISessionColumn> _liveCollection;
        SessionColumnCollection _sessionColumnCollection;
        private DateTime _dataSetStartDate;
        private  DateTime _dataSetEndDate;
        private BackgroundWorker worker;
        private bool _fileIsLoading;
        private string _fileProgress;
        private bool _hasColumnCollection;
        private string  _heightToShearTo;
        private List<double> _heightToShearToList=new List<double> ();
        private string _outputFileLocation;
        private string _OutputSummaryFileLocation;
        private string _NameOfThisSite;
        private ShearCalculationGridCollection _ogrid;
        private bool _isProcessing;
        private bool _doNotRunComps;
        

        #endregion

        #region properties

        public SessionColumnCollection ColumnCollection
        {
            get
            {
                return _sessionColumnCollection;
            }
            set
            {
                _sessionColumnCollection = value;
                OnPropertyChanged("ColumnCollection");
                if (_sessionColumnCollection.Columns != null)
                    HasColumnCollection = true;
                else
                    HasColumnCollection = false;
            }
        }
        
        public ObservableCollection  <ISessionColumn > LiveCollection
        {
            get
            {
                
                return _liveCollection; 
            }
            set
            {
                _liveCollection  = value;

                _sessionColumnCollection .Columns .Clear ();
                foreach (ISessionColumn sc in _liveCollection)
                    _sessionColumnCollection.Columns.Add(sc);

                OnPropertyChanged("LiveCollection");
             
            }

        }

        public bool DoNotRunComps
        {
            get 
            {
                return _doNotRunComps;
            }
            set
            {
                _doNotRunComps = value;
                OnPropertyChanged("DoNotRunComps");
            }
        }
        public DateTime DataSetStartDate
        {
            get { return _dataSetStartDate; }
            set { 
                _dataSetStartDate = value;
                OnPropertyChanged("DataSetStartDate");
            }
        }
        public DateTime DataSetEndDate
        {
            get { return _dataSetEndDate ;}
            set
            {
                _dataSetEndDate = value;
                OnPropertyChanged("DataSetEndDate");
            }
        }
        public bool FileIsLoading
        {
            get
            {
                return _fileIsLoading;
            }
            private set
            {
                _fileIsLoading = value;
                OnPropertyChanged("FileIsLoading");
            }

        }
        public bool IsProcessing
        {
            get
            {
                return _isProcessing;
            }
            private set
            {
                _isProcessing = value;
                OnPropertyChanged("IsProcessing");
            }

        }
        public string FileProgressText
        {
            get
            {
                return _fileProgress;
            }
            private set
            {
                _fileProgress = value;
                OnPropertyChanged("FileProgressText");
            }
        }
        public bool HasColumnCollection
        {
            get { return _hasColumnCollection; }
            private set 
            {
                _hasColumnCollection = value;
                OnPropertyChanged("HasColumnCollection");
            }
        }
        public string  HeightToShearTo
        {
            get
            {
                return _heightToShearTo;
            }
            set
            {
                _heightToShearTo = value;
                foreach (string s in _heightToShearTo.Split(','))
                {
                    double d = 0.0;
                    if(double.TryParse (s,out d))
                    {
                        _heightToShearToList .Add(d);
                    }
                    else
                    {
                        MessageBox.Show("Error Processing Height List. Must be a comma seperated list of numeric values");
                    }
                }
                OnPropertyChanged("HeightToShearTo");
            }
        }
        public string OutputFileLocation
        {
            get
            { return _outputFileLocation; }
            set
            {
                if (value != null)
                {
                    _outputFileLocation = value;
                    OnPropertyChanged("OutputFileLocation");
                }
            }
        }
        public string OutputSummaryFileLocation
        {
            get
            {
                return _OutputSummaryFileLocation;
            }
            set
            {
                _OutputSummaryFileLocation = value;
                OnPropertyChanged("OutputSummaryFileLocation");
            }
        }
        public string NameOfThisSite
        {
            get
            {
                return _NameOfThisSite;
            }
            set
            {
                if (value != null)
                {
                    _NameOfThisSite = value;
                    OnPropertyChanged("NameOfThisSite");

                }
            }
        }
        public SelectionList<SelectionItem <StationSummarySheetType> > SummarySheets { get; private set; }
        
        

               
        #endregion

        public Window1ViewModel()
        {

            List<SelectionItem<StationSummarySheetType>> stypes = new List<SelectionItem < StationSummarySheetType>>();
            
            foreach (StationSummarySheetType s in Enum.GetValues (typeof(StationSummarySheetType )))
            {
                stypes.Add(new SelectionItem <StationSummarySheetType >(s));
            }
            SummarySheets = new SelectionList<SelectionItem<StationSummarySheetType>>(stypes);

            

            
          
        }

        #region commands

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to load data from a file.
        /// </summary>
        public ICommand LoadFileCommand
        {
            get
            {
                if (_loadFileCommand== null)
                    _loadFileCommand= new RelayCommand(param => this.LoadFile(),param=>this.CanLoadFile ());

                return _loadFileCommand;
            }
        }
        public ICommand SaveConfigCommand
        {
            get
            {
                if (_saveConfigCommand == null)
                    _saveConfigCommand = new RelayCommand(param => this.SaveConfig(), param => this.CanSaveConfig());

                return _saveConfigCommand;
            }
        }
        public ICommand LoadPersistedConfigCommand
        {
            get
            {
                if (_loadPersistedConfigCommand == null)
                    _loadPersistedConfigCommand = new RelayCommand(param => this.LoadPersistedConfig(), param => this.CanLoadPersistedConfig());

                return _loadPersistedConfigCommand;
            }
        }
        public ICommand BulkEditColTypeCommand
        {
            get
            {
                if (_bulkEditColTypeCommand == null)
                    _bulkEditColTypeCommand = new RelayCommand(param => this.BulkEditColType(param), param => this.CanBulkEditColType(param));

                return _bulkEditColTypeCommand;
            }
        }
        public ICommand ProcessCommand
        {
            get
            {
                if (_processCommand == null)
                    _processCommand = new RelayCommand(param => this.RunProcessing(),param=>this.CanRunProcessing ());

                return _processCommand;
            }
        }
        public ICommand LoadConfigCommand
        {
            get
            {
                if (_LoadConfigCommand == null)
                    _LoadConfigCommand = new RelayCommand( param   => this.LoadConfig(param));
                
                return _LoadConfigCommand ;
            }
            
        }
        public ICommand OutputFileCommand
        {
            get
            {
                if (_OutputFileCommand == null)
                    _OutputFileCommand = new RelayCommand(param => this.OutputFile(),param=>this.CanOutputFile ());

                return _OutputFileCommand;
            }
        }
        public ICommand SetOutputFileLocationCommand
        {
            get
            {
                if (_setOutputFileLocationCommand == null)
                    _setOutputFileLocationCommand = new RelayCommand( param   => this.SetOutputFileLocation());
                
                return _setOutputFileLocationCommand ;
            }
        }

        #endregion 

        #region methods

        void LoadFile()
        {
            
            string filename = Utils.GetFile();
            if (filename.Length < 1)
                return;
            
            FileIsLoading  = true;

            try
            {
                worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;

                DataRepository repository
                      = new DataRepository(filename, DataSourceType.CSV);

                
                repository.FileOpening += new DataRepository.FileProgressEventHandler(repository_UpdateProgress);
                repository.FileLoading += new DataRepository.FileProgressEventHandler(repository_UpdateProgress);
                repository.FileLoaded += new DataRepository.FileProgressEventHandler(repository_UpdateProgress);


                worker.DoWork += delegate(object sender, DoWorkEventArgs args)
                {
                    data = repository.GetAllData();

                    //reset composite flags since we're loading a new dataset
                   // WDCompositeExists = false;
                    //WSCompositeExists = false;
                    
                    ColumnCollection   = new SessionColumnCollection(data);
                    ObservableCollection <ISessionColumn > colClxn= new MultiThreadObservableCollection<ISessionColumn>();
                    foreach (SessionColumn sc in _sessionColumnCollection.Columns)
                        colClxn.Add(sc);

                    LiveCollection = colClxn;
                    

                    DataSetStartDate = ColumnCollection.DataSetStart;
                    DataSetEndDate = ColumnCollection.DataSetEnd;
                    FileIsLoading  = false;
                };

                worker.RunWorkerAsync();


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            

        }
        bool CanLoadFile()
        {
            if (_fileIsLoading || _isProcessing )
                return false;
            else
                return true;
        }
        void SaveConfig() 
        {
            string filename = Utils.GetFile();
            if (filename.Length < 1)
                return;
            PersistSettings.Save(LiveCollection, filename);
        }
        bool CanSaveConfig() 
        {
            if(data==null)return false;
            else
            return true;
        }
        void LoadPersistedConfig()
        {
            string filename=Utils.GetFile();
            if (filename == null)
                return;

            object loadedobject = PersistSettings.Load(filename);
            if (loadedobject != null)
            {
                LiveCollection  = (ObservableCollection<ISessionColumn>)loadedobject;
            }

        }
        bool CanLoadPersistedConfig()
        {
            if (data == null||_isProcessing ) return false;
            else
            return true;
        }
        void BulkEditColType(Object sender)
        {
            DependencyObject o = (DependencyObject)sender;
            ComboBox cbo = VisualTreeExtensions.GetVisualDescendent<ComboBox>(o);
            List<TreeViewItem> tvi = VisualTreeExtensions.GetVisualDescendents<TreeViewItem>(o).ToList ();
            
            foreach (TreeViewItem item in tvi)
            {
                CheckBox chk = VisualTreeExtensions.GetVisualDescendent<CheckBox>(item);
                if (chk.IsChecked == true)
                { 
                    SessionColumn col = (SessionColumn)item.DataContext;
                    col.ColumnType = (SessionColumnType)cbo.SelectedValue ;
                    chk.IsChecked = false;
                }
            }
            
        }
        bool CanBulkEditColType(Object sender)
        {
            if (sender == null) return false;
            DependencyObject o = (DependencyObject)sender;
            List<TreeViewItem> tvi = VisualTreeExtensions.GetVisualDescendents<TreeViewItem>(o).ToList();

            foreach (TreeViewItem item in tvi)
            {
                if (VisualTreeExtensions.GetVisualDescendent<CheckBox>(item).IsChecked == true)
                {
                    return true;
                }
            }
            return false;

        }
        void RunProcessing()
        {
            BackgroundWorker worker = new BackgroundWorker();
            
                    WindDirectionComposite  wdcomposite = new WindDirectionComposite(_sessionColumnCollection, data);
                    WindSpeedComposite wscomposite = new WindSpeedComposite(_sessionColumnCollection, data);
                    //SoundPlayer sp = new SoundPlayer("Careless_whisper_(Wham).wav");
                   
            try
            {
                worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
                {
                    MessageBox.Show("Done Processing!");
                    //sp.Stop();
                };
                worker.DoWork += delegate(object s, DoWorkEventArgs args)
                {
                    try
                    {
                        IsProcessing = true;
                        
                        //sp.Play();
                        if (!DoNotRunComps)
                        {
                            wdcomposite.NewCompositeColumnAdded += new WindDirectionComposite.ProgressEventHandler(repository_UpdateProgress);
                            if (wdcomposite.CalculateComposites())
                            {
                                LiveCollection = new MultiThreadObservableCollection<ISessionColumn>(_sessionColumnCollection.Columns);
                            }


                            wdcomposite.CompletedWindDirectionCompositeValues += new WindDirectionComposite.ProgressEventHandler(repository_UpdateProgress);



                            wscomposite.DeterminingWindSpeedCompositeValues += new WindSpeedComposite.ProgressEventHandler(repository_UpdateProgress);
                            if (wscomposite.CalculateComposites())
                            {

                                LiveCollection = new MultiThreadObservableCollection<ISessionColumn>(_sessionColumnCollection.Columns);
                            }
                            wscomposite.CompletedWindSpeedCompositeValues += new WindSpeedComposite.ProgressEventHandler(repository_UpdateProgress);
                        }
                        else
                        {
                            //set all the Ws sensors to comps 
                            foreach (SessionColumn sc in _sessionColumnCollection.Columns)
                            {
                                if (sc.ColumnType == SessionColumnType.WSAvg) sc.IsComposite = true;
                                if (sc.ColumnType == SessionColumnType.WDAvg)
                                {
                                    sc.ColName = "WDAvgComposite";
                                    sc.IsComposite = true;
                                }
                                
                            }
                        }

                        MonthbyHourShear Shear = new MonthbyHourShear(_sessionColumnCollection, data.AsDataView());
                        foreach(double height in _heightToShearToList )
                        {
                            Shear.Xaxis = new MonthAxis();
                            Shear.Yaxis = new HourAxis();
                            Shear.AddingShearValues +=new MonthbyHourShear.ProgressEventHandler(repository_UpdateProgress);

                            Shear.CalculateWindSpeed(height, out _ogrid);
                            LiveCollection = new MultiThreadObservableCollection<ISessionColumn>(_sessionColumnCollection.Columns);

                        }
                        Shear.AddedShearValues +=new MonthbyHourShear.ProgressEventHandler(repository_UpdateProgress);
                        IsProcessing = false;
                    }
                    finally
                    {
                        IsProcessing = false;
                    }
                    
                };
                worker.RunWorkerAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
            

        }
        bool CanRunProcessing()
        {
            if (_fileIsLoading)
                return false;
            else
                return true;

        }
        void LoadConfig(Object sender)
        {
            DependencyObject item = VisualTreeHelper.GetParent((DependencyObject)sender);
            
            while (item.GetType() != typeof(TreeViewItem ))
            {
                if (item == null) return;
                item = VisualTreeHelper.GetParent(item);
            }
            
            ItemsControl itemParent = ItemsControl.ItemsControlFromItemContainer(item);
            object SourceItem ;
            if (itemParent == null)
            {
                TreeViewItem tv = (TreeViewItem)item;
                SourceItem = tv.DataContext ;
            }
            else
            {
                SourceItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
            }
            if (SourceItem is SessionColumn)
            {
                //if the user has selected wsavg as the column type then create a windspeed sensor config
                //other wise create a regular sensor config 

                SessionColumn col = (SessionColumn)SourceItem;
                if (col.ColumnType == SessionColumnType.WSAvg)
                {
                    WindSpeedConfig config = new WindSpeedConfig();
                    config.StartDate = DataSetStartDate;
                    config.EndDate = DataSetEndDate;
                    _sessionColumnCollection [col.ColName].ColumnType = col.ColumnType ;
                    _sessionColumnCollection [col.ColName].addConfig(config);

                }
                else
                {
                    SensorConfig config = new SensorConfig();
                    config.StartDate = DataSetStartDate;
                    config.EndDate = DataSetEndDate;
                    _sessionColumnCollection [col.ColName].ColumnType = col.ColumnType ;
                    _sessionColumnCollection [col.ColName].addConfig(config);
                }
                
            }
            else
            {
                throw new ApplicationException("Type passed in must be a SessionColumn. ViewModel1.LoadConfig");
            }
            
        }
        void SetOutputFileLocation()
        {
            string workFileLocation = Utils.GetFolder();
            string outputDate = DateTime.Now.ToShortDateString();
            outputDate = outputDate.Replace("/", "");
            OutputFileLocation = workFileLocation + "\\" + NameOfThisSite + "_WindART_AllData_12by24" + outputDate + ".csv";
            OutputSummaryFileLocation =  workFileLocation + "\\" + NameOfThisSite + "_StnSummary_" + outputDate + ".xlsx";
        }
        void OutputFile()
        {
            List<int> printcols=new List<int>();

            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs e)
            {
                MessageBox.Show("File output complete");
            };
            worker.DoWork += delegate(object s, DoWorkEventArgs args)
                {
                    printcols=GetPrintCols (LiveCollection );

                    IExportFile file = new ExportFile(_sessionColumnCollection,
                        data.AsDataView(), printcols);

                    file.OutputFile(OutputFileLocation);

                    
                    List<StationSummarySheetType> runSheets = new List<StationSummarySheetType>();
                    foreach (SelectionItem <StationSummarySheetType > Item in SummarySheets.selectedItems)
                    {
                        runSheets.Add(Item.SelectedItem);
                    }
                    
                    XbyYShearStationSummary summary = new XbyYShearStationSummary(_sessionColumnCollection ,
                        data.AsDataView(), 30, 10, 2, _ogrid,runSheets  );

                    summary.CreateReport(OutputSummaryFileLocation );
                };
            worker.RunWorkerAsync ();

        }
        bool CanOutputFile()
        {
            if (HasColumnCollection ==false|| OutputFileLocation == string.Empty || OutputSummaryFileLocation == string.Empty
                || FileIsLoading || IsProcessing)
                return false;
            else
                return true;
        }

        void repository_UpdateProgress(string msg)
        {
            FileProgressText = msg;
        }
        
        public virtual  List<int> GetPrintCols(IList<ISessionColumn> cols)
        {//return the indexes of the columns to be output
            //a column can only have 1 child right now 
            
            List<int> result=new List<int>();
            
            foreach (SessionColumn sc in cols)
            {
                result.Add(sc.ColIndex);
                //get child columns
                foreach (SessionColumn innercol in sc.ChildColumns)
                {
                    result.Add(innercol.ColIndex );
                }

            }

            if (result.Count > 0)
                return result;
            else
                return null;
            
            
        }

        
        #endregion

        #region IDropTarget Members

        public void DragOver(DropInfo dropInfo)
        {
            ISessionColumn source = (ISessionColumn)dropInfo.DragInfo.SourceItem;
            if (dropInfo.InTarget & source.ColName !="DateTime" )
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                
            }
            else
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                
            }
            dropInfo.Effects = DragDropEffects.Move;

        }

        public void Drop(DropInfo dropInfo)
        {
                ISessionColumn source = (ISessionColumn)dropInfo.DragInfo.SourceItem;
                ISessionColumn target = (ISessionColumn)dropInfo.TargetItem;
                IList targetCollection = (IList)dropInfo.TargetCollection;
                IList sourceCollection=(IList)dropInfo.DragInfo.SourceCollection ;
                int index=dropInfo.InsertIndex;

                if (source.ColName  == "DateTime")
                {
                    sourceCollection.RemoveAt(sourceCollection.IndexOf(source));
                    targetCollection.Insert(index++, source);
                    
                }
                else
                {
                    if (!IsChild(dropInfo.VisualTargetItem) && (dropInfo.TargetItem !=dropInfo.DragInfo.SourceItem))
                    {
                        if (dropInfo.InTarget)
                        {
                            sourceCollection.RemoveAt(sourceCollection.IndexOf(source));
                            target.ChildColumns.Add(source);
                            
                        }
                        else
                        {
                            sourceCollection.RemoveAt(sourceCollection.IndexOf(source));
                            targetCollection.Insert(index++, source);

                        }

                        
                    }

                }
        }
        
        protected  bool IsChild(UIElement targetItem)
        {
            //if the itemscontrol of this item is a treeview then it is a root element 
            //if not and it is not null then it is assumed to be a child node
            ItemsControl parent = ItemsControl.ItemsControlFromItemContainer(targetItem);
            return parent !=null & parent.GetType() !=typeof ( TreeView );
            
        }

        protected  bool TestCompatibleTypes(IEnumerable target, object data)
        {
            TypeFilter filter = (t, o) =>
            {
                return (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            };

            var enumerableInterfaces = target.GetType().FindInterfaces(filter, null);
            var enumerableTypes = from i in enumerableInterfaces select i.GetGenericArguments().Single();

            if (enumerableTypes.Count() > 0)
            {
                Type dataType = TypeUtilities.GetCommonBaseClass(ExtractData(data));
                return enumerableTypes.Any(t => t.IsAssignableFrom(dataType));
            }
            else
            {
                return target is IList;
            }
        }

        protected static IList GetList(IEnumerable enumerable)
        {
            if (enumerable is ICollectionView)
            {
                return ((ICollectionView)enumerable).SourceCollection as IList;
            }
            else
            {
                return enumerable as IList;
            }
        }

        protected static IEnumerable ExtractData(object data)
        {
            if (data is IEnumerable && !(data is string))
            {
                return (IEnumerable)data;
            }
            else
            {
                return Enumerable.Repeat(data, 1);
            }
        }

        #endregion

        #region INotifyCollectionChanged Members

        

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedAction action, ObservableCollection <ISessionColumn> columns)
        {

            if (CollectionChanged != null)
            {
                if(this.CollectionChanged !=null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs (action,columns));

            }

        }

        #endregion
    }

    public class CheckableSessionColumn :ViewModelBase 
    {
        //extends  session column to include a checkbox 
        //declared protected class because only intended for use in view model

        private bool? _isChecked;
        public SessionColumn SessCol{get;private set;}

        public CheckableSessionColumn (SessionColumn col)
        {
            SessCol = col;
        }
        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }
    }
}
