using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client.Events
{
    public sealed class FileSelectionEventArgs : EventArgs
    {
        public ITreeItem FileInfo { get; set; }

        public FileSelectionEventArgs(ITreeItem fileInfo)
        {
            this.FileInfo = fileInfo;
        }
    }
}
