using NUnit.Framework;

[assembly: LevelOfParallelism(15)]
[assembly: Parallelizable(ParallelScope.Fixtures)]