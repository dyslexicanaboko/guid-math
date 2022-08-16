using System.Numerics;

namespace GuidMath.Lib
{
    public static class Constants
    {
        //                                      A        B    C    D    E                A          B     C     D     E
        public const string GuidHexStringMin = "00000000-0000-0000-0000-000000000000"; //0         -0    -0    -0    -0
        public const string GuidHexStringMax = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"; //4294967295-65535-65535-65535-281474976710655

        public static class Segments
        {
            //Added for completeness, but it's still a long - I just want to be able to see it
            public static class HexBase16
            {
                public const long MaxA = 0xFFFFFFFF;
                public const long MaxB = 0xFFFF;
                public const long MaxC = 0xFFFF;
                public const long MaxD = 0xFFFF;
                public const long MaxE = 0xFFFFFFFFFFFF;
            }

            public static class DecimalBase10
            {
                public const long MaxA = 4294967295L;
                public const long MaxB = 65535L;
                public const long MaxC = 65535L;
                public const long MaxD = 65535L;
                public const long MaxE = 281474976710655L;
            }

            public static class Width
            {
                public const int A = 8;
                public const int B = 4;
                public const int C = 4;
                public const int D = 4;
                public const int E = 12;
            }

            public static class Multiplier
            {
                //The following Doubles become unreliable:
                //  1e30 yields 1000000000000000019884624838656
                //  1e25 yields 10000000000000000905969664
                //Therefore parsing explicitly
                public static readonly BigInteger A = BigInteger.Parse("1000000000000000000000000000000"); //1e30
                public static readonly BigInteger B = BigInteger.Parse("10000000000000000000000000"); //1e25
                public static readonly BigInteger C = new BigInteger(1e20D);
                public static readonly BigInteger D = new BigInteger(1e15D);
                public static readonly BigInteger E = new BigInteger(1e0D);
            }
        }        
    }
}
