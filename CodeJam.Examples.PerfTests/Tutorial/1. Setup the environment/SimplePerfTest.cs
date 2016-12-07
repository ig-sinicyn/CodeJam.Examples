using System;
using System.Threading;

using BenchmarkDotNet.Attributes;

using CodeJam.PerfTests;

using NUnit.Framework;

namespace CodeJam.Examples.PerfTests.Tutorial
{
	// PerfTest attribute annotations.
	// Check Configuration system and Source annotations sections for more
	[CompetitionBurstMode]
	// A perf test class.
	// Contains runner method, baseline method, implementations to benchmark, setup and cleanup methods.
	public class SimplePerfTest
	{
		// Wrap all helpers into a region to improve test readability
		// (Visual Studio collapses regions by default).
		#region Helpers
		// Constants / fields
		private static readonly int _count = CompetitionHelpers.RecommendedSpinCount;

		// Optional setup method. Same as in BenchmarkDotNet.
		[Setup]
		public void Setup() { /* We have nothing to do here. */ }

		// Optional cleanup method. Same as in BenchmarkDotNet.
		[Cleanup]
		public void Cleanup() { /* We have nothing to do here. */ }
		#endregion

		// Perftest runner method. Naming pattern is $"Run{nameof(PerfTestClass)}".
		// You may use it to write additional assertions after the perftest is completed.
		[Test]
		public void RunSimplePerfTest() => Competition.Run(this);

		// Baseline competition member. Other competition members will be compared with this.
		[CompetitionBaseline]
		public void Baseline() => Thread.SpinWait(_count);

		// Competition member #1. Should take ~3x more time to run.
		[CompetitionBenchmark(2.88, 3.12)]
		public void SlowerX3() => Thread.SpinWait(3 * _count);

		// Competition member #2. Should take ~5x more time to run.
		[CompetitionBenchmark(4.80, 5.20)]
		public void SlowerX5() => Thread.SpinWait(5 * _count);

		// Competition member #3. Should take ~7x more time to run.
		[CompetitionBenchmark(6.72, 7.28)]
		public void SlowerX7() => Thread.SpinWait(7 * _count);
	}
}