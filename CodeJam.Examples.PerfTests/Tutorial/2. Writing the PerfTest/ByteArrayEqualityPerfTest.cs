using System;
using System.Linq;

using BenchmarkDotNet.Attributes;

using CodeJam.PerfTests;

using NUnit.Framework;

namespace CodeJam.Examples.PerfTests.Tutorial
{
	[CompetitionAnnotateSources]
	public class ByteArrayEqualityPerfTest
	{
		// Wrap all helpers into a region to improve readability
		// (Visual Studio collapses regions by default).
		#region Helpers
		private byte[] _arrayA;
		private byte[] _arrayB;

		private ulong[] _arrayA2;
		private ulong[] _arrayB2;

		[GlobalSetup]
		public void Setup()
		{
			// Constant rnd seed to get repeatable results
			var rnd = new Random(0);
			_arrayA = Enumerable.Range(0, 128).Select(i => (byte)rnd.Next()).ToArray();
			_arrayB = _arrayA.ToArray();
			_arrayA2 = ByteArrayEqualityTest.ToUInt64Array(_arrayA);
			_arrayB2 = _arrayA2.ToArray();
		}
		#endregion

		// Perftest runner method. Naming pattern is $"Run{nameof(PerfTestClass)}".
		// You may use it to write additional assertions after the perftest is completed.
		[Test]
		public void RunByteArrayEqualityPerfTest() => Competition.Run(this);

		[CompetitionBaseline]
		[GcAllocations(0)]
		public bool EqualsForLoop() => ByteArrayEquality.EqualsForLoop(_arrayA, _arrayB);

		[CompetitionBenchmark(12.17, 22.79)]
		[GcAllocations(64, BinarySizeUnit.Byte)]
		public bool EqualsLinq() => ByteArrayEquality.EqualsLinq(_arrayA, _arrayB);

		[CompetitionBenchmark(0.084, 0.150)]
		[GcAllocations(0)]
		public bool EqualsCodeJam() => ByteArrayEquality.EqualsCodeJam(_arrayA, _arrayB);

		[CompetitionBenchmark(0.078, 2.570)]
		[GcAllocations(0)]
		public bool EqualsVectors() => ByteArrayEquality.EqualsVectors(_arrayA, _arrayB);

		[CompetitionBenchmark(0.16, 0.27)]
		[GcAllocations(0)]
		public bool EqualsUnsafe() => ByteArrayEquality.EqualsUnsafe(_arrayA, _arrayB);

		[CompetitionBenchmark(0.150, 0.410)]
		[GcAllocations(0)]
		public bool EqualsInterop() => ByteArrayEquality.EqualsInterop(_arrayA, _arrayB);

		[CompetitionBenchmark(0.133, 0.240)]
		[GcAllocations(0)]
		public bool EqualsUInt64ForLoop() => ByteArrayEquality.EqualsUInt64ForLoop(_arrayA2, _arrayB2);

		[CompetitionBenchmark(1.97, 3.40)]
		[GcAllocations(64, BinarySizeUnit.Byte)]
		public bool EqualsUInt64Linq() => ByteArrayEquality.EqualsUInt64Linq(_arrayA2, _arrayB2);

		[CompetitionBenchmark(0.09, 0.17)]
		[GcAllocations(0)]
		public bool EqualsUInt64Hardcoded() => ByteArrayEquality.EqualsUInt64Hardcoded(_arrayA2, _arrayB2);

		[CompetitionBenchmark(0.079, 0.150)]
		[GcAllocations(0)]
		public bool EqualsUInt64CodeJam() => ByteArrayEquality.EqualsUInt64CodeJam(_arrayA2, _arrayB2);

		[CompetitionBenchmark(0.078, 1.450)]
		[GcAllocations(0)]
		public bool EqualsUInt64Vectors() => ByteArrayEquality.EqualsUInt64Vectors(_arrayA2, _arrayB2);
	}
}