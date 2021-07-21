using NUnit.Framework;
using Utils.Number;

namespace Utils.Test.Number {
    [TestFixture]
    public class NumberHelperTest {
        [TestCase(1, ExpectedResult = 1)]
        [TestCase(9, ExpectedResult = 1)]
        [TestCase(10, ExpectedResult = 2)]
        [TestCase(19, ExpectedResult = 2)]
        [TestCase(99, ExpectedResult = 2)]
        [TestCase(100, ExpectedResult = 3)]
        [TestCase(111, ExpectedResult = 3)]
        [TestCase(999, ExpectedResult = 3)]
        [TestCase(1000, ExpectedResult = 4)]
        public int GetLength(int number) =>
            number.GetLength();
    }
}
