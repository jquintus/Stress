using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stress
{
    public class Program
    {
        public static bool Run = true;

        public static IEnumerable<Task> SpinUp()
        {
            for (int i = 0; i < System.Environment.ProcessorCount; i++)
            {
                yield return Task.Factory.StartNew(() =>
                {
                    while (Run) { };
                });
            }
        }

        public static void StressCpu()
        {
            var tasks = SpinUp().ToArray();

            Console.WriteLine("Stressing the CPU.  Press enter to stop.");
            Console.ReadLine();

            Run = false;

            Task.WaitAll(tasks);
        }

        private static void Main(string[] args)
        {
            StressCpu();
            //StressMem();
        }

        private static void StressMem()
        {
            LinkedList<byte> bytes = new LinkedList<byte>();

            while (true)
            {
                Console.ReadLine();
                for (int i = 0; i < 1024 * 1024; i++)
                {
                    bytes.AddLast(new byte());
                }
            }
        }
    }
}