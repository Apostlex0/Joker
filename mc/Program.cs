using System;
namespace Mc
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Writer("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    return;
                if (line =="1 + 2 * 3")
                    Console.WriterLine("7");
            }
        }
    }
}