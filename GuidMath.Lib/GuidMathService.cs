using System.Numerics;
using C = GuidMath.Lib.Constants;

namespace GuidMath.Lib
{
	public class GuidMathService
	{
		private readonly string _guidString;
		private readonly long[] _longSegments;
		private Segment _a;
		private Segment _b;
		private Segment _c;
		private Segment _d;
		private Segment _e;

		/// <summary>Decimal (base10) representation of a Guid.</summary>
		public string GuidDecimalString { get; private set; }
		
		/// <summary>Supplied Guid.</summary>
		public Guid GuidInput { get; private set; }

		public GuidMathService(Guid guid)
		{
			GuidInput = guid;

			_guidString = guid.ToString("D"); //Just hypens

			_longSegments = _guidString
				.Split('-')
				.Select(x => long.Parse(x, System.Globalization.NumberStyles.HexNumber))
				.ToArray();

			//Break it down for easy access
			_a = new Segment(_longSegments[0], C.Segments.DecimalBase10.MaxA, 8);
			_b = new Segment(_longSegments[1], C.Segments.DecimalBase10.MaxB, 4);
			_c = new Segment(_longSegments[2], C.Segments.DecimalBase10.MaxC, 4);
			_d = new Segment(_longSegments[3], C.Segments.DecimalBase10.MaxD, 4);
			_e = new Segment(_longSegments[4], C.Segments.DecimalBase10.MaxE, 12);

			//Setup the relationships between the segments
			_b.Left = _a;
			_c.Left = _b;
			_d.Left = _c;
			_e.Left = _d;

			GuidDecimalString = FormatAsGuidString(_longSegments);
		}

		private string FormatAsGuidString<T>(IEnumerable<T> segments) => string.Join('-', segments);

		public Guid Add(BigInteger number)
		{
			//Check if it was addition or subtraction
			if (number >= 0)
			{
				TryAdd(_e, number);
			}
			else
			{
				TrySubtract(_e, number);
			}

			var str = GetHexGuidString(_a, _b, _c, _d, _e);

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

		private void TrySubtract(Segment segment, BigInteger number)
		{
			if (segment == null) throw new InvalidSubtractionException();

			var diff = segment.Value + number;

			//If this Segment is not negative (or less than zero) then then it can be decremented.
			if (diff >= 0)
			{
				segment.Value = diff;

				return;
			}

			//If the segment is less than zero, then it is has to be reset to zero
			//after reducing the number by the current value and we attempt to decrement the 
			//next segment.
			segment.Value = 0; //Reset to zero

			TrySubtract(segment.Left, diff);
		}

		private string GetHexGuidString(params Segment[] segments)
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

		private class Segment
		{
			public Segment(long value, long max, int hexLength)
			{
				Value = value;

				Max = max;

				HexLength = hexLength;
			}

			public int HexLength { get; set; }

			public long Max { get; set; }

			public BigInteger Value { get; set; }

			public Segment Left { get; set; }
		}
	}
}