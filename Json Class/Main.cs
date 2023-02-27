using System;

namespace JsonClasses
{
    public class MainClass
    {
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                Console.WriteLine("Make sure you entered a valid location for json file.");
                return;
            }

            string jsonFormat = File.ReadAllText(args[1]);
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
