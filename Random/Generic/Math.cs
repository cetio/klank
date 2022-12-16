using System.Runtime.CompilerServices;

namespace Klank.Generic
{
    public class Math
    {
        public static double pi = 3.141592653589;
        private static double piRad = 57.295779513097;

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double RadiansToDegrees(double radians)
            => radians * piRad;

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double DegreesToRadians(double degrees)
            => degrees / piRad;

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double Tan(double degrees)
        {
            degrees = DegreesToRadians(degrees);
            double x = degrees - pi * (int)(degrees / pi + 0.5);
            return (x / (x * x - 2.46898)) * -2.01868 + (0.182399 * x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double Sin(double degrees)
        {
            degrees = DegreesToRadians(degrees);
            double modpi = degrees % pi;
            modpi = modpi / 6.04 * (Power(0.204 * modpi, 3) - Power(1.273885 * modpi, 2) + (0.05 * modpi) + 6.1);
            // modpi / 6.04 * (1.57 - modpi / 2) * (4.9 - Power(1 - modpi / 1.57, 2))

            if ((int)(degrees / pi + 0.5) % 2 == 0)
                modpi *= -1;

            return modpi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double Cos(double degrees)
            => Sin(degrees + pi / 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double ASin(double sin)
        {
            if (sin > 0)
                return ASin(sin * -1) * -1;

            return sin - (Power(sin, 4) / pi) - (Power(sin, 40) / pi);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double ACos(double cos)
            => ASin(cos * -1) + pi / 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double SCos(double degrees)
            => Sin(Cos(degrees * 24));


        [MethodImpl(MethodImplOptions.AggressiveInlining |
        MethodImplOptions.AggressiveOptimization)]
        public static double Power(double x, int n)
        {
            double result = 1;
            while (n > 0)
            {
                if ((n & 1) == 0)
                {
                    x *= x;
                    n >>= 1;
                }
                else
                {
                    result *= x;
                    --n;
                }
            }

            return result;
        }
    }
}
