using System;

namespace JsonClasses
{
    public class MainClass
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Make sure you enter an argument.");
                return;
            }

            if (args.Length > 0)
            {
                string jsonFormat = System.IO.File.ReadAllText(args[0]);
                var value = new Value();
                var match = value.Match(jsonFormat);
                if (match.Succes() && match.RemainingText() == "")
                {
                    Console.WriteLine("File is a Json valid format");
                }
                else
                {
                    Console.WriteLine("File is not a Json valid format");
                }
            }
        }
    }
}
