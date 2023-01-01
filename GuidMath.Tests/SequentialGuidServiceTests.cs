using GuidMath.Lib;
using GuidMath.Lib.Services;
using NUnit.Framework;
using System.Diagnostics;
using System.Numerics;

namespace GuidMath.Tests
{
    [TestFixture]
    public class SequentialGuidServiceTests
        : BaseTest
    {
        [TestCase(10)]
        [TestCase(Constants.Segments.DecimalBase10.MaxB)]
        public void Sequential_guids_differ_by_one_each_always(long generate)
        {
            var bigOne = new BigInteger(1);

            var svcSequential = new SequentialGuidService();

            var previousGuid = svcSequential.CreateSequentialGuid();
            
            for (var i = 0L; i < generate; i++)
            {
                var nextGuid = svcSequential.CreateSequentialGuid();

                var svcMath = new GuidMathService(nextGuid);

                Assert.AreEqual(bigOne, svcMath.Difference(previousGuid));

                previousGuid = nextGuid;
            }
        }

        //Virtual machine - 32GB RAM on AMD Ryzen 7
        [TestCase(1)]        //   000 ms -    1
        [TestCase(100)]      //   004 ms -  100
        [TestCase(1000)]     //   027 ms -   1K
        [TestCase(10000)]    //   140 ms -  10K
        [TestCase(100000)]   //   372 ms - 100K
        [TestCase(1000000)]  // 3,276 ms -   1M
        [TestCase(10000000)] //33,432 ms -  10M
        public void Sequential_guid_speed_test(long generate)
        {
            var svcSequential = new SequentialGuidService();

            var sw = new Stopwatch();
            
            sw.Start();

            for (var i = 0L; i < generate; i++)
            {
                svcSequential.CreateSequentialGuid();
            }

            sw.Stop();

            Assert.Pass($"Guids: {generate:n0} @ {sw.ElapsedMilliseconds:n0} ms");
        }
    }
}
