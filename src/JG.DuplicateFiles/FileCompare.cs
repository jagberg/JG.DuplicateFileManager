using JG.DuplicateFiles.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles
{
    public class FileCompare
    {
        public List<FileCompareInfo> FileList { get; set; }

        public FileCompare()
        {
        }

        public List<FileCompareInfo> GetDuplicateFiles(string sourceLocation, string comparisonLocation)
        {
            DirectoryInfo sourceDirectory = new DirectoryInfo(sourceLocation);
            DirectoryInfo comparisonDirectory = new DirectoryInfo(comparisonLocation);

            string[] extensions = new[] { ".jpg", ".bmp", ".jpeg" };

            IEnumerable<System.IO.FileInfo> sourceFiles = sourceDirectory.EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                                                            .Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));

            IEnumerable<System.IO.FileInfo> comparisonLocationFiles = comparisonDirectory.EnumerateFiles("*.*", System.IO.SearchOption.TopDirectoryOnly)
                                                            .Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));


            FileComparer fileCompare = new FileComparer();

            var distinctFiles = sourceFiles.Except(comparisonLocationFiles, fileCompare)
                .Select((t, v) => new FileCompareInfo()
                {
                    SourceFileInfo = t,
                    DuplicateFileInfo = t
                }
                );

            return distinctFiles.ToList();
        }
    }
}
