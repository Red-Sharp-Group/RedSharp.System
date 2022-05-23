using System;
using System.Linq;
using RedSharp.Sys.Utils;

namespace RedSharp.Sys.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            var set = FlexibleSets.CreateArrayIndexing(10);

            foreach (var item in set.Select(item => item.ToString() + ' '))
                Console.Write(item);

            Console.ReadKey();
        }
    }
}
