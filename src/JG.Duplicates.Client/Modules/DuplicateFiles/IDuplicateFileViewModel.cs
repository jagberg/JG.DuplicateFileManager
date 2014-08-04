﻿using System;
namespace JG.Duplicates.Client.Modules
{
    public interface IDuplicateFileViewModel
    {
        global::System.Threading.Tasks.Task LoadRootComparisonAsync();
        global::Microsoft.Practices.Prism.Commands.DelegateCommand LoadRootComparisonClickCommand { get; }
        global::System.Collections.Generic.List<global::JG.DuplicateFiles.FileTreeInfo> MyFileTree { get; set; }
        event global::System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        string RootLocation { get; set; }
        global::JG.DuplicateFiles.FileTreeInfo SelectedFileItem { get; set; }
        global::Microsoft.Practices.Prism.Commands.DelegateCommand SelectedFolder { get; set; }
    }
}
