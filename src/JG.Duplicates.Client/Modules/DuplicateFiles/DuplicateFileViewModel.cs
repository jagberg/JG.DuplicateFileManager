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

namespace JG.Duplicates.Client
{
    public class DuplicateFileViewModel : INotifyPropertyChanged, IDuplicateFileViewModel
    {
        private List<FileTreeInfo> _fileTree;
        private string _rootLocation;

        private ICommand _clickCommand;
        private ICommand _fileSelectionChangedCommand;
        private bool _canExecute;

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

        public ICommand LoadRootComparisonClickCommand
        {
            get
            {
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => LoadRootComparison(), _canExecute));
            }
        }


        public ICommand FileSelectionChangedCommand
        {
            get
            {
                return _fileSelectionChangedCommand ?? (_fileSelectionChangedCommand = new CommandHandler(() => FileSelectionChanged(), _canExecute));
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

            this._canExecute = true;
        }

        private void LoadRootComparison()
        {
            try
            {
                this._canExecute = false;

                this.MyFileTree = new FileTree(this.RootLocation).FileTreeList;
            }
            finally
            {
                this._canExecute = true;
            }
        }

        private void FileSelectionChanged()
        {
            // Publish the events.
            this.eventAggregator
                .GetEvent<FileSelectionEvent>()
                .Publish(new FileSelectionEventArgs());
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
