using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            var program = new FolderComparer();
            program.CompareAsync();

            Console.ReadLine();
        }
    }

    internal class FolderComparer
    {
        public async void CompareAsync()
        {
            Console.WriteLine("Введите первую папку:");
            string path1 = Console.ReadLine();
            Console.WriteLine("Введите вторую папку:");
            string path2 = Console.ReadLine();

            if (Directory.Exists(path1) && Directory.Exists(path2))
            {
                IEnumerable<FileInfo> list1 = await GetInitialListAsync(path1);
                IEnumerable<FileInfo> list2 = await GetInitialListAsync(path2);

                FileCompare myFileCompare = new FileCompare();

                var queryList1Only = await GetResultListAsync(list1, list2, myFileCompare);
                var queryList2Only = await GetResultListAsync(list2, list1, myFileCompare);

                if (queryList1Only.Count() != 0 || queryList2Only.Count() != 0)
                {
                    WriteInfo(queryList1Only, "Только в " + path1 + ":");
                    WriteInfo(queryList2Only, "Только в " + path2 + ":");
                }
                else
                {
                    Console.WriteLine("Различий нет \r\n");
                }
            }
        }

        private async Task<IEnumerable<FileInfo>> GetInitialListAsync(string path)
        {
            return await Task.Run(() => GetInitialList(path));
        }

        private async Task<IEnumerable<string>> GetResultListAsync(IEnumerable<FileInfo> list1, IEnumerable<FileInfo> list2, FileCompare myFileCompare)
        {
            return await Task.Run(() => GetResultList(list1, list2, myFileCompare));
        }

        private IEnumerable<FileInfo> GetInitialList(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            return dir.GetFiles("*.*", SearchOption.AllDirectories);
        }

        private IEnumerable<string> GetResultList(IEnumerable<FileInfo> list1, IEnumerable<FileInfo> list2, FileCompare myFileCompare)
        {
            return list1.Except(list2, myFileCompare).Select(x => x.FullName);
        }

        private void WriteInfo(IEnumerable<string> files, string comment)
        {
            Console.WriteLine(comment + "\r\n");
            Console.WriteLine("Файлы:" + "\r\n");
            foreach (string s in files)
            {
                Console.WriteLine(s + "\r\n");
            }
        }
    }

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
