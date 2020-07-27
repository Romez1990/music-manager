using Core.CoreEngine;
using Core.FileScanner;
using Core.FileSystem;
using Core.Test.FileSystem;
using NUnit.Framework;

namespace Core.Test.FileScanner
{
    [TestFixture]
    public class ScannerTest
    {
        [SetUp]
        public void SetUp()
        {
            _fsNodeElementFactory = new MockFsNodeElementFactory();
            _scanner = new Scanner();
        }

        private MockFsNodeElementFactory _fsNodeElementFactory;

        private IScanner _scanner;

        [Test]
        public void Scanner_ScansAlbum_Correctly()
        {
            var albumDirectoryElement = _fsNodeElementFactory.AlbumDirectoryElement;

            var newAlbumDirectoryElement = _scanner.Scan(albumDirectoryElement, Mode.Album);
            var albumContent = newAlbumDirectoryElement.Content;

            Assert.That(albumContent[0].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(albumContent[1].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(albumContent[2].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(albumContent[3].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(albumContent[13].CheckState, Is.EqualTo(CheckState.Unchecked));
            Assert.That(newAlbumDirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }

        [Test]
        public void Scanner_ScansBand_Correctly()
        {
            var bandDirectoryElement = _fsNodeElementFactory.BandDirectoryElement;

            var newBandDirectoryElement = _scanner.Scan(bandDirectoryElement, Mode.Band);
            var album1Content = ((IDirectoryElement)newBandDirectoryElement.Content[0]).Content;
            var album2Content = ((IDirectoryElement)newBandDirectoryElement.Content[1]).Content;

            Assert.That(album1Content[0].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[1].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[2].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[3].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[13].CheckState, Is.EqualTo(CheckState.Unchecked));
            Assert.That(album2Content[0].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[1].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[2].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[3].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[12].CheckState, Is.EqualTo(CheckState.Unchecked));
            Assert.That(newBandDirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }

        [Test]
        public void Scanner_ScansCompilation_Correctly()
        {
            var compilationDirectoryElement = _fsNodeElementFactory.CompilationDirectoryElement;

            var newCompilationDirectoryElement = _scanner.Scan(compilationDirectoryElement, Mode.Compilation);
            var bandDirectoryElement = (IDirectoryElement)newCompilationDirectoryElement.Content[0];
            var album1Content = ((IDirectoryElement)bandDirectoryElement.Content[0]).Content;
            var album2Content = ((IDirectoryElement)bandDirectoryElement.Content[1]).Content;

            Assert.That(album1Content[0].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[1].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[2].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[3].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album1Content[13].CheckState, Is.EqualTo(CheckState.Unchecked));
            Assert.That(album2Content[0].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[1].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[2].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[3].CheckState, Is.EqualTo(CheckState.Checked));
            Assert.That(album2Content[12].CheckState, Is.EqualTo(CheckState.Unchecked));
            Assert.That(newCompilationDirectoryElement.CheckState, Is.EqualTo(CheckState.CheckedPartially));
        }
    }
}
