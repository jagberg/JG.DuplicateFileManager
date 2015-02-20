using JG.DuplicateFiles;
using JG.DuplicateFiles.Domain;
using JG.Duplicates.Client.Behaviours;
using JG.Duplicates.Client.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JG.Duplicates.Client.Modules
{
    public class FileCompareViewModel : INotifyPropertyChanged, IFileCompareViewModel, IDroppable
    {
        private readonly IEventAggregator eventAggregator;


        private ICommand _loadCompareCommand;
        private bool _canLoadCompareExecute = true;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _firstLocation;
        public string FirstLocation
        {
            set
            {
                this._firstLocation = value;
                OnPropertyChanged("FirstLocation");
            }
            get
            {
                return this._firstLocation;
            }
        }

        private string _secondLocation;
        public string SecondLocation
        {
            set
            {
                this._secondLocation = value;
                OnPropertyChanged("SecondLocation");
            }
            get
            {
                return this._secondLocation;
            }
        }

        private string _processingStatus;
        public string ProcessingStatus
        {
            set
            {
                this._processingStatus = value;
                OnPropertyChanged("ProcessingStatus");
            }
            get
            {
                return this._processingStatus;
            }
        }

        private List<FileCompareInfo> _duplicateList;
        public List<FileCompareInfo> DuplicateList
        {
            set
            {
                this._duplicateList = value;
                OnPropertyChanged("DuplicateList");
            }
            get
            {
                return this._duplicateList;
            }
        }

        public ICommand LoadCompareClickCommand
        {
            get
            {
                return _loadCompareCommand ?? (_loadCompareCommand = new CommandHandler(() => LoadComparison(), _canLoadCompareExecute));
            }
        }

        public FileCompareViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            InitializeEventAggregatorSubscriptions();

            this.FirstLocation = @"\\justin-nas\AllDisk\Pictures\Wedding\Different";
            this.SecondLocation = @"\\justin-nas\AllDisk\Pictures\Wedding\Agreed";
        }

        private void InitializeEventAggregatorSubscriptions()
        {
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(
                this.SetComparisonLocation,
                true);
        }

        private void SetComparisonLocation(FileSelectionEventArgs args)
        {
            this.FirstLocation = args.FileInfo.DirectoryName.ToString(); //.FileInfo.Directory.FullName;
        }

        private void LoadComparison()
        {
            try
            {
                FileCompare fileCompare = new FileCompare();
                this.DuplicateList = fileCompare.GetDuplicateFiles(this.FirstLocation, this.SecondLocation);

                this.ProcessingStatus = string.Empty;                
            }
            catch (Exception ex)
            {
                this.ProcessingStatus = ex.Message;
            }
        }

        #region INotifyPropertyChanged Members

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // INotifyPropertyChanged Members

        public Type DataType
        {
            get { return typeof(DuplicateFileTree); }
        }

        public void Drop(object data, FrameworkElement element)
        {
            ((System.Windows.Controls.TextBox)element).Text = ((ITreeItem)data).DirectoryName.FullName;
        }
    }
}
