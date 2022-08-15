using System.Numerics;

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

		public Guid Add(Guid guid)
		{
			return Add(1);
		}

		public Guid Subtract(Guid guid)
		{
			return Add(-1);
		}

		private BigInteger ConvertToNumber(Guid guid)
		{
			return 1;
		}

		public Guid Add(BigInteger number)
		{
			//Check if it was addition or subtraction
			if (number >= 0)
			{
				TryAdd(_segments.E, number);
			}
			else
			{
				TrySubtract(_segments.E, number);
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
	}
}