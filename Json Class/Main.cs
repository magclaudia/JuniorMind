using System;

namespace JsonClasses
{
    public class MainClass
    {
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            string jsonValidFormat = File.ReadAllText(args[1]);
            var value = new Value();
            if (value.Match(jsonValidFormat).Succes() && value.Match(jsonValidFormat).RemainingText() == "")
            {
                Console.WriteLine("File is a Json format");
            }
            else
            {
                Console.WriteLine("File is not a Json format");
            }
        }
    }
}
