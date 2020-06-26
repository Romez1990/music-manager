using Core.FileSystem;
using Moq;
using NUnit.Framework;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class FsNodeElementBaseTest
    {
        [SetUp]
        public void SetUp()
        {
            var mockFsNode = new Mock<IFsNode>(MockBehavior.Strict);
            var mockFsNodeBase = new Mock<FsNodeElementBase<IFsNode>>(mockFsNode.Object)
            {
                CallBase = true,
            };
            _fsNodeElementBaseTest = mockFsNodeBase.Object;
        }

        private FsNodeElementBase<IFsNode> _fsNodeElementBaseTest;

        [Test]
        public void FsNodeElementBase_Creates_Unchecked()
        {
            Assert.That(_fsNodeElementBaseTest.CheckState, Is.EqualTo(CheckState.Unchecked));
        }

        [Test]
        public void FsNodeElementBase_Checks_Correctly()
        {
            _fsNodeElementBaseTest.Check();

            Assert.That(_fsNodeElementBaseTest.CheckState, Is.EqualTo(CheckState.Checked));
        }

        [Test]
        public void FsNodeElementBase_Unchecks_Correctly()
        {
            Assert.That(_fsNodeElementBaseTest.CheckState, Is.EqualTo(CheckState.Unchecked));

            _fsNodeElementBaseTest.Uncheck();
        }

        [Test]
        public void FsNodeElementBase_ChecksAndUnchecks_Correctly()
        {
            _fsNodeElementBaseTest.Check();
            _fsNodeElementBaseTest.Uncheck();

            Assert.That(_fsNodeElementBaseTest.CheckState, Is.EqualTo(CheckState.Unchecked));
        }
    }
}
