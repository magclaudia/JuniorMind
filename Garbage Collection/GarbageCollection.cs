using System;

namespace GarbageCollection
{
    public class GarbageCollector
    {
        static int objSize = 10;
        static void Main()
        {
            Console.WriteLine("Write \"start\" for beginning checking perfomance between generations 0, 1 or 2 of Garbage Colletion.");
            var startWord = Console.ReadLine();
            if (startWord == "start")
            {
                MeasurePerformanceForGenerationOfGarbageCollection(objSize);
            }
            else 
            {
                Console.WriteLine("Please write the right word to start.");
            }
        }

        static void MeasurePerformanceForGenerationOfGarbageCollection(int objSize)
        {
            var startTime = DateTime.Now;
            var objects = new object[objSize];
            var countTimeUseToCleanMem = (DateTime.Now - startTime).TotalMilliseconds;
            var generationType = GC.GetGeneration(objects);
            Console.WriteLine("Current object generation type is {0}", generationType);
            Console.WriteLine("Time use to clean memory used by objects of generation {0} is {1} milliseconds.", generationType, countTimeUseToCleanMem);
            startTime = DateTime.Now;
            for (int i = 0; i < objSize; i++)
            {
                var obj = objects[i];
            }
            
            var countTimeForAccesingObj = (DateTime.Now - startTime).TotalMilliseconds;
            Console.WriteLine("Time use for accesing elements of generation type {0} is {1} milliseconds.", generationType, countTimeForAccesingObj);
            Console.WriteLine();

            for (int i = 0;i < objSize; i++)
            {
                objects[i] = new object();
            }

            GC.Collect();
            countTimeUseToCleanMem = (DateTime.Now - startTime).TotalMilliseconds;
            generationType = GC.GetGeneration(objects);
            Console.WriteLine("Current object generation type is {0}", generationType);
            Console.WriteLine("Time use to clean memory used by objects of generation type {0} is {1} milliseconds.", generationType, countTimeUseToCleanMem);
            startTime = DateTime.Now;
            for (int i = 0; i < objSize; i++)
            {
                var obj = objects[i];
            }

            countTimeForAccesingObj = (DateTime.Now - startTime).TotalMilliseconds;
            Console.WriteLine("Time use for accesing elements of generation type {0} is {1} milliseconds.", generationType, countTimeForAccesingObj);
            Console.WriteLine();

            startTime = DateTime.Now;
            for (int i = 0; i < objSize; i++)
            {
                objects[i] = new object();
            }

            GC.Collect();
            countTimeUseToCleanMem = (DateTime.Now - startTime).TotalMilliseconds;
            generationType = GC.GetGeneration(objects);
            Console.WriteLine("Current object generation type is {0}", generationType);
            Console.WriteLine("Time use to clean memory used by objects of generation type {0} is {1} milliseconds.", generationType, countTimeUseToCleanMem);
            startTime = DateTime.Now;
            for (int i = 0; i < objSize; i++)
            {
                var obj = objects[i];
            }

            countTimeForAccesingObj = (DateTime.Now - startTime).TotalMilliseconds;
            Console.WriteLine("Time use for accesing elements of generation type {0} is {1} milliseconds.", generationType, countTimeForAccesingObj);
        }
    }
}