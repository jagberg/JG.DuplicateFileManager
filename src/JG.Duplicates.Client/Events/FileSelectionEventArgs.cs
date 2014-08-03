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
        public FileInfo FileInfo { get; set; }

        public FileSelectionEventArgs(FileInfo fileInfo)
        {
            this.FileInfo = fileInfo;
        }
    }
}
