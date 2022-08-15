using GuidMath.Lib;
using NUnit.Framework;
using System.Numerics;

namespace GuidMath.Tests
{
    [TestFixture]
    public class BaseTests
    {
        private const string SomeGuidString = "67624A31-850B-4525-B4BF-778D20D47076";
        private readonly Guid SomeGuid = new Guid(SomeGuidString);

        [TestCase(1, "67624A31-850B-4525-B4BF-778D20D47077")]
        [TestCase(0x8872DF2B8F8A, "67624A31-850B-4525-B4C0-000000000000")]
        [TestCase(0, SomeGuidString)]
        [TestCase(-1, "67624A31-850B-4525-B4BF-778D20D47075")]
        [TestCase(-0x778D20D47077, "67624A31-850B-4525-B4BE-000000000000")]
        public void Add_x(long number, string expectedGuid)
        {
            AreGuidsEqual(expectedGuid, number);
        }

        [Test]
        public void Exception_when_result_is_too_large()
        {
            Assert.Throws<InvalidAdditionException>(() => {
                new GuidMathService(SomeGuid).Add(0xFFFFFFFFFFFF);
            });
        }

        [Test]
        public void Exception_when_result_is_less_than_zero()
        {
            Assert.Throws<InvalidSubtractionException>(() => {
                new GuidMathService(SomeGuid).Add(-0x8872DF2B8F8A);
            });
        }

        private void AreGuidsEqual(string guid, long increment)
        {
            var expected = new Guid(guid);

            var svc = new GuidMathService(SomeGuid);

            var actual = svc.Add(increment);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Constants.GuidHexStringMin, "0")]
        [TestCase("00000000-0000-0000-0000-000000000001", "1")]
        [TestCase("00000000-0000-0000-0000-FFFFFFFFFFFF", "281474976710655")]
        [TestCase(Constants.GuidHexStringMax, "4294967295655356553565535281474976710655")]
        //[TestCase("67624A31-850B-4525-B4BF-778D20D47077", 1)]
        //[TestCase("67624A31-850B-4525-B4BF-778D20D47077", 1)]
        public void Guid_can_be_converted_to_number(string guid, string expectedNumber)
        {
            var expected = BigInteger.Parse(expectedNumber);
            
            var actual = GuidMathService.ConvertToNumber(new Guid(guid));

            Assert.AreEqual(expected, actual);
        }
    }
}