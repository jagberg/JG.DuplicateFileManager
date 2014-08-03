using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles
{
    public class FileComparer : System.Collections.Generic.IEqualityComparer<System.IO.FileInfo>
    {
        public bool Equals(System.IO.FileInfo first, System.IO.FileInfo second)
        {
            bool isEqual = false;

            //isEqual = first.Name == second.Name;
            isEqual = first.Name == second.Name;
            isEqual = first.LastWriteTime == second.LastWriteTime;

            return isEqual;
        }

        public int GetHashCode(System.IO.FileInfo fi)
        {

            string s = String.Format("{0}{1}", fi.LastWriteTime, fi.Name);
            return s.GetHashCode();
        }
    }
}
