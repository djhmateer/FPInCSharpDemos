using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FPAbstractions
{
    // Static so can use extension methods
    static class Program
    {
        static void Main()
        {
            One(); // Pure functions
            Two(); // Immutability, Smart Constructor
            Three(); // Option type with None and Some
            Four(); // Option with Match to primitive
            Five(); // Option and Map (Functor)
            Six(); // IEnumerable and Select (Functor)
            Seven(); // Chaining Map
            Eight(); // Bind
            Nine(); // Bind chain 
            Ten(); // IEnumerable and Extension methods chaining
            Eleven(); // Either - Exception
            Twelve(); // Either - Validation pipeline
        }

        // Pure functions (Core part of FP!)
        static void One()
        {
            // A function is pure if the same input always gives the same output
            // can't have any IO...eg API call, 
            var result = Double(2);

            // This is impure - as the website may be down, so a unit test would return a different result 
            var resultb = GetHtml("test.com");
        }

        // This is a pure function
        // Very easy to unit test
        static int Double(int i) => i * 2;





        // Immutability (Core part of FP!), Smart ctor
        static void Two()
        {
            PersonOO dave = new PersonOO();
            dave.Name = "dave";
            dave.Age = 45;
            // Don't do this in FP.. F# (by default), Haskell etc. does not allow this
            // Should avoid mutating objects in place and favour making new ones
            // (as can be sure no side effects anywhere else)
            dave.Age = 46;

            Person bob = new Person(name: "bob", age: 22);
            //bob.Age = 23; // compiler doesn't allow this
            // Creating a new version of bob with an updated age
            var bob2 = bob.With(age: 23);
            // A new version of bob with updated age 
            var bob3 = Birthday(bob);
        }

        // Function is pure - no side effects ie not updating anything which could be used anywhere else
        // we are returning a brand new object
        static Person Birthday(Person p) => p.With(age: p.Age + 1);

        class PersonOO
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        class Person
        {
            public string Name { get; } // C#6 Getter only auto-property. 
            public int Age { get; }

            // Smart constructor enforcing a name and age are given
            public Person(string name, int age)
            {
                Name = name;
                Age = age;
            }

            // Updates here, enforcing a new object is returned every time
            // strings are nullable because they are reference types
            public Person With(string name = null, int? age = null)
            {
                // if null, return the current value
                // else set the newly passed in value
                // ?? null coalescing operator
                return new Person(name: name ?? Name, age: age ?? Age);
            }
        }




        // Option type with None and Some
        static void Three()
        {
            Option<string> html = GetHtmlThree("badurl"); // None 
            Option<string> htmlb = GetHtmlThree("test.com"); // html here 
        }

        static Option<string> GetHtmlThree(string url) =>
            // None represents nothing. Part of language-ext
            // Some represents a container for the string
            url == "test.com" ? Some("html here") : None;





        // Option type with Match down to primitive type
        static void Four()
        {
            // 2 different results of the GetHtml function
            Option<string> result = None;
            Option<string> resultb = Some("html here");

            // Forces us to deal with the None case
            // Null reference exceptions can't happen as it wont compile unless we handle it
            // Match down to primitive type (as opposed to elevated types of Option<T>, IEnumerable<T>, Either<T>)
            string stringResult = result.
                Match(Some: x => x, None: () => "none returned"); // none returned
            string stringResultb = resultb.
                Match(Some: x => x, None: () => "none returned"); // html here
        }



        // Match and Map
        static void Five()
        {
            string value = GetValue(true)
                           .Match(Some: name => $"Hello, {name}", None: () => "Goodbye");
            Console.WriteLine(value); // Hello, Bob

            // Map - Functor
            // Lambda inside Map wont be invoked if Option is in None state
            // Option is a replacement for if statements ie if obj == null
            // Working in elevated context to do logic
            // Similar to LINQ on IEnumerable.. ie if it is an empty list, then nothing will happen

            Option<string> resultb = GetValue(false).Map(name => $"Hello, {name}"); // None
        }

        // Ternary nice and clean
        static Option<string> GetValue(bool hasValue) =>
            hasValue ? Some("Bob") : None;


        // IEnumerable Map and Select
        static void Six()
        {
            Option<string> name = Some("Bob");

            // Map works on Option applying the function..just like select on IEnumerable.. they are both functors
            Option<string> result = name.Map(x => x.ToUpper()); // Some(BOB)


            IEnumerable<string> names = new[] { "Bob", "Alice" };

            IEnumerable<string> resultb = names.Map(x => x.ToUpper()); // a normal IEnumerable
            IEnumerable<string> resultc = names.Select(x => x.ToUpper()); // a normal IEnumerable
        }


        // Using Map (to avoid having to check for null at each stage)
        static void Seven()
        {
            //Option<string> html = GetHtml("a");
            Option<string> html = GetHtml("c");
            // shorten html
            Option<string> shortHtml = html.Map(x => x.Substring(0, 1)); // Some(a)
                                                                         // put on https
            Option<string> addedHttps = shortHtml.Map(x => $"https://{x}"); // Some(https://a)

            // come down from elevated context and deal with None
            string final = addedHttps.Match(Some: x => x, None: () => "No html returned");
        }

        static Option<string> GetHtml(string url)
        {
            if (url == "a") return "aa";
            if (url == "b") return "bb";
            return None;
        }


        // Bind - Monad - Bind two Option<strings>
        // SelectMany or FlatMap
        static void Eight()
        {
            // If either GetFirstName() or MakeFullName(string firstName) returned None
            // then name would be None
            // return is not an Option<Option<string>>.. but flattened
            Option<string> name = GetFirstName()
                                 .Bind(MakeFullName);
            Console.WriteLine(name); // Some(Joe Bloggs)
        }

        static Option<string> GetFirstName() =>
            Some("Joe");

        static Option<string> MakeFullName(string firstName) =>
            Some($"{firstName} Bloggs");



        // Using Bind so we can chain multiple Option<T> functions together
        static void Nine()
        {
            Option<string> html = GetHtml("a");
            //Option<string> html = GetHtml("c");

            // we don't want to have Option<Option<string>>
            Option<string> shortHtml = html.Bind(x => ShortenHtml(x)); // x is a string

            // put on https using shortened Method syntax so don't need x
            Option<string> addedHttps = shortHtml.Bind(PutOnHttps);

            Option<string> both = html.Bind(ShortenHtml)
                                      .Bind(PutOnHttps);

            // Come down from elevated context and deal with None
            string final = addedHttps.Match(Some: x => x, None: () => "No html returned");
            string finalb = both.Match(Some: x => x, None: () => "No html returned");
        }

        static Option<string> ShortenHtml(string html) =>
            html == "" ? None :
            Some(html.Substring(0, 10));

        static Option<string> PutOnHttps(string html) =>
            html.Length < 3 ? None : // business rule to catch invalid html
            Some("https://" + html);


        // IEnumerable and Extension methods
        static void Ten()
        {
            IEnumerable<string> listHrefs = GetListHrefs();
            var baseUrl = "https://davemateer.com";
            // LINQ extension method style
            // if UrlType is Invalid (ie not Absolute or Relative) then don't add to this list
            IEnumerable<string> finalUrl = listHrefs
                                          // extension method which suffixes Relative, Absolute or Invalid
                                          .GetUrlTypes()
                                          // depending on UrlType above, process the url
                                          .ProcessUrls(baseUrl);
        }

        static IEnumerable<string> ProcessUrls(this IEnumerable<string> urls, string baseUrl) =>
            // ProcessUrl could return None so we use Bind instead of Select
            urls.Bind(x => ProcessUrl(x, baseUrl));

        static Option<string> ProcessUrl(string url, string baseUrl) =>
            url.EndsWith("Absolute") ? url :
            url.EndsWith("Relative") ? Some(baseUrl + url) :
            None;

        static IEnumerable<string> GetUrlTypes(this IEnumerable<string> html) =>
            // Apply GetUrlType to each element in the list
            html.Select(GetUrlType);

        // Using expressions instead of if..return
        static string GetUrlType(string link) =>
            link.StartsWith("https://") ? $"{link} : Absolute" :
            link.StartsWith("/") ? $"{link} : Relative" :
            "{link} : Unknown";

        static IEnumerable<string> GetListHrefs()
        {
            yield return "/programming-in-c-sharp";
            yield return "https://bbc.co.uk";
            yield return "mailto:davemateer@gmail.com";
        }


        // Either - Exception handling
        static void Eleven()
        {
            Option<string> result = GetHtml("invalidurl"); // None. ie it will swallow the exception

            Either<Exception, string> resultb = GetHtmlE("invalidurl");  // Left(System.InvalidOperationException)

            // Did the request throw an exception or return html?
            resultb.Match(
                Left: ex => HandleException(ex),
                Right: ProcessPipeline
            );

            // Keep in the elevated context 
            var resultc = resultb.Map(x => x.Substring(0, 1)); // if not an Exception ie Right, then apply a lambda expression

            // Bind an Either<Exception, string> with an Either<Exception, string>
            var resultd = resultb.Bind(DoSomething);
        }

        static Either<Exception, string> DoSomething(string thing) => thing.Substring(0, 1);

        static void HandleException(Exception ex) => Console.WriteLine(ex);

        static void ProcessPipeline(string html) => Console.WriteLine(html);

        public static Either<Exception, string> GetHtmlE(string url)
        {
            var httpClient = new HttpClient(new HttpClientHandler());
            try
            {
                // async and tasks later
                var httpResponseMessage = httpClient.GetAsync(url).Result;
                return httpResponseMessage.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex) { return ex; }
        }


        // Either - Errors in Validation handling
        static void Twelve()
        {
            var url = "https://davemateer.com";
            var result = RunUrlValidationPipeline(url);

            var message = result.Match(Left: r => $"Rejected because: {r.ReasonForRejection}",
                Right: x => "Success"); // Rejected because: Is not in allowed suffixes
        }

        // Railway oriented validation
        // a url which needs to go through a validation pipeline
        // if it fails at any point it goes on the left track 
        // and wont go any further on right
        public static Either<URLRejection, string> RunUrlValidationPipeline(string url) =>
            DoesUrlStartWithHttp(url)
            .Bind(DoesUrlStartWithHttps)
            .Bind(IsUrlInAllowedSuffixes);

        public static Either<URLRejection, string> DoesUrlStartWithHttp(string url) => url; // pass

        public static Either<URLRejection, string> DoesUrlStartWithHttps(string url) => url; // pass

        public static Either<URLRejection, string> IsUrlInAllowedSuffixes(string url)
        {
            return new URLRejection { ReasonForRejection = "Is not in allowed suffixes" };
            //return url; // hack for yes it does
        }

        public class URLRejection
        {
            public string ReasonForRejection { get; set; }
        }
    }
}

// C# has a syntax for monadic types: LINQ
// Is Bind the same as method chaining in LINQ when using IEnumerable<T>
// Bind is for working with Option<T> and Either<L,R> but supports IEnumerable<T>
// https://github.com/louthy/language-ext/issues/456
// LINQ SelectMany / FlatMap
//static void Six()
//{
//    var name = from first in GetFirstName()
//               from last in GetLastName()
//               select $"{first} {last}";

//    // flatmap
//    var nameb = GetFirstName().SelectMany(
//        first => GetLastName(), (first, last)
//            => $"{first} {last}"
//        );
//    Console.WriteLine(name); // Some(Joe BloggsB)
//}

//static Option<string> GetLastName() =>
//    Some("BloggsB");