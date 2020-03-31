using BenchmarkDotNet.Running;

namespace InstanceCreator.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
            //var summary = BenchmarkRunner.Run<TypeConverterBenchmarks>();
        }
    }
}
