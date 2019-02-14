using System;
using System.Collections.Generic;
using System.Linq;

namespace Example2Linq
{
    class Program
    {
        static void Main()
        {
            // 2. Functional nature of LINQ
            IEnumerable<string> a = Enumerable.Range(1, 100)
                .Where(x => x % 20 == 0) // filter with a predicate (a function which returns a bool) here using a lambda expression so only get 20,40..
                //.OrderBy(x => -x) // sort by descending into a new sequence
                .OrderByDescending(x => x) // sort by descending into a new sequence
                .Select(x => $"{x}%"); // map each numerical value to a string suffixed by a % into a new sequence

            // convert to method group
            a.ToList().ForEach(x => Console.WriteLine(x)); // 100%, 80%, 60%, 40%, 20%
        }
    }
}
