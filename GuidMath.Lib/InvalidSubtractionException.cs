namespace GuidMath.Lib
{
    public class InvalidSubtractionException
        : Exception
    {
        public InvalidSubtractionException()
            : base("Resultant Guid is less than zero. Guids cannot be negative.")
        {
            
        }
    }
}
