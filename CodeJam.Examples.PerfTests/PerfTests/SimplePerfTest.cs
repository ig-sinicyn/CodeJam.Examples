using System;
using System.Threading;

using CodeJam.PerfTests;

using NUnit.Framework;

namespace CodeJam.Examples.PerfTests
{
	// A perf test class.
	[CompetitionBurstMode]
	public class SimplePerfTest
	{
		private static readonly int _count = CompetitionHelpers.RecommendedSpinCount;

		// Perf test runner method.
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