using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client
{
    public class DuplicateFileTree : ITreeItem
    {
        public DirectoryInfo DirectoryName { get; set; }

        public List<DuplicateDirectoryList> DirList { get; set; }

        public int TotalDuplicates
        {
            get
            {
                return this.DirList.Sum(t => t.Children.Count);
            }
        }
    }
}
