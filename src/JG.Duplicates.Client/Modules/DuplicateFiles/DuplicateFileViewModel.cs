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

namespace JG.Duplicates.Client
{
    public class DuplicateFileViewModel : INotifyPropertyChanged, IDuplicateFileViewModel
    {
        private List<FileTreeDirectory> _fileTree;
        private string _rootLocation;
        private string _searchFileTypes;

        private DelegateCommand _loadComparisonCommand;
        private bool _canLoadComparisonExecute;

        public DelegateCommand SelectedFolder { get; set; }

        private readonly IEventAggregator eventAggregator;

        public List<FileTreeDirectory> MyFileTree
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

        private FileTreeInfo _selectedFileItem;
        public FileTreeInfo SelectedFileItem
        {
            set
            {
                this._selectedFileItem = value;

                //PublishFileSelectionChanged(this._selectedFileItem.DirectoryInfo.);

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
                //var tl = tree.ToList();
                //Children = (from b in g
                //                          join c in fileTree on b.Name equals c.Name
                //                         // where c.DirectoryName != g.FirstOrDefault().DirectoryName
                //                          group c by b.DirectoryName into g2
                //                          select new FileItemInfo()
                //                          {
                //                              FileInfo = g2.FirstOrDefault(),
                //                              DirectoryName = g2.FirstOrDefault().DirectoryName,
                //                              Children = (from d in g2
                //                                          group g2 by d.Name into g3
                //                                          select new FileItemInfo()
                //                                          {
                //                                              Unknown = g3
                //                                          }).ToList()
                //                          }).ToList()


                // Group by first component (before /)

            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private IEnumerable<FileTreeDirectory> GetTreeViewModel(List<FileInfo> fileTree)
        {
            //var dirs = from s in fileTree
            //           group s by s.DirectoryName into g
            //           select new { DirName = g.FirstOrDefault().DirectoryName };

            //List<FileItemInfo> fil = new List<FileItemInfo>();
            //foreach (var item in dirs)
            //{
            //    FileItemInfo fi = new FileItemInfo() { DirectoryName = item.DirName };

            //    string childDir = "";
            //    foreach (var file in fileTree.Where(x => x.DirectoryName == item.DirName))
            //    {
            //        var tr = from s in fileTree
            //                 where s.DirectoryName != file.DirectoryName && file.Name.Contains(s.Name)
            //                 select s;

            //        FileItemInfo childDirItem = null;
            //        foreach (var dupFile in tr)
            //        {
            //            // Get distinct directories
            //            if (childDir != dupFile.DirectoryName)
            //            {
            //                if (childDir != "")
            //                {
            //                    fi.Children.Add(childDirItem);
            //                }

            //                childDirItem = new FileItemInfo() { DirectoryName = dupFile.DirectoryName, Children = new List<FileItemInfo>() };
            //            }

            //            childDirItem.Children.Add(new FileItemInfo() { FileInfo = dupFile });
            //        }
            //        fil.Add(fi);
            //    }
            //}
            //return fil;
            var test = from s in fileTree
                       from t in fileTree
                       where s.DirectoryName != t.DirectoryName && s.Name == t.Name
                       orderby s.DirectoryName, t.DirectoryName
                       select new FileItemInfoFlat { DirectoryNameA = s.DirectoryName, DirectoryNameB = t.DirectoryName, FileInfo = s };

            var tree = from s in test
                       group s by s.DirectoryNameA into grpA
                       select new FileTreeDirectory()
                       {
                           DirectoryName = grpA.FirstOrDefault().DirectoryNameA,
                           DirList = (from v in test
                                      where v.DirectoryNameA == grpA.FirstOrDefault().DirectoryNameA
                                      group v by v.DirectoryNameB into grpB
                                      select new FileDirectoryInfo()
                                      {
                                          DirectoryName = grpB.FirstOrDefault().DirectoryNameB,
                                          Children = (from t in test
                                                      where t.DirectoryNameA == grpB.FirstOrDefault().DirectoryNameA
                                                      && t.DirectoryNameB == grpB.FirstOrDefault().DirectoryNameB
                                                      select new FileItemInfo()
                                           {
                                               DirectoryName = t.DirectoryNameB,
                                               FileInfo = t.FileInfo
                                           }).ToList()
                                      }).ToList()
                       };
            var x = 1;
            //var tree =
            //          from s in fileTree
            //          group s by s.DirectoryName into g
            //          select new FileItemInfo()
            //          {
            //              DirectoryName = g.FirstOrDefault().DirectoryName,
            //              FileInfo = g.FirstOrDefault(),
            //              Unknown = g,
            //              Children = (from c in fileTree
            //                          where c.Name.Contains(g.FirstOrDefault().Name)
            //                          // && c.DirectoryName != g.FirstOrDefault().DirectoryName
            //                          group c by c.DirectoryName into g2
            //                          select new FileItemInfo()
            //                          {
            //                              Unknown = g2, // this should sovle the issue
            //                              FileInfo = g2.FirstOrDefault(),
            //                              DirectoryName = g2.FirstOrDefault().DirectoryName,
            //                              Children = g2.Select(t => new FileItemInfo() { FileInfo = t }).ToList()
            //                          }).ToList()
            //          };
            return tree;
        }

        private void PublishFileSelectionChanged(FileInfo fileInfo)
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
    }

    public class FileTreeDirectory
    {
        public string DirectoryName { get; set; }

        public List<FileDirectoryInfo> DirList { get; set; }
    }

    public class FileDirectoryInfo
    {
        public string DirectoryName { get; set; }

        public List<FileItemInfo> Children { get; set; }
    }

    public class FileItemInfo
    {
        public string DirectoryName { get; set; }

        public FileInfo FileInfo { get; set; }
    }

    public class FileItemInfoFlat
    {
        public string DirectoryNameA { get; set; }

        public string DirectoryNameB { get; set; }

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
