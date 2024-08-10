using System.Collections.Generic;
using System.IO;

namespace DirCompare
{
    internal class FileCompare : IEqualityComparer<FileInfo>
    {
        public FileCompare() { }

        public bool Equals(FileInfo f1, FileInfo f2)
        {
            return f1.Name == f2.Name
                && f1.Length == f2.Length;
        }

        // Return a hash that reflects the comparison criteria. According to the   
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must  
        // also be equal. Because equality as defined here is a simple value equality, not  
        // reference identity, it is possible that two or more objects will produce the same  
        // hash code.  
        public int GetHashCode(FileInfo fi)
        {
            string s = string.Format("{0}{1}", fi.Name, fi.Length);
            return s.GetHashCode();
        }

    }
}
