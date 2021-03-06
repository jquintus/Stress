﻿using CommandLine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Stress
{
    public class Program
    {
        private static bool _run = true;

        public static void Main(string[] args)
        {
            try
            {
                var options = new StressOptions();
                if (args == null || !args.Any())
                {
                    args = new string[] { "--help" };
                }

                Parser.Default.ParseArguments(args, options,
                    (verb, subOptions) =>
                    {
                        switch (verb?.ToLower())
                        {
                            case nameof(OptionTypes.cpu):
                                StressCpu(options.Cpu.DryRun);
                                break;

                            case nameof(OptionTypes.ram):
                                StressMem(options.Ram.DryRun);
                                break;

                            case nameof(OptionTypes.disk):
                                StressDisk(options.Disk);
                                break;

                            default:
                                Console.WriteLine(options.GetUsage());
                                break;
                        }
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

        private static void StressDisk(DiskOptions disk)
        {
            var filePath = disk.Path;

            if (filePath == null)
            {
                var file = new FileInfo(Path.GetTempFileName()).Name;
                var dir = Directory.GetCurrentDirectory();

                filePath = Path.Combine(dir, file);
            }

            Console.WriteLine($"Stressing out disk by writing {disk.FileSizeMb}MB file to {filePath}");
            if (disk.DryRun) return;

            var pagesPerMB = 2;
            var data = new byte[1000000 / pagesPerMB];

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < disk.FileSizeMb * pagesPerMB; i++)
            {
                File.WriteAllBytes(filePath, data);
            }
            watch.Stop();

            Console.WriteLine($"Took {watch.Elapsed.TotalSeconds} seconds to write {disk.FileSizeMb}MB to disk");
            File.Delete(filePath);
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