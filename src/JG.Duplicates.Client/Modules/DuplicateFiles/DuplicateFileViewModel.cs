using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JG.DuplicateFiles;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using JG.Duplicates.Client.Modules;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Unity;
using JG.Duplicates.Client.Events;
using System.IO;
using System.Collections;
using JG.Duplicates.Client.Behaviours;
using JG.DuplicateFiles.Engine;

namespace JG.Duplicates.Client
{
    public class DuplicateFileViewModel : INotifyPropertyChanged, IDuplicateFileViewModel, IDraggable
    {
        private List<DuplicateFileTree> _fileTree;
        private string _rootLocation;
        private string _searchFileTypes;

        private DelegateCommand _loadComparisonCommand;
        private bool _canLoadComparisonExecute;

        public DelegateCommand SelectedFolder { get; set; }

        private readonly IEventAggregator eventAggregator;

        public List<DuplicateFileTree> MyFileTree
        {
            set
            {
                this._fileTree = value;
                OnPropertyChanged("MyFileTree");
            }
            get
            {
                return this._fileTree;
            }
        }

        public string RootLocation
        {
            set
            {
                this._rootLocation = value;
                OnPropertyChanged("RootLocation");
            }
            get
            {
                return this._rootLocation;
            }
        }

        public string SearchFileTypes
        {
            set
            {
                this._searchFileTypes = value;
                OnPropertyChanged("SearchFileTypes");
            }
            get
            {
                return this._searchFileTypes;
            }
        }

        public string[] SearchFileTypesList
        {
            get
            {
                try
                {
                    string[] splitSearchFileTypeList = this.SearchFileTypes.Split('|');

                    return splitSearchFileTypeList;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Invalid File Type format entered.");
                }
            }
        }

        private ITreeItem _selectedFileItem;
        public ITreeItem SelectedFileItem
        {
            set
            {
                this._selectedFileItem = value;

                //PublishFileSelectionChanged(this._selectedFileItem);

                OnPropertyChanged("SelectedFileItem");
            }
            get
            {
                return this._selectedFileItem;
            }
        }

        public DelegateCommand LoadRootComparisonClickCommand
        {
            get
            {
                return _loadComparisonCommand ?? (_loadComparisonCommand = new DelegateCommand(
                async () =>
                {
                    await LoadRootComparisonAsync();
                }, CanLoadComparisonExecute));

                //return _clickCommand ?? (_clickCommand = new CommandHandler(() => LoadRootComparison(), _canExecute));
            }
        }

        public bool IsLoading
        {
            get
            {
                return !this._canLoadComparisonExecute;
            }
            set
            {
                this._canLoadComparisonExecute = !value;
                this.LoadRootComparisonClickCommand.RaiseCanExecuteChanged();
            }
        }

        public DuplicateFileViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            this.Initialize();
        }

        private void Initialize()
        {
            this.RootLocation = @"C:\Temp\Pics";
            this.SearchFileTypes = @".jpg|.bmp|.jpeg";

            this._canLoadComparisonExecute = true;
        }

        private bool CanLoadComparisonExecute()
        {
            return _canLoadComparisonExecute;
        }

        public Task LoadRootComparisonAsync()
        {
            return Task.Run(() => LoadRootComparison());
        }

        public void LoadRootComparison()
        {
            try
            {
                this.IsLoading = true;

                var fileTree = new FileTree(this.RootLocation, this.SearchFileTypesList).FileTreeList;

                var tree = GetTreeViewModel(fileTree);
                this.MyFileTree = tree.ToList();
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private IEnumerable<DuplicateFileTree> GetTreeViewModel(List<FileInfo> fileTree)
        {
            System.Diagnostics.Stopwatch objStopWatch = new System.Diagnostics.Stopwatch();

            //objStopWatch.Start();
            // Get a cross join of the files that match and where the directories are different
            // This will then be used to create the tree structure
            //var crossJoinFile = from s in fileTree
            //                    from t in fileTree
            //                    where s.DirectoryName != t.DirectoryName && s.Name == t.Name
            //                    orderby s.DirectoryName, t.DirectoryName
            //                    select new FileItemInfoFlat { DirectoryNameFirst = s.DirectoryName, DirectoryNameSecond = t.DirectoryName, FileInfo = s };
            
            //Console.WriteLine(string.Format("Time Taken Cross Join: {0}", objStopWatch.Elapsed.TotalSeconds));

            //objStopWatch.Reset();
            objStopWatch.Start();

            var crossJoinFile = from s in fileTree
                                join t in fileTree on s.Name equals t.Name
                                where s.DirectoryName != t.DirectoryName
                                orderby s.DirectoryName, t.DirectoryName
                                select new FileItemInfoFlat { DirectoryNameFirst = s.Directory, DirectoryNameSecond = t.Directory, FileInfo = s };


            //var crossJoinFileTest = fileTree.SelectMany(t1 => fileTree.Select(t2 => Tuple.Create(t1, t2)))
            //    .Where(t => t.Item1.DirectoryName == t.Item2.DirectoryName && t.Item1.Name == t.Item2.Name)
            //    .Select(v => new FileItemInfoFlat { DirectoryNameFirst = v.Item1.DirectoryName, DirectoryNameSecond = v.Item2.DirectoryName, FileInfo = v.Item1 }).ToList();

            Console.WriteLine(string.Format("Time Taken Inner Join Execution: {0}", objStopWatch.Elapsed.TotalSeconds));
            objStopWatch.Reset();
            objStopWatch.Start();
           
            // Process to list initially as this is more optimal than to do this later on as it would be done 3 times if it were deferred to the next statement
            var crossJoinFileList = crossJoinFile.ToList();
            Console.WriteLine(string.Format("Time Taken Cross Join Execution: {0}", objStopWatch.Elapsed.TotalSeconds));

            objStopWatch.Reset();
            objStopWatch.Start();


            var test = from s in crossJoinFileList
                       group s by s.DirectoryNameFirst.Name into grpA
                       select new 
                       {
                           DirectoryName = grpA.FirstOrDefault().DirectoryNameFirst
                       };

            // Create a tree view from the cross join. Each child will be have its own cross join set which is joined to the parent.
            // The comparison is based on the directory and file name. 
            // TODO: This can be improved so that files that are the same but have different name can be compared.
            var tree = from s in crossJoinFileList
                       group s by s.DirectoryNameFirst.Name into grpA
                       select new DuplicateFileTree()
                       {
                           DirectoryName = grpA.FirstOrDefault().DirectoryNameFirst,
                           DirList = (from v in crossJoinFileList
                                      where v.DirectoryNameFirst.Name == grpA.FirstOrDefault().DirectoryNameFirst.Name
                                      group v by v.DirectoryNameSecond.Name into grpB
                                      select new DuplicateDirectoryList()
                                      {
                                          DirectoryName = grpB.FirstOrDefault().DirectoryNameSecond,
                                          Children = (from t in crossJoinFileList
                                                      where t.DirectoryNameFirst.Name == grpB.FirstOrDefault().DirectoryNameFirst.Name
                                                      && t.DirectoryNameSecond.Name == grpB.FirstOrDefault().DirectoryNameSecond.Name
                                                      select new FileItemInfo()
                                           {
                                               DirectoryName = t.DirectoryNameSecond,
                                               FileInfo = t.FileInfo
                                           }).ToList()
                                      }).ToList()
                       };

            Console.WriteLine(string.Format("Time Taken Tree: {0}", objStopWatch.Elapsed.TotalSeconds));

            return tree;
        }

        private void PublishFileSelectionChanged(ITreeItem fileInfo)
        {
            // Publish the events.
            this.eventAggregator
                .GetEvent<FileSelectionEvent>()
                .Publish(new FileSelectionEventArgs(fileInfo));
        }

        //public List<Node> BuildTree(IEnumerable<FileInfo> fileInfo, int parentId)
        //{
        //    return (
        //      from s in fileInfo
        //      group s by s.DirectoryName into g  // Group by first component (before /)
        //      select new Node
        //      {
        //          Name = g.Key,
        //          Children = BuildTree(            // Recursively build children
        //            from s in g
        //            select s, ) // Select remaining components
        //      }
        //      ).ToList();
        //}


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members

        public object DragItem
        {
            get { return this.SelectedFileItem; }
        }
    }

    public class FileItemInfoFlat
    {
        public DirectoryInfo DirectoryNameFirst { get; set; }

        public DirectoryInfo DirectoryNameSecond { get; set; }

        public FileInfo FileInfo { get; set; }

    }


    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;
        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
