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

namespace JG.Duplicates.Client
{
    public class DuplicateFileViewModel : INotifyPropertyChanged, IDuplicateFileViewModel
    {
        private List<FileTreeInfo> _fileTree;
        private string _rootLocation;
        private string _searchFileTypes;

        private DelegateCommand _loadComparisonCommand;
        private bool _canLoadComparisonExecute;

        public DelegateCommand SelectedFolder { get; set; }

        private readonly IEventAggregator eventAggregator;

        public List<FileTreeInfo> MyFileTree
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

                PublishFileSelectionChanged(this._selectedFileItem.FileInfo);

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
            this.RootLocation = @"\\justin-nas\AllDisk\Pictures\Wedding";
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

        private void LoadRootComparison()
        {
            try
            {
                this.IsLoading = true;

                this.MyFileTree = new FileTree(this.RootLocation, this.SearchFileTypesList).FileTreeList;
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        private void PublishFileSelectionChanged(FileInfo fileInfo)
        {
            // Publish the events.
            this.eventAggregator
                .GetEvent<FileSelectionEvent>()
                .Publish(new FileSelectionEventArgs(fileInfo));
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members
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
