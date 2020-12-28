using Core.Renaming;
using NUnit.Framework;

namespace Core.Test.Renaming
{
    [TestFixture]
    public class TrackRenamerTest
    {
        [SetUp]
        public void SetUp()
        {
            _trackRenamer = new TrackRenamer();
        }

        private TrackRenamer _trackRenamer;

        [Test]
        public void RenameTrack_RemovesDot()
        {
            var result = _trackRenamer.RenameTrack("01. Alerion.mp3", false);

            Assert.That(result, Is.EqualTo("01 Alerion.mp3"));
        }

        [Test]
        public void RenameTrack_IfNoDot_ReturnsSameName()
        {
            var result = _trackRenamer.RenameTrack("01 Alerion.mp3", false);

            Assert.That(result, Is.EqualTo("01 Alerion.mp3"));
        }

        [Test]
        public void RenameTrack_IfIsTrackNumberOneDigit_RemovesZero()
        {
            var result = _trackRenamer.RenameTrack("01 Alerion.mp3", true);

            Assert.That(result, Is.EqualTo("1 Alerion.mp3"));
        }
    }
}
