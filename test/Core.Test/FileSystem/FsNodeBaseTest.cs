using System.IO.Abstractions;
using Core.FileSystem;
using Moq;
using NUnit.Framework;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class FsNodeBaseTest
    {
        [SetUp]
        public void SetUp()
        {
            _fsInfoFactory = new MockFsInfoFactory();
            var info = _fsInfoFactory.FileInfo;
            var mockFsNodeBase = new Mock<FsNodeBase<IFsNode<object>, IFileSystemInfo>>(MockBehavior.Strict, info);
            _fsNodeBase = mockFsNodeBase.Object;
        }

        private MockFsInfoFactory _fsInfoFactory;

        private FsNodeBase<IFsNode<object>, IFileSystemInfo> _fsNodeBase;

        [Test]
        public void FsNodeBase_Name()
        {
            Assert.That(_fsNodeBase.Name, Is.EqualTo("file1.ext"));
        }

        [Test]
        public void FsNodeBase_Path()
        {
            Assert.That(_fsNodeBase.Path, Is.EqualTo(_fsInfoFactory.FileInfo.FullName));
        }
    }
}
