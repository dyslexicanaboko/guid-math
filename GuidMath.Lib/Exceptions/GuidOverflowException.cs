namespace GuidMath.Lib.Exceptions
{
    public class GuidOverflowException
        : Exception
    {
        public GuidOverflowException()
            : base("Resultant Guid is too large and will overflow 2^128.")
        {

        }
    }
}
