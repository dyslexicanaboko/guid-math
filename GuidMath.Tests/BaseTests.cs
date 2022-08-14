using GuidMath.Lib;
using NUnit.Framework;

namespace GuidMath.Tests
{
    [TestFixture]
    public class BaseTests
    {
        [Test]
        public void Increment_by_one()
        {
            var input = "67624A31-850B-4525-B4BF-778D20D47076";
            var expected = new Guid("67624A31-850B-4525-B4BF-778D20D47077");

            var svc = new GuidMathService(input);
            
            var actual = svc.Add(1);

            Assert.AreEqual(expected, actual);
        }
    }
}