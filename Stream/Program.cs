using System;
using System.IO;

namespace ReadAndWriteProject
{
    class Program
    {
        static void Main(string[] args) 
        {
            WriteForFile();
            ReadForFile();
        }

        public static void WriteForFile()
        {
            StreamWriter streamWriter = new StreamWriter(@"D:\Github\JuniorMind\Stream\text.txt");
            streamWriter.WriteLine("Input Text");
            streamWriter.Close();
        }

        public static void ReadForFile()
        {
            StreamReader streamReader = new StreamReader(@"D:\Github\JuniorMind\Stream\text.txt");
            Console.WriteLine(streamReader.ReadToEnd());
            streamReader.Close();
        }
    }
}