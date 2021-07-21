using NUnit.Framework;
using Utils.String;

namespace Utils.Test.String {
    [TestFixture]
    public class StringHelperTest {
        [TestCase("word", ExpectedResult = "Word")]
        [TestCase("some word", ExpectedResult = "Some word")]
        [TestCase("Capital", ExpectedResult = "Capital")]
        [TestCase("Capital word", ExpectedResult = "Capital word")]
        public string Capitalize(string @string) =>
            @string.Capitalize();
    }
}
