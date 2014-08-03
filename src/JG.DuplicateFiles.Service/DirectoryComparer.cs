using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.DuplicateFiles
{
    public class DirectoryComparer : System.Collections.Generic.IEqualityComparer<System.IO.DirectoryInfo>
    {
        public bool Equals(System.IO.DirectoryInfo first, System.IO.DirectoryInfo second)
        {
            bool isEqual = false;

            //isEqual = first.Name == second.Name;
            isEqual = first.Name == second.Name;

            return isEqual;
        }

        public int GetHashCode(System.IO.DirectoryInfo fi)
        {

            string s = String.Format("{0}", fi.Name);
            return s.GetHashCode();
        }
    }
}
