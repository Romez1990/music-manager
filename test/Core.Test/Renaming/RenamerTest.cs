using System;
using System.Collections.Immutable;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Renaming;
using Moq;
using NUnit.Framework;

namespace Core.Test.Renaming
{
    [TestFixture]
    public class RenamerTest
    {
        [SetUp]
        public void SetUp()
        {
            _albumRenamer = new Mock<IAlbumRenamer>(MockBehavior.Strict);
            _trackRenamer = new Mock<ITrackRenamer>(MockBehavior.Strict);
            _renamer = new Renamer(_albumRenamer.Object, _trackRenamer.Object);
        }

        private Renamer _renamer;
        private Mock<IAlbumRenamer> _albumRenamer;
        private Mock<ITrackRenamer> _trackRenamer;

        [Test]
        public void Rename_RenamesAlbum_Correctly()
        {
            var album = new Mock<IDirectoryElement>(MockBehavior.Strict);
            album.SetupGet(d => d.Name).Returns("album");
            album.SetupGet(d => d.CheckState).Returns(CheckState.CheckedPartially);
            var track1 = new Mock<IFileElement>(MockBehavior.Strict);
            var track2 = new Mock<IFileElement>(MockBehavior.Strict);
            var track3 = new Mock<IFileElement>(MockBehavior.Strict);
            track1.SetupGet(f => f.Name).Returns("track1");
            track2.SetupGet(f => f.Name).Returns("track2");
            track3.SetupGet(f => f.Name).Returns("track3");
            track1.SetupGet(f => f.CheckState).Returns(CheckState.Checked);
            track2.SetupGet(f => f.CheckState).Returns(CheckState.Unchecked);
            track3.SetupGet(f => f.CheckState).Returns(CheckState.Checked);
            var albumContent = ImmutableArray.Create(new IFsNodeElement<object>[]
            {
                track1.Object,
                track2.Object,
                track3.Object,
            });

            var renamedAlbum = new Mock<IDirectoryElement>(MockBehavior.Strict);
            var renamedAlbumWithoutContent = new Mock<IDirectoryElement>(MockBehavior.Strict);
            renamedAlbumWithoutContent.SetupGet(d => d.Content).Returns(albumContent);
            album.Setup(d => d.Rename("renamedAlbum")).Returns(renamedAlbumWithoutContent.Object);
            renamedAlbumWithoutContent.Setup(d =>
                    d.MapContent(It.IsAny<Func<IFsNodeElement<object>, IFsNodeElement<object>>>()))
                .Returns(renamedAlbum.Object);
            _albumRenamer.Setup(r => r.RenameAlbumWithoutNumber("album")).Returns("renamedAlbum");
            var renamedTrack1 = new Mock<IFileElement>(MockBehavior.Strict);
            var renamedTrack3 = new Mock<IFileElement>(MockBehavior.Strict);
            track1.Setup(f => f.Rename("renamedTrack1")).Returns(renamedTrack1.Object);
            track3.Setup(f => f.Rename("renamedTrack3")).Returns(renamedTrack3.Object);

            var result = _renamer.Rename(album.Object, Mode.Album);

            Assert.That(result, Is.EqualTo(renamedAlbum.Object));
            _albumRenamer.Verify(r => r.RenameAlbumWithoutNumber("album"), Times.Once());
        }
    }
}
