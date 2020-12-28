using Core.Renaming;
using NUnit.Framework;

namespace Core.Test.Renaming
{
    [TestFixture]
    public class AlbumRenamerTest
    {
        [SetUp]
        public void SetUp()
        {
            _albumRenamer = new AlbumRenamer();
        }

        private AlbumRenamer _albumRenamer;

        [Test]
        public void RenameAlbumWithoutNumber_WithBand()
        {
            var result = _albumRenamer.RenameAlbumWithoutNumber("2009 - Asking Alexandria - Stand Up And Scream");

            Assert.That(result, Is.EqualTo("Stand Up And Scream (2009)"));
        }

        [Test]
        public void RenameAlbumWithoutNumber_WithoutBand()
        {
            var result = _albumRenamer.RenameAlbumWithoutNumber("2009 - Stand Up And Scream");

            Assert.That(result, Is.EqualTo("Stand Up And Scream (2009)"));
        }

        [Test]
        public void RenameAlbumWithNumber_1Number()
        {
            var result = _albumRenamer.RenameAlbumWithNumber("2009 - Asking Alexandria - Stand Up And Scream", 1, 1);

            Assert.That(result, Is.EqualTo("1 Stand Up And Scream (2009)"));
        }

        [Test]
        public void RenameAlbumWithNumber_04Number()
        {
            var result = _albumRenamer.RenameAlbumWithNumber("2009 - Asking Alexandria - Stand Up And Scream", 2, 4);

            Assert.That(result, Is.EqualTo("04 Stand Up And Scream (2009)"));
        }

        [Test]
        public void RenameAlbumWithNumber_13Number()
        {
            var result = _albumRenamer.RenameAlbumWithNumber("2009 - Asking Alexandria - Stand Up And Scream", 2, 13);

            Assert.That(result, Is.EqualTo("13 Stand Up And Scream (2009)"));
        }
    }
}
