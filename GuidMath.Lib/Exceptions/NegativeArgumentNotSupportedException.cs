namespace GuidMath.Lib.Exceptions
{
    public class NegativeArgumentNotSupportedException
        : Exception
    {
        public NegativeArgumentNotSupportedException()
            : base("Supplied arguments cannot be negative for this operation.")
        {

        }
    }
}
