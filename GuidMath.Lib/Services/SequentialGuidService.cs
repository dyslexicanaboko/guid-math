namespace GuidMath.Lib.Services
{
    /// <summary>Alternative to rpcrt4.dll for sequential Guid generation.</summary>
    public class SequentialGuidService
    {
        private readonly GuidMathService _math;
        private readonly Guid _seedGuid;
        private int _i;

        public SequentialGuidService()
        {
            _seedGuid = Guid.NewGuid();

            _math = new GuidMathService(_seedGuid);

            _i = 0;
        }

        public Guid CreateSequentialGuid()
        {
            var nextGuid = _math.Add(_i);

            _i++;

            return nextGuid;
        }
    }
}
