using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DirCompare
{
    internal class FolderComparer
    {
        public async void CompareAsync()
        {
            Console.WriteLine("Enter first folder:");
            string path1 = Console.ReadLine();
            Console.WriteLine("Enter second folder:");
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
                    WriteInfo(queryList1Only, "Only folder " + path1 + ": \r\n");
                    WriteInfo(queryList2Only, "Only folder " + path2 + ": \r\n");
                }
                else
                {
                    Console.WriteLine("Folders are equal \r\n");
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
            Console.WriteLine("Files:" + "\r\n");
            foreach (string s in files)
            {
                Console.WriteLine(s + "\r\n");
            }
        }
    }
}
