using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(@"\\justin-nas\AllDisk\Pictures\Wedding");

            string[] extensions = new[] { ".jpg", ".bmp", ".jpeg" };

            IEnumerable<System.IO.FileInfo> files = dir1.EnumerateFiles("*.*", System.IO.SearchOption.AllDirectories)
                                                            .Where(f => extensions.Contains(f.Extension, StringComparer.OrdinalIgnoreCase));

            FileComparer fileCompare = new FileComparer();

            var q = files.GroupBy(x => x, fileCompare)
            .Select(x => new
            {
                Count = x.Count(),
                Files = x.Select(y => new {Loc = y.FullName}),
                Name = x.Key,
                ID = x.First().Name
            })
            .Where(x => x.Count > 1)
            .OrderByDescending(x => x.Count);

            q.ToList();
        }
    }
}
