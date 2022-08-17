using GuidMath.Lib.Exceptions;
using GuidMath.Lib.Models;
using System.Numerics;

namespace GuidMath.Lib.Services
{
    public class GuidMathService
    {
        private Guid _guid;
        private readonly GuidSegments _segments;

        public GuidMathService(Guid guid)
        {
            _guid = guid;

            _segments = new GuidSegments(guid);
        }

        public Guid Add(Guid guid)
        {
            //If both are empty, then it's empty
            if (_guid == Guid.Empty && guid == Guid.Empty) return Guid.Empty;

            //If one of them is empty, then return the non-empty one
            if (_guid == Guid.Empty ^ guid == Guid.Empty) return GetNonEmpty(_guid, guid);

            return InternalAdd(GMath.ConvertToNumber(guid));
        }

        public Guid Subtract(Guid guid)
        {
            //If they are equal, then it will be Empty (Zero, A - A = 0)
            if (_guid == guid) return Guid.Empty;

            //If left argument is empty, then this is a failure
            if (_guid == Guid.Empty) throw new InvalidSubtractionException();

            return InternalAdd(-GMath.ConvertToNumber(guid));
        }
        
        public BigInteger Difference(Guid guid)
        {
            //If they are equal, then it will be Empty (Zero, A - A = 0)
            if (_guid == guid) return 0;

            //If left argument is empty, then this is a failure
            if (_guid == Guid.Empty) throw new InvalidSubtractionException();

            var number = -GMath.ConvertToNumber(guid);

            var result = TryBase10Addition(number);

            return result;
        }

        public Guid Add(BigInteger number)
        {
            //If both are empty, then it's empty
            if (_guid == Guid.Empty && number.IsZero) return Guid.Empty;

            //If left argument is empty, and the number is negative, then this is a failure
            if (_guid == Guid.Empty && number.Sign < 0) throw new InvalidSubtractionException();

            //If one of them is empty, then return the non-empty one
            if (_guid == Guid.Empty ^ number.IsZero) return GetNonEmpty(_guid, number);

            return InternalAdd(number);
        }

        private Guid GetNonEmpty(Guid left, Guid right)
        {
            if (left == Guid.Empty) return right;

            return left;
        }

        private Guid GetNonEmpty(Guid left, BigInteger right)
        {
            if (left == Guid.Empty) return GMath.ConvertToGuid(right);

            return left;
        }

        private Guid InternalAdd(BigInteger number)
        {
            var result = TryBase10Addition(number);

            var segments = GMath.ConvertToSegments(result);

            ApplySegments(segments);

            var str = GMath.GetHexGuidString(_segments.GetSegments());

            if (Guid.TryParse(str, out _guid)) return _guid;

            Console.WriteLine(str); //For debug when the Guid Format is wrong

            //This is an unexpected exception because the Guid Parse failed.
            throw new UnexpectedGuidFormatException(str);
        }

        private void ApplySegments(GuidSegments segments)
        {
            _segments.A.Value = segments.A.Value;
            _segments.B.Value = segments.B.Value;
            _segments.C.Value = segments.C.Value;
            _segments.D.Value = segments.D.Value;
            _segments.E.Value = segments.E.Value;
        }

        private BigInteger TryBase10Addition(BigInteger input)
        {
            var number = GMath.ConvertToNumber(_segments);

            //Sign is already applied upon input
            var result = number + input;

            //Negative numbers cannot be supported because there is no such thing as a negative Guid
            if (result.Sign < 0) throw new InvalidSubtractionException();
            
            if (result > Constants.GuidDecimalMax) throw new InvalidAdditionException();

            return result;
        }

        //TODO: Division and multiplication
        private BigInteger TryBase10Multiplication(BigInteger input)
        {
            var number = GMath.ConvertToNumber(_segments);

            //Sign is already applied upon input
            var result = number * input;

            //Negative numbers cannot be supported because there is no such thing as a negative Guid
            if (result.Sign < 0) throw new InvalidSubtractionException();

            if (result > Constants.GuidDecimalMax) throw new InvalidAdditionException();

            return result;
        }
    }
}
