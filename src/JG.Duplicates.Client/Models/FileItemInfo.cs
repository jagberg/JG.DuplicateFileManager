using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client
{
    public class FileItemInfo : ITreeItem
    {
        public DirectoryInfo DirectoryName { get; set; }

        public FileInfo FileInfo { get; set; }
    }
}
