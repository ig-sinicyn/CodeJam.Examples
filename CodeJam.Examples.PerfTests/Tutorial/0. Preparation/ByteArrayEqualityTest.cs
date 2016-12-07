using System;
using System.Text;

using NUnit.Framework;

namespace CodeJam.Examples.PerfTests.Tutorial
{
	public static class ByteArrayEqualityTest
	{
		public static ulong[] ToUInt64Array(byte[] bytes)
		{
			if (bytes.Length % sizeof(ulong) != 0)
				throw new ArgumentException("Invalid length", nameof(bytes));

			var result = new ulong[bytes.Length / sizeof(ulong)];
			Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
			return result;
		}

		public static byte[] ToByteArray(ulong[] uints64)
		{
			var result = new byte[uints64.Length * sizeof(ulong)];
			Buffer.BlockCopy(uints64, 0, result, 0, result.Length);
			return result;
		}

		// 1024 bit keys
		private const int UnicodeLength = 1024 / 8 / 2; // 1024 bit keys to unicode (without composite characters)
		private static readonly byte[] _bytes = Encoding.Unicode.GetBytes("Hello, world!".PadLeft(UnicodeLength));
		private static readonly byte[] _sameBytes = ToByteArray(ToUInt64Array(_bytes));
		private static readonly byte[] _otherBytes = Encoding.Unicode.GetBytes("Emm?".PadLeft(UnicodeLength));

		private static readonly ulong[] _uint64 = ToUInt64Array(_bytes);
		private static readonly ulong[] _sameUInt64 = ToUInt64Array(_sameBytes);
		private static readonly ulong[] _otherUInt64 = ToUInt64Array(_otherBytes);

		private static void TestCore(
			Func<byte[], byte[], bool> comparer,
			Func<ulong[], ulong[], bool> ulongComparer = null)
		{
			if (comparer != null)
			{
				var algName = comparer.Method.Name;
				Assert.True(
					comparer(_bytes, _bytes),
					algName + nameof(_bytes));
				Assert.True(
					comparer(_sameBytes, _sameBytes),
					algName + nameof(_sameBytes));
				Assert.True(
					comparer(_otherBytes, _otherBytes),
					algName + nameof(_otherBytes));
				Assert.True(
					comparer(_bytes, _sameBytes),
					algName + nameof(_bytes) + nameof(_sameBytes));
				Assert.False(
					comparer(_bytes, _otherBytes),
					algName + nameof(_bytes) + nameof(_otherBytes));
			}

			if (ulongComparer != null)
			{
				var algName = ulongComparer.Method.Name;
				Assert.True(
					ulongComparer(_uint64, _uint64),
					algName + nameof(_uint64));
				Assert.True(
					ulongComparer(_sameUInt64, _sameUInt64),
					algName + nameof(_sameUInt64));
				Assert.True(
					ulongComparer(_otherUInt64, _otherUInt64),
					algName + nameof(_otherUInt64));
				Assert.True(
					ulongComparer(_uint64, _sameUInt64),
					algName + nameof(_uint64) + nameof(_sameUInt64));
				Assert.False(
					ulongComparer(_uint64, _otherUInt64),
					algName + nameof(_uint64) + nameof(_otherUInt64));
			}
		}

		[Test]
		public static void TestEqualsForLoop() => TestCore(
			ByteArrayEquality.EqualsForLoop, ByteArrayEquality.EqualsUInt64ForLoop);

		[Test]
		public static void TestEqualsHardcoded() => TestCore(
			null, ByteArrayEquality.EqualsUInt64Hardcoded);

		[Test]
		public static void TestEqualsLinq() => TestCore(
			ByteArrayEquality.EqualsLinq, ByteArrayEquality.EqualsUInt64Linq);

		[Test]
		public static void TestEqualsCodeJam() => TestCore(
			ByteArrayEquality.EqualsCodeJam, ByteArrayEquality.EqualsUInt64CodeJam);

		[Test]
		public static void TestEqualsVectors() => TestCore(
			ByteArrayEquality.EqualsVectors, ByteArrayEquality.EqualsUInt64Vectors);

		[Test]
		public static void TestEqualsUnsafe() => TestCore(
			ByteArrayEquality.EqualsUnsafe);

		[Test]
		public static void TestEqualsInterop() => TestCore(
			ByteArrayEquality.EqualsInterop);
	}
}