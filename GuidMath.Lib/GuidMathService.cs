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

		public Guid Add(Guid guid) => Add(GMath.ConvertToNumber(guid));

		public Guid Subtract(Guid guid) => Add(-GMath.ConvertToNumber(guid));

		public BigInteger Difference(Guid guid)
		{
			var number = -GMath.ConvertToNumber(guid);

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

				var segments = GMath.ConvertToSegments(result);

				ApplySegments(segments);
			}

			var str = GMath.GetHexGuidString(_segments.GetSegments());

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
			var number = GMath.ConvertToNumber(_segments);

			//Sign is already applied upon input
			var result = number + input;

			//Negative numbers cannot be supported because there is no such thing as a negative Guid
			if (result.Sign < 0) throw new InvalidSubtractionException();

			return result;
		}
	}
}
