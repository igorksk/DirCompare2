using System;
using System.Text;

namespace DirCompare
{
    class Program
    {
        static void Main(string[] args)
        {
            // Cyrillic support
            Console.OutputEncoding = Encoding.GetEncoding("Cyrillic");
            Console.InputEncoding = Encoding.GetEncoding("Cyrillic");

            var program = new FolderComparer();
            program.CompareAsync();

            Console.ReadLine();
        }
    }  
}
