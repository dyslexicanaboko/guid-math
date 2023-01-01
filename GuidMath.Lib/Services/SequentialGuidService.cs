namespace GuidMath.Lib.Services
{
    /// <summary>Alternative to rpcrt4.dll for sequential Guid generation.</summary>
    public class SequentialGuidService
    {
        private readonly GuidMathService _math;

        public Guid SeedGuid { get; private set; }

        public long GenerationCount { get; private set; }

        public SequentialGuidService()
        {
            SeedGuid = Guid.NewGuid();

            _math = new GuidMathService(SeedGuid);
        }

        public Guid CreateSequentialGuid()
        {
            //Base case - return the SeedGuid first - index 0
            if (GenerationCount == 0)
            {
                GenerationCount = 1;

                return SeedGuid;
            }

            //Keep incrementing the in-memory Guid by one
            var nextGuid = _math.Add(1);

            //Keep track of the times it has been incremented
            GenerationCount++;

            return nextGuid;
        }
    }
}
