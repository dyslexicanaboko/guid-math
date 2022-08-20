using GuidMath.Lib.Exceptions;
using GuidMath.Lib.Models;
using System.Numerics;

namespace GuidMath.Lib.Services
{
    public class GuidMathService
    {
        private Guid _guid;
        private readonly GuidSegments _segments;

        public Guid Value => _guid;

        public GuidMathService(Guid guid)
        {
            _guid = guid;

            _segments = new GuidSegments(guid);
        }

        public Guid Add(Guid guid)
        {
            //If both are empty, then it's empty
            if (_guid == Guid.Empty && guid == Guid.Empty) return ApplyNumber(0);

            //If one of them is empty, then return the non-empty one
            if (_guid == Guid.Empty ^ guid == Guid.Empty) return ApplyGuid(GetNonEmpty(_guid, guid));

            return InternalAdd(GMath.ConvertToNumber(guid));
        }

        public Guid Subtract(Guid guid)
        {
            //If they are equal, then it will be Empty (Zero, A - A = 0)
            if (_guid == guid) return ApplyNumber(0);

            //If left argument is empty, then this is a failure
            if (_guid == Guid.Empty) throw new InvalidSubtractionException();

            return InternalAdd(-GMath.ConvertToNumber(guid));
        }

        //Does not apply, only looks at the difference between the two
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
            if (_guid == Guid.Empty && number.IsZero) return ApplyNumber(0);

            //If left argument is empty, and the number is negative, then this is a failure
            if (_guid == Guid.Empty && number.Sign < 0) throw new InvalidSubtractionException();

            //If one of them is empty, then return the non-empty one
            if (_guid == Guid.Empty ^ number.IsZero) return ApplyGuid(GetNonEmpty(_guid, number));

            return InternalAdd(number);
        }

        public Guid Multiply(Guid guid)
        {
            //If either one or both are empty, then return empty
            if (_guid == Guid.Empty || guid == Guid.Empty) return ApplyNumber(0);

            return InternalMultiply(GMath.ConvertToNumber(guid));
        }

        public Guid Multiply(BigInteger number)
        {
            //If either one or both are empty, then return empty
            if (_guid == Guid.Empty || number.IsZero) return ApplyNumber(0);

            //If supplied argument is negative, then the result would be negative, then this is a failure
            if (number.Sign < 0) throw new NegativeArgumentNotSupportedException();

            return InternalMultiply(number);
        }

        public Guid Divide(Guid guid)
        {
            //If numerator is empty, then return empty
            if (_guid == Guid.Empty) return Guid.Empty;

            //If denominator is empty, then raise exception
            if (guid == Guid.Empty) throw new DivideByZeroException();

            //If they are equal, then the answer is technically one, but need to mind infinity
            if (_guid == guid) return ApplyNumber(1);

            return InternaDivision(GMath.ConvertToNumber(guid));
        }

        public Guid Divide(BigInteger number)
        {
            //If numerator is empty, then return empty
            if (_guid == Guid.Empty) return Guid.Empty;

            //If denominator is empty, then raise exception
            if (number.IsZero) throw new DivideByZeroException();

            //If supplied argument is negative, then the result would be negative, then this is a failure
            if (number.Sign < 0) throw new NegativeArgumentNotSupportedException();

            return InternaDivision(number);
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

            return ApplyNumber(result);
        }

        private Guid InternalMultiply(BigInteger number)
        {
            var result = TryBase10Multiplication(number);

            return ApplyNumber(result);
        }

        private Guid InternaDivision(BigInteger number)
        {
            var result = TryBase10Division(number);

            return ApplyNumber(result);
        }

        private Guid ApplyGuid(Guid guid)
        {
            var number = GMath.ConvertToNumber(guid);

            return ApplyNumber(number);
        }

        private Guid ApplyNumber(BigInteger number)
        {
            var segments = GMath.ConvertToSegments(number);

            _segments.A.Value = segments.A.Value;
            _segments.B.Value = segments.B.Value;
            _segments.C.Value = segments.C.Value;
            _segments.D.Value = segments.D.Value;
            _segments.E.Value = segments.E.Value;

            var str = GMath.GetHexGuidString(_segments.GetSegments());

            if (Guid.TryParse(str, out _guid)) return _guid;

            Console.WriteLine(str); //For debug when the Guid Format is wrong

            //This is an unexpected exception because the Guid Parse failed.
            throw new UnexpectedGuidFormatException(str);
        }

        //Covers addition and subtraction (negative numbers)
        private BigInteger TryBase10Addition(BigInteger input)
        {
            var number = GMath.ConvertToNumber(_segments);

            //Sign is already applied upon input
            var result = number + input;

            //Negative numbers cannot be supported because there is no such thing as a negative Guid
            if (result.Sign < 0) throw new InvalidSubtractionException();
            
            if (result > Constants.GuidDecimalMax) throw new GuidOverflowException();

            return result;
        }

        //Covers multiplication only, division needs to happen separately to avoid going to infinity.
        //A/A = 1, but A * 1/A fails this because 1/A goes to infinity and makes it zero.
        //Therefore, A * 0 = 0 which is true, but not what is desired
        private BigInteger TryBase10Multiplication(BigInteger input)
        {
            var number = GMath.ConvertToNumber(_segments);

            //Ensured both arguments are positive before getting to this point
            var result = number * input;
            
            if (result > Constants.GuidDecimalMax) throw new GuidOverflowException();

            return result;
        }

        private BigInteger TryBase10Division(BigInteger input)
        {
            var number = GMath.ConvertToNumber(_segments);

            //Ensured numerator and denominator are non-zero
            //Ensured that arguments are positive before getting to this point
            var result = number * input;

            //The worst that can happen is the value goes to zero due to infinity conditions

            return result;
        }
    }
}
