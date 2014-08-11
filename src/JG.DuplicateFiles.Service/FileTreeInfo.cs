using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles
{
    public class FileTreeInfo
    {
        public FileTreeInfo()
        {
        }

        public int DuplicateCount { get; set; }

        public FileInfo FileInfo { get; set; }

        public List<FileInfo> ChildFiles { get; set; }
        //public List<FileTreeDirectoryItemInfo> Directories { get; set; }

    }

    public class FileTreeDirectoryItemInfo
    {
        public int DuplicateCount { get; set; }

        public List<FileTreeItemInfo> ChildFiles { get; set; }
    }

    public class FileTreeItemInfo
    {
        public int DuplicateCount { get; set; }

        public FileInfo FileInfo { get; set; }
    }
}
