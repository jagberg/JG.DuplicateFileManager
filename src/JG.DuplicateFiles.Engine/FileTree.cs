using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles.Engine
{
    public class FileTree
    {
        private List<FileInfo> treeList;

        public List<FileInfo> FileTreeList { get { return this.treeList; } }

        public FileTree(string rootLocation, string[] searchFileTypes)
        {
            treeList = new List<FileInfo>();
            this.LoadFileTree(rootLocation, searchFileTypes);
        }

        /// <summary>
        /// Load files from root location.
        /// </summary>
        /// <param name="rootLocation">Root location to begin recursive search of files.</param>
        /// <param name="extensions">The file extensions that will be retrieved in the search</param>
        private void LoadFileTree(string rootLocation, string[] extensions)
        {
            DirectoryInfo dir1 = new DirectoryInfo(rootLocation);

            IEnumerable<System.IO.FileInfo> files = dir1.EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories)
                                                            .Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));

            CompareFiles(files);
        }

        private void CompareFiles(IEnumerable<System.IO.FileInfo> files)
        {
            FileInfoComparer fileCompare = new FileInfoComparer();

            var trList = files.GroupBy(x => x, fileCompare)
            .Select(x => new FileTreeInfo()
            {
                DuplicateCount = x.Count(),
                ChildFiles = x.Select(y => y).ToList(),
                FileInfo = x.Key,
            })
            .Where(x => x.DuplicateCount > 1);

            // Flatten the list so the view can diplay it however it wants.
            var trListA = trList
                        .SelectMany(x => x.ChildFiles)
                        .Select(y => y);

            DirectoryInfoComparer dirCompare = new DirectoryInfoComparer();

            this.treeList = trListA.ToList();
        }
    }
}
