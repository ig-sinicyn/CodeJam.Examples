using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

// PREREQUISITES:
// * Reference to https://www.nuget.org/packages/System.Numerics.Vectors
// * Reference to https://www.nuget.org/packages/NUnit
//   (or any another testing framework CodeJam.PerfTests does support)
// * Enable unsafe code support in build project properties.

namespace CodeJam.Examples.PerfTests.Tutorial
{
	[SuppressMessage("ReSharper", "ConvertMethodToExpressionBody")]
	[SuppressMessage("ReSharper", "RedundantCast")]
	public static class ByteArrayEquality
	{
		public static bool EqualsForLoop(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}
			return true;
		}

		public static bool EqualsUInt64ForLoop(ulong[] a, ulong[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (int i = 0; i < a.Length; i++)
			{
				if (a[i] != b[i])
					return false;
			}
			return true;
		}

		public static bool EqualsUInt64Hardcoded(ulong[] a, ulong[] b)
		{
			if (a.Length != 16)
				throw new ArgumentException("Length should be == 16", nameof(a));
			if (b.Length != 16)
				throw new ArgumentException("Length should be == 16", nameof(b));

			return a[0] == b[0] && a[1] == b[1] && a[2] == b[2] && a[3] == b[3]
				&& a[4] == b[4] && a[5] == b[5] && a[6] == b[6] && a[7] == b[7]
				&& a[8] == b[8] && a[9] == b[9]
				&& a[10] == b[10] && a[11] == b[11] && a[12] == b[12] && a[13] == b[13]
				&& a[14] == b[14] && a[15] == b[15];
		}

		public static bool EqualsLinq(byte[] a, byte[] b)
		{
			return a.SequenceEqual(b);
		}

		public static bool EqualsUInt64Linq(ulong[] a, ulong[] b)
		{
			return a.SequenceEqual(b);
		}

		public static bool EqualsCodeJam(byte[] a, byte[] b)
		{
			// ReSharper disable once RedundantNameQualifier
			return CodeJam.Collections.ArrayExtensions.EqualsTo(a, b);
		}

		public static bool EqualsUInt64CodeJam(ulong[] a, ulong[] b)
		{
			// ReSharper disable once RedundantNameQualifier
			return CodeJam.Collections.ArrayExtensions.EqualsTo(a, b);
		}

		public static bool EqualsVectors(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
				return false;

			int i;
			var max = a.Length - a.Length % Vector<byte>.Count;
			for (i = 0; i < max; i += Vector<byte>.Count)
			{
				if (new Vector<byte>(a, i) != new Vector<byte>(b, i))
					return false;
			}
			if (i < a.Length)
			{
				for (; i < a.Length; i++)
				{
					if (a[i] != b[i])
						return false;
				}
			}
			return true;
		}

		public static bool EqualsUInt64Vectors(ulong[] a, ulong[] b)
		{
			if (a.Length != b.Length)
				return false;

			int i;
			var max = a.Length - a.Length % Vector<ulong>.Count;
			for (i = 0; i < max; i += Vector<ulong>.Count)
			{
				if (new Vector<ulong>(a, i) != new Vector<ulong>(b, i))
					return false;
			}
			if (i < a.Length)
			{
				for (; i < a.Length; i++)
				{
					if (a[i] != b[i])
						return false;
				}
			}
			return true;
		}

		// THANKSTO: Hafthor (http://stackoverflow.com/a/8808245/)
		// Copyright (c) 2008-2013 Hafthor Stefansson
		// Distributed under the MIT/X11 software license
		// Ref: http://www.opensource.org/licenses/mit-license.php.
		public static unsafe bool EqualsUnsafe(byte[] a1, byte[] a2)
		{
			if (a1 == null || a2 == null || a1.Length != a2.Length)
				return false;
			fixed (byte* p1 = a1, p2 = a2)
			{
				byte* x1 = p1, x2 = p2;
				int l = a1.Length;
				for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
					if (*((long*)x1) != *((long*)x2))
						return false;
				if ((l & 4) != 0)
				{
					if (*((int*)x1) != *((int*)x2))
						return false;
					x1 += 4;
					x2 += 4;
				}
				if ((l & 2) != 0)
				{
					if (*((short*)x1) != *((short*)x2))
						return false;
					x1 += 2;
					x2 += 2;
				}
				if ((l & 1) != 0)
					if (*((byte*)x1) != *((byte*)x2))
						return false;
				return true;
			}
		}

		// THANKSTO: plinth (http://stackoverflow.com/a/1445405/)
		// P/Invoke... booo...
		public static bool EqualsInterop(byte[] b1, byte[] b2)
		{
			// Validate buffers are the same length.
			// This also ensures that the count does not exceed the length of either buffer.  
			return b1.Length == b2.Length && memcmp(b1, b2, b1.Length) == 0;
		}

		[DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
		private static extern int memcmp(byte[] b1, byte[] b2, long count);
	}
}