using System;
using System.Collections.Generic;
using System.IO;

namespace DirCompare
{
    // Optimized replacement for FileCompare.cs
    public class FileCompare : IEqualityComparer<FileInfo>
    {
        private static readonly StringComparer NameComparer = StringComparer.Ordinal;

        public FileCompare() { }

        public bool Equals(FileInfo f1, FileInfo f2)
        {
            if (ReferenceEquals(f1, f2)) return true;
            if (f1 is null || f2 is null) return false;

            return NameComparer.Equals(f1.Name, f2.Name)
                && f1.Length == f2.Length;
        }

        // Return a hash that reflects the comparison criteria. Avoid string allocation.
        public int GetHashCode(FileInfo fi)
        {
            if (fi is null) return 0;

            unchecked
            {
                int hashName = fi.Name != null ? NameComparer.GetHashCode(fi.Name) : 0;
                int hashLen = fi.Length.GetHashCode();
                return (hashName * 397) ^ hashLen;
            }
        }
    }
}
