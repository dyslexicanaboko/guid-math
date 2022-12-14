using GuidMath.Lib;
using GuidMath.Lib.Exceptions;
using GuidMath.Lib.Services;
using NUnit.Framework;
using System.Numerics;

namespace GuidMath.Tests
{
    [TestFixture]
    public class GuidMathServiceTests
        : BaseTest
    {
        [TestCase(1, "67624A31-850B-4525-B4BF-778D20D47077")]
        [TestCase(0x8872DF2B8F8A, "67624a31-850b-4525-b4bf-000000000000")]
        [TestCase(0, SomeGuidString)]
        [TestCase(-1, "67624A31-850B-4525-B4BF-778D20D47075")]
        [TestCase(-0x778D20D47076, "67624A31-850B-4525-B4BF-000000000000")]
        public void Add_number_to_guid(long number, string expectedGuid)
        {
            AreGuidsEqual(expectedGuid, number);
        }

        //Adding whole guids together probably has low practicallity other than fast incrementing
        [TestCase(Constants.GuidHexStringMin, Constants.GuidHexStringMin, Constants.GuidHexStringMin)]
        [TestCase(SomeGuidString, Constants.GuidHexStringMin, SomeGuidString)]
        [TestCase(SomeGuidString, "00000000-0000-0000-0000-000000000001", "67624A31-850B-4525-B4BF-778D20D47077")]
        //[TestCase(SomeGuidString, SomeGuidString)] //TODO: Need to find the expected
        public void Add_guids_together(string guidA, string guidB, string expectedGuid)
        {
            var expected = new Guid(expectedGuid);

            var svc = new GuidMathService(new Guid(guidA));

            var actual = svc.Add(new Guid(guidB));

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Constants.GuidHexStringMin, Constants.GuidHexStringMin, Constants.GuidHexStringMin)]
        [TestCase(SomeGuidString, "00000000-0000-0000-0000-000000000001", "67624A31-850B-4525-B4BF-778D20D47075")]
        [TestCase(SomeGuidString, SomeGuidString, Constants.GuidHexStringMin)]
        public void Subtract_guids(string guidA, string guidB, string expectedGuid)
        {
            var gA = new Guid(guidA);
            var gB = new Guid(guidB);

            var c = gA.CompareTo(gB);

            Guid smaller;
            Guid larger;

            if (c >= 0)
            {
                smaller = gB;
                larger = gA;
            }
            else
            {
                smaller = gA;
                larger = gB;
            }
            
            var expected = new Guid(expectedGuid);

            var svc = new GuidMathService(larger);

            var actual = svc.Subtract(smaller);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Constants.GuidHexStringMin, Constants.GuidHexStringMin, "0")]
        [TestCase(Constants.GuidHexStringMax, Constants.GuidHexStringMax, "0")]
        [TestCase(SomeGuidString, "67624A31-850B-4525-B4BF-778D20D47000", "118")]
        [TestCase(SomeGuidString, SomeGuidString, "0")]
        public void Gap_between_guids(string guidA, string guidB, string expectedNumber)
        {
            var gA = new Guid(guidA);
            var gB = new Guid(guidB);

            var c = gA.CompareTo(gB);

            Guid smaller;
            Guid larger;

            if (c >= 0)
            {
                smaller = gB;
                larger = gA;
            }
            else
            {
                smaller = gA;
                larger = gB;
            }

            var expected = BigInteger.Parse(expectedNumber);

            var svc = new GuidMathService(larger);

            var actual = svc.Difference(smaller);

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Constants.GuidHexStringMin, Constants.GuidHexStringMin, Constants.GuidHexStringMin)]
        [TestCase(SomeGuidString, Constants.GuidHexStringMin, Constants.GuidHexStringMin)]
        [TestCase("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000001")]
        [TestCase("00000000-0000-0000-0000-000000000010", "00000000-0000-0000-0000-000000000010", "00000000-0000-0000-0000-000000000100")]
        public void Multiply_guids(string guidA, string guidB, string expectedGuid)
        {
            var expected = new Guid(expectedGuid);

            var svc = new GuidMathService(new Guid(guidA));

            var actual = svc.Multiply(new Guid(guidB));

            Assert.AreEqual(expected, actual);
        }

        [TestCase("00000000-0000-0000-0000-000000000002", 2)]
        public void Multiply_guid_by_number(string guidA, int number)
        {
            var gA = new Guid(guidA);

            var expected = new GuidMathService(gA);

            //Sigma (N - 1) of A
            for (int i = 0; i < number - 1; i++)
            {
                expected.Add(gA);
            }

            var svc = new GuidMathService(gA);

            var actual = svc.Multiply(number);

            Assert.AreEqual(expected.Value, actual);
        }

        [TestCase(SomeGuidString, SomeGuidString, "00000000-0000-0000-0000-000000000001")]
        public void Divide_guids(string guidA, string guidB, string expectedGuid)
        {
            var expected = new Guid(expectedGuid);

            var svc = new GuidMathService(new Guid(guidA));

            var actual = svc.Divide(new Guid(guidB));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Exception_when_result_is_too_large()
        {
            Assert.Throws<GuidOverflowException>(() => {
                new GuidMathService(SomeGuid).Add(Constants.GuidDecimalMax);
            });
        }

        [Test]
        public void Exception_when_result_is_less_than_zero()
        {
            Assert.Throws<InvalidSubtractionException>(() => {
                new GuidMathService(SomeGuid).Add(-Constants.GuidDecimalMax);
            });
        }

        [Test]
        public void Exception_when_denominator_is_zero()
        {
            Assert.Throws<DivideByZeroException>(() => {
                new GuidMathService(SomeGuid).Divide(Guid.Empty);
            });
        }

        [Test]
        public void Exception_when_multiplication_argument_is_negative()
        {
            Assert.Throws<NegativeArgumentNotSupportedException>(() => {
                new GuidMathService(SomeGuid).Multiply(-1);
            });
        }

        [TestCase(Constants.GuidHexStringMin, "0")]
        [TestCase("00000000-0000-0000-0000-000000000001", "1")]
        [TestCase("00000000-0000-0000-0000-FFFFFFFFFFFF", "281474976710655")]
        [TestCase(Constants.GuidHexStringMax, "4294967295655356553565535281474976710655")]
        public void Guid_can_be_converted_to_number(string guid, string expectedNumber)
        {
            var expected = BigInteger.Parse(expectedNumber);
            
            var actual = GMath.ConvertToNumber(new Guid(guid));

            Assert.AreEqual(expected, actual);
        }

        [TestCase(Constants.GuidHexStringMin, "0")]
        [TestCase("00000000-0000-0000-0000-000000000001", "1")]
        [TestCase("00000000-0000-0000-0000-FFFFFFFFFFFF", "281474976710655")]
        [TestCase(Constants.GuidHexStringMax, "4294967295655356553565535281474976710655")]
        public void Number_can_be_converted_to_guid(string expectedGuid, string number)
        {
            var expected = new Guid(expectedGuid);

            var actual = GMath.ConvertToGuid(BigInteger.Parse(number));

            Assert.AreEqual(expected, actual);
        }
    }
}