using System;
using System.Diagnostics;

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

                var time = new Stopwatch();
                time.Start();
                var path = "D:\\Github\\JuniorMind\\Json Class\\jsonFormat.txt";
                string jsonFormat = System.IO.File.ReadAllText(path);
                var value = new Value();
                var text = new StringSpan(jsonFormat);
                var actualResult = value.Match(text);
                var expectedResult = new StringSpan(jsonFormat, jsonFormat.Length);
                if (expectedResult.CheckIfEqualTo(actualResult.RemainingText()))
                {
                    Console.WriteLine("File is a Json valid format: YES");
                }
                else
                {
                    var (line, column) = Match.GetLineAndColumnFromPosition(jsonFormat);
                    Console.WriteLine("File is not a valid Json format: NO. Failed at line and column ({0}, {1}).", line, column);
                }

                time.Stop();
                Console.WriteLine("Time needed to verify {0} milliseconds", time.ElapsedMilliseconds);
            }
		}
    }
}
