using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stress
{
    public class Program
    {
        private static bool _run = true;

        public static void Main(string[] args)
        {
            object invokedVerbInstance;

            var options = new StressOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options,
              (verb, subOptions) =>
              {
                  invokedVerbInstance = subOptions;

                  switch (verb.ToLower())
                  {
                      case "cpu":

                          StressCpu(options.Cpu.DryRun);
                          break;

                      case "ram":
                          StressMem(options.Ram.DryRun);
                          break;
                  }
              }))
            {
                
            }
        }

        private static IEnumerable<Task> SpinUp()
        {
            for (int i = 0; i < System.Environment.ProcessorCount; i++)
            {
                yield return Task.Factory.StartNew(() =>
                {
                    while (_run) { };
                });
            }
        }

        private static void StressCpu(bool dryRun)
        {
            Console.WriteLine("Stressing the CPU.  Press enter to stop.");
            if (dryRun) return;

            var tasks = SpinUp().ToArray();

            Console.ReadLine();

            _run = false;

            Task.WaitAll(tasks);
        }

        private static void StressMem(bool dryRun)
        {
            Console.WriteLine("Stressing out the memory");
            if (dryRun) return;

            LinkedList<byte> bytes = new LinkedList<byte>();

            while (true)
            {
                Console.WriteLine("Press enter to allocate more memory");
                Console.ReadLine();
                for (int i = 0; i < 1024 * 1024; i++)
                {
                    bytes.AddLast(new byte());
                }
            }
        }
    }
}