using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles.Domain
{
    public class FileCompareInfo
    {
        public FileInfo SourceFileInfo { get; set; }

        public FileInfo DuplicateFileInfo { get; set; }

    }
}
