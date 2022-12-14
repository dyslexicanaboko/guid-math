using GuidMath.Lib.Models;
using System.Numerics;
using M = GuidMath.Lib.Constants.Segments.Multiplier;

namespace GuidMath.Lib
{
	public static class GMath
	{
		public static string FormatAsGuidString<T>(IEnumerable<T> segments) => string.Join('-', segments);

		public static BigInteger ConvertToNumber(Guid guid) => ConvertToNumber(new GuidSegments(guid));

		public static BigInteger ConvertToNumber(GuidSegments segments)
		{
			var s = segments;

			//Using expansion, the numbers can be merged without overlapping
			var base10Number =
				s.A.Value * M.A +
				s.B.Value * M.B +
				s.C.Value * M.C +
				s.D.Value * M.D +
				s.E.Value * M.E;

			return base10Number;
		}

		public static GuidSegments ConvertToSegments(BigInteger number)
		{
			//Using the same multipliers used in expansion, the numbers can be segmented without losing digits.
			//The difference is that the previous segment has to be removed before getting the next segment.
			BigInteger offset = 0;

			var a = GetSegment(number, M.A, offset);

			offset += a * M.A;

			var b = GetSegment(number, M.B, offset);

			offset += b * M.B;

			var c = GetSegment(number, M.C, offset);

			offset += c * M.C;

			var d = GetSegment(number, M.D, offset);

			offset += d * M.D;

			var e = GetSegment(number, M.E, offset);

			var segments = new GuidSegments(a, b, c, d, e);

			return segments;
		}

		public static Guid ConvertToGuid(BigInteger number)
		{
			var segments = ConvertToSegments(number);

			var strGuid = GetHexGuidString(segments.GetSegments());

			Console.WriteLine(strGuid); //Very possible for the guid conversion to fail

			return new Guid(strGuid);
		}

		private static BigInteger GetSegment(BigInteger numerator, BigInteger denominator, BigInteger offset)
		{
			var offsetted = numerator - offset;

			//Don't bother dividing if it's smaller, it's zero
			if (offsetted < denominator) return 0;

			//Not possible for denomimantor to be zero
			var segment = offsetted / denominator;

			return segment;
		}

		public static string GetHexGuidString(params Segment[] segments)
		{
			var arr = new string[segments.Length];

			for (int i = 0; i < segments.Length; i++)
			{
				var s = segments[i];

				var f = string.Format("{0:X" + s.HexLength + "}", s.Value);

				//Remove first zero, side effect of working with BigInteger
				//https://stackoverflow.com/questions/6248086/why-does-biginteger-tostringx-prepend-a-0-for-values-between-signed-maxvalue
				if (f.Length > s.HexLength && s.Value.Sign > 0) f = f.Remove(0, 1);

				arr[i] = f;
			}

			var str = FormatAsGuidString(arr);

			return str;
		}
	}
}
