using System;

namespace Utils.Number {
    public static class NumberHelper {
        public static int GetLength(this int number) =>
            (int)Math.Log10(number) + 1;
    }
}
