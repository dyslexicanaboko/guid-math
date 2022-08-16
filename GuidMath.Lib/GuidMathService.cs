using System.Numerics;
using M = GuidMath.Lib.Constants.Segments.Multiplier;

namespace GuidMath.Lib
{
	public partial class GuidMathService
	{
		public readonly GuidSegments _segments;

		public GuidMathService(Guid guid)
		{
			_segments = new GuidSegments(guid);
		}

		public static string FormatAsGuidString<T>(IEnumerable<T> segments) => string.Join('-', segments);

		public Guid Add(Guid guid) => Add(ConvertToNumber(guid));

		public Guid Subtract(Guid guid) => Add(-ConvertToNumber(guid));

		public BigInteger Difference(Guid guid)
		{
			var number = -ConvertToNumber(guid);

			var result = TryBase10Subtract(number);

			return result;
		}

		public Guid Add(BigInteger number)
		{
			//TODO: Handle base cases

			//Check if it was addition or subtraction
			if (number >= 0)
			{
				TryAdd(_segments.E, number);
			}
			else
			{
				//TrySubtractBySegment(_segments.E, number);

				var result = TryBase10Subtract(number);

				var segments = ConvertToSegments(result);

				ApplySegments(segments);
			}

			var str = GetHexGuidString(_segments.GetSegments());

			if (Guid.TryParse(str, out var guid)) return guid;

			Console.WriteLine(str); //For debug when the Guid Format is wrong

			//This is an unexpected exception because the Guid Parse failed.
			throw new Exception($"Guid string {str} could not be parsed. It's not a valid Guid. Math bug.");
		}

		private void TryAdd(Segment segment, BigInteger number)
		{
			if (segment == null) throw new InvalidAdditionException();

			var sum = segment.Value + number;

			//If this Segment is not larger than max then it can be incremented.
			if (segment.Max > sum)
			{
				segment.Value = sum;

				return;
			}

			//If the segment is larger than or equal to max, then it is has to be reset to zero
			//after reducing the number by the current value and we attempt to increment the 
			//next segment.
			var remainder = sum - segment.Max; //Calculate the remainder

			//Console.WriteLine(remainder);

			segment.Value = 0; //Reset to zero

			TryAdd(segment.Left, remainder);
		}

		private void ApplySegments(GuidSegments segments)
		{
			_segments.A.Value = segments.A.Value;
			_segments.B.Value = segments.B.Value;
			_segments.C.Value = segments.C.Value;
			_segments.D.Value = segments.D.Value;
			_segments.E.Value = segments.E.Value;
		}

		//This alone is a base case that covers getting the Gap between two Guids
		private BigInteger TryBase10Subtract(BigInteger input)
		{
			var number = ConvertToNumber(_segments);

			//Sign is already applied upon input
			var result = number + input;

			//Negative numbers cannot be supported because there is no such thing as a negative Guid
			if (result.Sign < 0) throw new InvalidSubtractionException();

			return result;
		}

		//This is no longer used, but I am going to keep it around just in case
		//This also didn't work, but trying to make it work would have been inefficient
		//private void TrySubtractBySegment(Segment segment, BigInteger number)
		//{
		//	if (segment == null) throw new InvalidSubtractionException();

		//	var diff = segment.Value + number;

		//	//If this Segment is not negative (or less than zero) then then it can be decremented.
		//	if (diff >= 0)
		//	{
		//		segment.Value = diff;

		//		return;
		//	}

		//	//If the segment is less than zero, then it is has to be reset to zero
		//	//after reducing the number by the current value and we attempt to decrement the 
		//	//next segment.
		//	segment.Value = 0; //Reset to zero

		//	TrySubtractBySegment(segment.Left, diff);
		//}

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

		private static string GetHexGuidString(params Segment[] segments)
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