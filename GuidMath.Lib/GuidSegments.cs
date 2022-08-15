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

		/// <summary>Decimal (base10) representation of a Guid.</summary>
		public string GuidDecimalString { get; private set; }

		/// <summary>Supplied Guid.</summary>
		public Guid GuidInput { get; private set; }

		public GuidSegments(Guid guid)
		{
			GuidInput = guid;

			_guidString = guid.ToString("D"); //Just hypens

			var longSegments = _guidString
				.Split('-')
				.Select(x => long.Parse(x, System.Globalization.NumberStyles.HexNumber))
				.ToArray();

			//Break it down for easy access
			A = new Segment(longSegments[0], Co.DecimalBase10.MaxA, Co.Width.A);
			B = new Segment(longSegments[1], Co.DecimalBase10.MaxB, Co.Width.B);
			C = new Segment(longSegments[2], Co.DecimalBase10.MaxC, Co.Width.C);
			D = new Segment(longSegments[3], Co.DecimalBase10.MaxD, Co.Width.D);
			E = new Segment(longSegments[4], Co.DecimalBase10.MaxE, Co.Width.E);

			//Setup the relationships between the segments
			B.Left = A;
			C.Left = B;
			D.Left = C;
			E.Left = D;

			GuidDecimalString = GuidMathService.FormatAsGuidString(longSegments);
		}

		public Segment[] GetSegments() => new Segment[] { A, B, C, D, E };
	}
}
