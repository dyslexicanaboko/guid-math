namespace GuidMath.Lib.Exceptions
{
    public class UnexpectedGuidFormatException
        : Exception
    {
        public UnexpectedGuidFormatException(string guidString)
            : base($"Guid string {guidString} could not be parsed. It's not a valid Guid. Math bug.")
        {

        }
    }
}
