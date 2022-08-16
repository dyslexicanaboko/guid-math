using System.Numerics;

namespace GuidMath.Lib
{
	public class Segment
	{
		public Segment(BigInteger value, long max, int hexLength)
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