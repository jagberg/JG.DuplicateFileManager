using JG.DuplicateFiles;
using JG.Duplicates.Client.Events;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JG.Duplicates.Client.Modules
{
    public class FileCompareViewModel : INotifyPropertyChanged, JG.Duplicates.Client.Modules.IFileCompareViewModel
    {
        private readonly IEventAggregator eventAggregator;

        #region INotifyPropertyChanged Members

        private ICommand _loadCompareCommand;
        private bool _canLoadCompareExecute;

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
        }

        private void InitializeEventAggregatorSubscriptions()
        {
            this.eventAggregator.GetEvent<FileSelectionEvent>().Subscribe(
                this.SetComparisonLocation,
                true);
        }

        private void SetComparisonLocation(FileSelectionEventArgs args)
        {
            this.FirstLocation = args.FileInfo.Directory.FullName;
        }

        private void LoadComparison()
        {
            FileCompare fileCompare = new FileCompare();
            fileCompare.GetDuplicateFiles(this.FirstLocation, this.SecondLocation);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion // INotifyPropertyChanged Members
    }
}
