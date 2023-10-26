using System;

namespace AirSoftAutomationFramework.Internals.ExtensionsMethods
{
    public static class NumbersExtensions
    {
        public static decimal MathRoundFromGeneric<T>(this T value, int placesAfterDecimalPoint,
            MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
        {
            return Math.Round(Convert.ToDecimal(value),
                placesAfterDecimalPoint, midpointRounding);    
        }

        public static decimal MathAbsGeneric(this decimal value)
        {
            return Math.Abs(value);
        }

        public static double MathAbsGeneric(this double value)
        {
            return Math.Abs(value);
        }

        public static int MathAbsGeneric(this int value)
        {
            return Math.Abs(value);
        }
    }
}
