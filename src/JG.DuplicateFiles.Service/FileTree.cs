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
        private List<FileInfo> treeList;

        public List<FileInfo> FileTreeList { get { return this.treeList; } }

        public FileTree(string rootLocation, string[] searchFileTypes)
        {
            treeList = new List<FileInfo>();
            this.LoadFileTree(rootLocation, searchFileTypes);
        }

        public void LoadFileTree(string rootLocation, string[] searchFileTypes)
        {
            DirectoryInfo dir1 = new DirectoryInfo(rootLocation);

            string[] extensions = searchFileTypes;

            IEnumerable<System.IO.FileInfo> files = dir1.EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories)
                                                            .Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));

            FileComparer fileCompare = new FileComparer();

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

            DirectoryComparer dirCompare = new DirectoryComparer();

            //var tr = trList.GroupBy(x => x.FileInfo.Directory, dirCompare)
            //    .Select(t => new FileTreeInfo()
            //    {
            //        DuplicateCount = 0,
            //        ChildFiles = t.ToList(),
            //        FileInfo = t.Select(z => z.FileInfo).FirstOrDefault(),
            //        ID = t.Select(z => z.FileInfo.DirectoryName).FirstOrDefault()
            //    }
            //    )
            //.OrderBy(x => x.FileInfo.FullName)
            //.ToList();

//            var trList = files.GroupBy(x => x, fileCompare)
//.Select(x => new FileTreeInfo()
//{
//    DuplicateCount = x.Count(),
//    DirectoryInfo = x.Select(z => z.Directory).Distinct().FirstOrDefault(),
//    Directories = x.Select(y => new FileTreeDirectoryItemInfo()
//    {
//        DuplicateCount = y.FullName.Count(),
//        ChildFiles = x.Select(z => new FileTreeItemInfo()
//        {
//            DuplicateCount = x.Count(),
//            FileInfo = z
//        }).ToList()
//    }).ToList()
//})
//.Where(x => x.DuplicateCount > 1);

//            DirectoryComparer dirCompare = new DirectoryComparer();

//            var trListA = trList.GroupBy(x => x.DirectoryInfo, dirCompare)
//                            .Select(x => new FileTreeInfo()
//                            {
//                                DuplicateCount = x.Count(),
//                                DirectoryInfo = x.Select(z => z.DirectoryInfo).Distinct().FirstOrDefault(),
//                                Directories = x.Select(y => new FileTreeDirectoryItemInfo()
//                                {
//                                    DuplicateCount = y.Directories.Count(),
//                                    ChildFiles = x.Select(z => new FileTreeItemInfo()
//                                    {
//                                        DuplicateCount = x.Count()
//                                    }).ToList()
//                                }).ToList()
//                            });

            this.treeList = trListA.ToList();
        }
    }
}
