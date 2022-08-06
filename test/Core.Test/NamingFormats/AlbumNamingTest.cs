using Core.NamingFormats;
using LanguageExt.UnitTesting;
using NUnit.Framework;

namespace Core.Test.NamingFormats {
    [TestFixture]
    public class AlbumNamingTest {
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            var regexRenamer = new RegexMatcher();
            var numberNormalizer = new NumberNormalizer();
            _albumNaming = new AlbumNaming(regexRenamer, numberNormalizer);
        }

        private AlbumNaming _albumNaming;

        [TestCase("2009 - Asking Alexandria - Stand Up And Scream", "Stand Up And Scream (2009)")]
        [TestCase("2009 - Stand Up And Scream", "Stand Up And Scream (2009)")]
        public void NormalizeWithoutNumber(string name, string expectedName) {
            var result = _albumNaming.NormalizeWithoutNumber(name);

            result.ShouldBeSome(resultName =>
                Assert.That(resultName, Is.EqualTo(expectedName)));
        }

        [TestCase("2009 - Asking Alexandria - Stand Up And Scream", 2, 1, "2 Stand Up And Scream (2009)")]
        [TestCase("2011 - Asking Alexandria - Reckless And Relentless", 7, 1, "7 Reckless And Relentless (2011)")]
        [TestCase("2001 - Silencer - Death - Pierce", 1, 1, "1 Death - Pierce (2001)")]
        public void NormalizeWithNumber(string name, int number, int numberLength, string expectedName) =>
            RenameWithNumber(name, number, numberLength, expectedName);

        [TestCase("2009 - Asking Alexandria - Stand Up And Scream", 2, 2, "02 Stand Up And Scream (2009)")]
        [TestCase("2011 - Asking Alexandria - Reckless And Relentless", 7, 2, "07 Reckless And Relentless (2011)")]
        public void NormalizeWithNumber_AddsZeros(string name, int number, int numberLength, string expectedName) =>
            RenameWithNumber(name, number, numberLength, expectedName);

        private void RenameWithNumber(string name, int number, int numberLength, string expectedName) {
            var result = _albumNaming.NormalizeWithNumber(name, number, numberLength);

            result.ShouldBeSome(resultName =>
                Assert.That(resultName, Is.EqualTo(expectedName)));
        }
    }
}
