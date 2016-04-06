using CommandLine;
using CommandLine.Text;

namespace Stress
{
    public enum OptionTypes
    {
        cpu = 0,
        ram,
        disk,
    }

    public class CpuOptions
    {
        [Option("dry-run", HelpText = "Do not stress out the cpu, just report what we would have done")]
        public bool DryRun { get; set; }
    }

    public class DiskOptions
    {
        [Option("dry-run", HelpText = "Do not stress out the disk, just report what we would have done")]
        public bool DryRun { get; set; }

        [Option("size", DefaultValue = 1024, HelpText = "The size of the file to create")]
        public int FileSizeMb { get; set; }

        [Option("path", HelpText = "The path to use when testing disk, defaults to the current directory")]
        public string Path { get; set; }
    }

    public class RamOptions
    {
        [Option("dry-run", HelpText = "Do not stress out the memory, just report what we would have done")]
        public bool DryRun { get; set; }
    }

    public class StressOptions
    {
        public StressOptions()
        {
            Cpu = new CpuOptions();
            Ram = new RamOptions();
            Disk = new DiskOptions();
        }

        [VerbOption(nameof(OptionTypes.cpu), HelpText = "Stress out the CPUs")]
        public CpuOptions Cpu { get; set; }

        [VerbOption(nameof(OptionTypes.disk), HelpText = "Stress out the memory")]
        public DiskOptions Disk { get; set; }

        [VerbOption(nameof(OptionTypes.ram), HelpText = "Stress out the memory")]
        public RamOptions Ram { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("Stress", typeof(StressOptions).Assembly.GetName().Version.ToString()),
                Copyright = new CopyrightInfo("Josh Quintus", 2015),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };

            help.AddPreOptionsLine("Stress is a command line tool that quickly and easily sucks up resources on your machine. It's intended to be used to test the impact of misbehaving processes.");
            help.AddOptions(this);
            return help;
        }
    }
}