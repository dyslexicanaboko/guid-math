namespace GuidMath.Lib.Exceptions
{
    public class InvalidAdditionException
        : Exception
    {
        public InvalidAdditionException()
            : base("Resultant Guid is too large and will overflow 2^128.")
        {

        }
    }
}
