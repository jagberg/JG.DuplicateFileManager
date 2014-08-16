using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client
{
    public class DuplicateFileTree
    {
        public string DirectoryName { get; set; }

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
