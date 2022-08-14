namespace GuidMath.Lib
{
    public static class Constants
    {
        //                                      A        B    C    D    E                A          B     C     D     E
        public const string GuidHexStringMin = "00000000-0000-0000-0000-000000000000"; //0         -0    -0    -0    -0
        public const string GuidHexStringMax = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF"; //4294967295-65535-65535-65535-281474976710655

        public static class Base10Segments
        {
            public const long MaxA = 4294967295L;
            public const long MaxB = 65535L;
            public const long MaxC = 65535L;
            public const long MaxD = 65535L;
            public const long MaxE = 281474976710655L;
        }
    }
}
