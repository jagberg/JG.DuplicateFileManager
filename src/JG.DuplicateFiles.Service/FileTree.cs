using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles
{
    public class FileTree
    {
        private List<FileTreeInfo> treeList;

        public List<FileTreeInfo> FileTreeList { get { return this.treeList; } }

        public FileTree(string rootLocation)
        {
            treeList = new List<FileTreeInfo>();
            this.LoadFileTree(rootLocation);
        }

        public void LoadFileTree(string rootLocation)
        {
            DirectoryInfo dir1 = new DirectoryInfo(rootLocation);

            string[] extensions = new[] { ".jpg", ".bmp", ".jpeg" };

            IEnumerable<System.IO.FileInfo> files = dir1.EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories)
                                                            .Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));

            FileComparer fileCompare = new FileComparer();

            var trList = files.GroupBy(x => x, fileCompare)
            .Select(x => new FileTreeInfo()
            {
                DuplicateCount = x.Count(),
                ChildFiles = x.Select(y => new FileTreeInfo() { ID = y.FullName }).ToList(),
                FileInfo = x.Key,
                ID = x.First().Name
            })
            .Where(x => x.DuplicateCount > 1);

            DirectoryComparer dirCompare = new DirectoryComparer();

            var tr = trList.GroupBy(x => x.FileInfo.Directory, dirCompare)
                .Select(t => new FileTreeInfo()
                {
                    DuplicateCount = 0,
                    ChildFiles = t.ToList(),
                    FileInfo = t.Select(z => z.FileInfo).FirstOrDefault(),
                    ID = t.Select(z => z.FileInfo.DirectoryName).FirstOrDefault()
                }
                )
            .OrderBy(x => x.FileInfo.FullName)
            .ToList();

            this.treeList = tr.ToList();
        }
    }
}
