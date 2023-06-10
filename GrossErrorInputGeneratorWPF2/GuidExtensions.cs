using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrossErrorInputGeneratorWPF2
{
    public static class GuidExtensions
    {
        public static string PrependZero(this string target, int times)
        {
            for (int i = 0; i < times; i++)
            {
                target = "0" + target;
            }

            return target;
        }

        public static double GetPseudoDoubleWithinRange(double lowerBound, double upperBound)
        {
            var random = new Random();
            var rDouble = random.NextDouble();
            var rRangeDouble = rDouble * (upperBound - lowerBound) + lowerBound;
            return rRangeDouble;
        }

        public static int GetPseudoIntWithinRange(int lowerBound, int upperBound)
        {
            var random = new Random();
            var rDouble = random.NextDouble();
            var rRangeDouble = rDouble * (upperBound - lowerBound) + lowerBound;
            return (int)rRangeDouble;
        }
    }
}
