using System.Numerics;
using Co = GuidMath.Lib.Constants.Segments;

namespace GuidMath.Lib
{
	public class GuidSegments
	{
		private readonly string _guidString;
		
		public Segment A { get; private set; }
		public Segment B { get; private set; }
		public Segment C { get; private set; }
		public Segment D { get; private set; }
		public Segment E { get; private set; }

		/// <summary>Supplied Guid.</summary>
		public Guid OriginalGuidInput { get; private set; }

		public GuidSegments(Guid guid)
		{
			OriginalGuidInput = guid;

			_guidString = guid.ToString("D"); //Just hypens

			var longSegments = _guidString
				.Split('-')
				.Select(x => long.Parse(x, System.Globalization.NumberStyles.HexNumber))
				.ToArray();

			Initialize(
				longSegments[0],
				longSegments[1],
				longSegments[2],
				longSegments[3],
				longSegments[4]);
		}

		public GuidSegments(BigInteger a, BigInteger b, BigInteger c, BigInteger d, BigInteger e)
		{
			Initialize(a, b, c, d, e);
		}

		private void Initialize(BigInteger a, BigInteger b, BigInteger c, BigInteger d, BigInteger e)
		{
			A = new Segment(a, Co.DecimalBase10.MaxA, Co.Width.A);
			B = new Segment(b, Co.DecimalBase10.MaxB, Co.Width.B);
			C = new Segment(c, Co.DecimalBase10.MaxC, Co.Width.C);
			D = new Segment(d, Co.DecimalBase10.MaxD, Co.Width.D);
			E = new Segment(e, Co.DecimalBase10.MaxE, Co.Width.E);

			//Setup the relationships between the segments
			B.Left = A;
			C.Left = B;
			D.Left = C;
			E.Left = D;
		}

		public Segment[] GetSegments() => new Segment[] { A, B, C, D, E };

		/// <summary>Decimal (base10) representation in Guid D string format.</summary>
		public string ToDecimalString()
		{
			var segments = GetSegments().Select(x => x.Value.ToString()).ToArray();

			var guidDecimalString = GuidMathService.FormatAsGuidString(segments);

			return guidDecimalString;
		}

		public BigInteger ToNumber() => GuidMathService.ConvertToNumber(this);
	}
}
