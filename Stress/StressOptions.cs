using CommandLine;

namespace Stress
{
    public class CpuOptions
    {
        [Option("dry-run", HelpText = "Do not stress out the cpu, just report what we would have done")]
        public bool DryRun { get; set; }
    }

    public class RamOptions
    {
        [Option("dry-run", HelpText = "Do not stress out the memory, just report what we would have done")]
        public bool DryRun { get; set; }
    }

    public class StressOptions
    {
        [VerbOption("cpu", HelpText = "Stress out the CPUs")]
        public CpuOptions Cpu { get; set; }

        [VerbOption("ram", HelpText = "Stress out the memory")]
        public RamOptions Ram { get; set; }
    }
}