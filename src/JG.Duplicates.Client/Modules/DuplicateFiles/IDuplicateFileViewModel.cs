using Microsoft.Practices.Prism.Commands;
using System;
namespace JG.Duplicates.Client.Modules
{
    public interface IDuplicateFileViewModel
    {
        System.Windows.Input.ICommand LoadRootComparisonClickCommand { get; }
        System.Collections.Generic.List<JG.DuplicateFiles.FileTreeInfo> MyFileTree { get; set; }
        event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        string RootLocation { get; set; }

        DelegateCommand SelectedFolder { get; set; }
    }
}
