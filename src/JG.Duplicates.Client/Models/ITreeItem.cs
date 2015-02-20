using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client
{
    public interface ITreeItem
    {
        DirectoryInfo DirectoryName { get; set; }
    }
}
