using GuidMath.Lib.Services;
using NUnit.Framework;

namespace GuidMath.Tests
{
    public abstract class BaseTest
    {
        protected const string SomeGuidString = "67624A31-850B-4525-B4BF-778D20D47076";
        protected readonly Guid SomeGuid = new Guid(SomeGuidString);

        protected void AreGuidsEqual(string guid, long increment)
        {
            var expected = new Guid(guid);

            var svc = new GuidMathService(SomeGuid);

            var actual = svc.Add(increment);

            Assert.AreEqual(expected, actual);
        }
    }
}
