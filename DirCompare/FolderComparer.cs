using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DirCompare
{
    internal class FolderComparer
    {
        public async Task CompareAsync()
        {
            Console.WriteLine("Enter first folder:");
            string path1 = Console.ReadLine();
            Console.WriteLine("Enter second folder:");
            string path2 = Console.ReadLine();

            if (Directory.Exists(path1) && Directory.Exists(path2))
            {
                List<FileInfo> list1 = await GetInitialListAsync(path1);
                List<FileInfo> list2 = await GetInitialListAsync(path2);

                FileCompare myFileCompare = new FileCompare();

                List<string> only1 = await GetResultListAsync(list1, list2, myFileCompare);
                List<string> only2 = await GetResultListAsync(list2, list1, myFileCompare);

                if (only1.Count != 0 || only2.Count != 0)
                {
                    WriteInfo(only1, "Only folder " + path1 + ": \r\n");
                    WriteInfo(only2, "Only folder " + path2 + ": \r\n");
                }
                else
                {
                    Console.WriteLine("Folders are equal \r\n");
                }
            }
        }

        private async Task<List<FileInfo>> GetInitialListAsync(string path)
        {
            return await Task.Run(() => GetInitialList(path).ToList());
        }

        private async Task<List<string>> GetResultListAsync(IEnumerable<FileInfo> list1, IEnumerable<FileInfo> list2, FileCompare myFileCompare)
        {
            return await Task.Run(() => GetResultList(list1, list2, myFileCompare).ToList());
        }

        private IEnumerable<FileInfo> GetInitialList(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            return dir.GetFiles("*.*", SearchOption.AllDirectories);
        }

        private IEnumerable<string> GetResultList(IEnumerable<FileInfo> list1, IEnumerable<FileInfo> list2, FileCompare myFileCompare)
        {
            // Build a hash set from list2 to allow O(1) lookups using the provided comparer
            var set2 = new HashSet<FileInfo>(list2, myFileCompare);

            foreach (var f in list1)
            {
                if (!set2.Contains(f))
                    yield return f.FullName;
            }
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
