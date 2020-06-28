using Core.CoreEngine;
using Core.FileRename;
using Core.FileScanner;
using Core.FileSystem;
using Core.Operation;
using Core.Test.FileSystem;
using NUnit.Framework;

namespace Core.Test.FileRename
{
    [TestFixture]
    public class RenameTest
    {
        [SetUp]
        public void SetUp()
        {
            _fsNodeElementFactory = new MockFsNodeElementFactory();
            _rename = new Rename();
            _scanner = new Scanner();
        }

        private MockFsNodeElementFactory _fsNodeElementFactory;
        private IOperation _rename;
        private IScanner _scanner;

        [Test]
        public void Rename_RenamesAlbum_Correctly()
        {
            var albumDirectoryElement = _fsNodeElementFactory.AlbumDirectoryElement;
            var albumContent = albumDirectoryElement.Content;
            _scanner.Scan(albumDirectoryElement, Mode.Album);
            albumDirectoryElement.Content[2].Uncheck();

            _rename.Perform(albumDirectoryElement, Mode.Album);

            Assert.That(albumDirectoryElement.FsNode.Name, Is.EqualTo("Stand Up And Scream (2009)"));
            Assert.That(albumContent[0].FsNode.Name, Is.EqualTo("01 Alerion.mp3"));
            Assert.That(albumContent[1].FsNode.Name, Is.EqualTo("02 The Final Episode (Let's Change Channel).mp3"));
            Assert.That(albumContent[2].FsNode.Name, Is.EqualTo("03. A Candlelit Dinner With Inamorta.mp3"));
            Assert.That(albumContent[3].FsNode.Name, Is.EqualTo("04 Nobody Don't Dance No More.mp3"));
            Assert.That(albumContent[13].FsNode.Name, Is.EqualTo("cover.jpg"));
        }

        [Test]
        public void Rename_RenamesBand_Correctly()
        {
            var bandDirectoryElement = _fsNodeElementFactory.BandDirectoryElement;
            var album1Content = ((IDirectoryElement)bandDirectoryElement.Content[0]).Content;
            var album2Content = ((IDirectoryElement)bandDirectoryElement.Content[1]).Content;
            var album3Content = ((IDirectoryElement)bandDirectoryElement.Content[2]).Content;
            _scanner.Scan(bandDirectoryElement, Mode.Band);
            bandDirectoryElement.Content[2].Uncheck();

            _rename.Perform(bandDirectoryElement, Mode.Band);

            Assert.That(bandDirectoryElement.Content[0].FsNode.Name, Is.EqualTo("1 Stand Up And Scream (2009)"));
            Assert.That(bandDirectoryElement.Content[1].FsNode.Name, Is.EqualTo("2 Reckless And Relentless (2011)"));
            Assert.That(bandDirectoryElement.Content[2].FsNode.Name,
                Is.EqualTo("2013 - Asking Alexandria - From Death To Destiny"));
            Assert.That(album1Content[0].FsNode.Name, Is.EqualTo("01 Alerion.mp3"));
            Assert.That(album1Content[1].FsNode.Name, Is.EqualTo("02 The Final Episode (Let's Change Channel).mp3"));
            Assert.That(album1Content[2].FsNode.Name, Is.EqualTo("03 A Candlelit Dinner With Inamorta.mp3"));
            Assert.That(album1Content[3].FsNode.Name, Is.EqualTo("04 Nobody Don't Dance No More.mp3"));
            Assert.That(album1Content[13].FsNode.Name, Is.EqualTo("cover.jpg"));
            Assert.That(album2Content[0].FsNode.Name, Is.EqualTo("01 Welcome.mp3"));
            Assert.That(album2Content[1].FsNode.Name, Is.EqualTo("02 Dear Insanity.mp3"));
            Assert.That(album2Content[2].FsNode.Name, Is.EqualTo("03 Closure.mp3"));
            Assert.That(album2Content[3].FsNode.Name, Is.EqualTo("04 A Lesson Never Learned.mp3"));
            Assert.That(album2Content[12].FsNode.Name, Is.EqualTo("cover.jpg"));
            Assert.That(album3Content[0].FsNode.Name, Is.EqualTo("01. Don't Pray For Me.mp3"));
            Assert.That(album3Content[1].FsNode.Name, Is.EqualTo("02. Killing You.mp3"));
            Assert.That(album3Content[2].FsNode.Name, Is.EqualTo("03. The Death Of Me.mp3"));
            Assert.That(album3Content[3].FsNode.Name, Is.EqualTo("04. Run Free.mp3"));
            Assert.That(album3Content[13].FsNode.Name, Is.EqualTo("cover.jpg"));
        }

        [Test]
        public void Rename_RenamesCompilation_Correctly()
        {
            var compilationDirectoryElement = _fsNodeElementFactory.CompilationDirectoryElement;
            var bandDirectoryElement = (IDirectoryElement)compilationDirectoryElement.Content[0];
            var album1Content = ((IDirectoryElement)bandDirectoryElement.Content[0]).Content;
            var album2Content = ((IDirectoryElement)bandDirectoryElement.Content[1]).Content;
            var album3Content = ((IDirectoryElement)bandDirectoryElement.Content[2]).Content;
            _scanner.Scan(compilationDirectoryElement, Mode.Compilation);
            bandDirectoryElement.Content[2].Uncheck();

            _rename.Perform(compilationDirectoryElement, Mode.Compilation);

            Assert.That(bandDirectoryElement.Content[0].FsNode.Name, Is.EqualTo("1 Stand Up And Scream (2009)"));
            Assert.That(bandDirectoryElement.Content[1].FsNode.Name, Is.EqualTo("2 Reckless And Relentless (2011)"));
            Assert.That(bandDirectoryElement.Content[2].FsNode.Name,
                Is.EqualTo("2013 - Asking Alexandria - From Death To Destiny"));
            Assert.That(album1Content[0].FsNode.Name, Is.EqualTo("01 Alerion.mp3"));
            Assert.That(album1Content[1].FsNode.Name, Is.EqualTo("02 The Final Episode (Let's Change Channel).mp3"));
            Assert.That(album1Content[2].FsNode.Name, Is.EqualTo("03 A Candlelit Dinner With Inamorta.mp3"));
            Assert.That(album1Content[3].FsNode.Name, Is.EqualTo("04 Nobody Don't Dance No More.mp3"));
            Assert.That(album1Content[13].FsNode.Name, Is.EqualTo("cover.jpg"));
            Assert.That(album2Content[0].FsNode.Name, Is.EqualTo("01 Welcome.mp3"));
            Assert.That(album2Content[1].FsNode.Name, Is.EqualTo("02 Dear Insanity.mp3"));
            Assert.That(album2Content[2].FsNode.Name, Is.EqualTo("03 Closure.mp3"));
            Assert.That(album2Content[3].FsNode.Name, Is.EqualTo("04 A Lesson Never Learned.mp3"));
            Assert.That(album2Content[12].FsNode.Name, Is.EqualTo("cover.jpg"));
            Assert.That(album3Content[0].FsNode.Name, Is.EqualTo("01. Don't Pray For Me.mp3"));
            Assert.That(album3Content[1].FsNode.Name, Is.EqualTo("02. Killing You.mp3"));
            Assert.That(album3Content[2].FsNode.Name, Is.EqualTo("03. The Death Of Me.mp3"));
            Assert.That(album3Content[3].FsNode.Name, Is.EqualTo("04. Run Free.mp3"));
            Assert.That(album3Content[13].FsNode.Name, Is.EqualTo("cover.jpg"));
        }
    }
}
