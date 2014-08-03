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

        public string ID { get; set; }

        public FileInfo FileInfo { get; set; }

        public List<FileTreeInfo> ChildFiles { get; set; }
    }
}
