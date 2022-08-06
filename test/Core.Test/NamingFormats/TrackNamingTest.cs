using System.Collections.Generic;
using Core.NamingFormats;
using LanguageExt.UnitTesting;
using NUnit.Framework;

namespace Core.Test.NamingFormats {
    [TestFixture]
    public class TrackNamingTest {
        [OneTimeSetUp]
        public void OneTimeSetUp() {
            var regexRenamer = new RegexMatcher();
            var numberNormalizer = new NumberNormalizer();
            _trackNaming = new TrackNaming(regexRenamer, numberNormalizer);
        }

        private TrackNaming _trackNaming;

        [TestCase(new object[] {
            new[] {
                "1 Band - title a",
                "2 Band - title b",
                "10 Band - title c",
            },
        }, ExpectedResult = 6)]
        [TestCase(new object[] {
            new[] {
                "1 Band - Name - title a",
                "2 Band - Name - title b",
                "10 Band - Name - title c",
            },
        }, ExpectedResult = 13)]
        [TestCase(new object[] {
            new string[] { },
        }, ExpectedResult = 0)]
        [TestCase(new object[] {
            new[] {
                "1 Band - title a",
                "2 Band - title b",
                "10 title c",
            },
        }, ExpectedResult = 0)]
        [TestCase(new object[] {
            new[] {
                "1 Band - title a",
                "2 Band - title b",
                "10 AnotherBand - title c",
            },
        }, ExpectedResult = 0)]
        public int FindPrefixLength(IEnumerable<string> trackNames) =>
            _trackNaming.FindPrefixLength(trackNames);

        [TestCase("4. title", 4, 1, "4 title")]
        [TestCase("1 - title", 1, 1, "1 title")]
        public void Rename_Normalizes(string name, int number, int numberLength, string expectedName) =>
            Rename(name, number, numberLength, expectedName);

        [TestCase("04 title", 4, 1, "4 title")]
        [TestCase("01 title", 1, 1, "1 title")]
        public void Rename_RemovesZeros(string name, int number, int numberLength, string expectedName) =>
            Rename(name, number, numberLength, expectedName);

        [TestCase("4 title", 4, 2, "04 title")]
        [TestCase("1 title", 1, 2, "01 title")]
        public void Rename_AddsZeros(string name, int number, int numberLength, string expectedName) =>
            Rename(name, number, numberLength, expectedName);

        [TestCase("4 title", 3, 1, "3 title")]
        [TestCase("1 title", 2, 1, "2 title")]
        public void Rename_ReplacesNumber(string name, int number, int numberLength, string expectedName) =>
            Rename(name, number, numberLength, expectedName);

        private void Rename(string name, int number, int numberLength, string expectedName) {
            var result = _trackNaming.Normalize(name, number, numberLength, 0);

            result.ShouldBeSome(resultName =>
                Assert.That(resultName, Is.EqualTo(expectedName)));
        }
    }
}
