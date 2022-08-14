using GuidMath.Lib;
using NUnit.Framework;

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

        private void AreGuidsEqual(string guid, long increment)
        {
            var expected = new Guid(guid);

            var svc = new GuidMathService(SomeGuid);

            var actual = svc.Add(increment);

            Assert.AreEqual(expected, actual);
        }
    }
}