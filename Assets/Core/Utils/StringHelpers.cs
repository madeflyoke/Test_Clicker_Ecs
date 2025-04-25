using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Core.Utils
{
    public static class StringHelpers
    {
        public static string ToIntFloorString(this double value)
        {
            return Math.Floor(value).ToString(CultureInfo.InvariantCulture);
        }

        public static string WithDollarPostfix(this string value)
        {
            return value + "$";
        }

        public static string ToPlusPercentView(this string value)
        {
            return "+" + value + "%";
        }
    }
}
