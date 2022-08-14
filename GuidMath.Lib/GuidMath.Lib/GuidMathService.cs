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

		public string LongGuidString { get; private set; }
		public Guid HexGuid { get; private set; }

		public GuidMathService(string guid)
		{
			_guidString = guid;

			_longSegments = guid
				.Split('-')
				.Select(x => long.Parse(x, System.Globalization.NumberStyles.HexNumber))
				.ToArray();

			//Break it down for easy access
			_a = new Segment(_longSegments[0], C.Base10Segments.MaxA, 8);
			_b = new Segment(_longSegments[1], C.Base10Segments.MaxB, 4);
			_c = new Segment(_longSegments[2], C.Base10Segments.MaxC, 4);
			_d = new Segment(_longSegments[3], C.Base10Segments.MaxD, 4);
			_e = new Segment(_longSegments[4], C.Base10Segments.MaxE, 12);

			//Setup the relationships between the segments
			_b.Left = _a;
			_c.Left = _b;
			_d.Left = _c;
			_e.Left = _d;

			LongGuidString = FormatAsGuidString(_longSegments);

			HexGuid = new Guid(_guidString);
		}

		private string FormatAsGuidString<T>(IEnumerable<T> segments) => string.Join('-', segments);

		public Guid Add(long number)
		{
			TryAdd(_e, number);

			var str = GetHexGuidString(_a, _b, _c, _d, _e);

			//Console.WriteLine(str);

			return new Guid(str);
		}

		private void TryAdd(Segment segment, long number)
		{
			if (segment == null) throw new Exception("Guid overflow - cannot increment further, max reached.");

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

		private string GetHexGuidString(params Segment[] segments)
		{
			var arr = segments
				.Select(s => string.Format("{0:X" + s.HexLength + "}", s.Value))
				.ToArray();

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

			public long Value { get; set; }

			public Segment Left { get; set; }
		}
	}
}