using System;
using System.Text.Json;
namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(JsonSerializer.Serialize(new Test() { A = 100 }));
        }

        class Test
        {

            public int A { get; set; }
            public string _B { get; set; }
        }
    }
}
