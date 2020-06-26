using Core.FileSystem;
using NUnit.Framework;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class FileTest
    {
        [SetUp]
        public void SetUp()
        {
            _fsInfoFactory = new MockFsInfoFactory();
            _file = new File(_fsInfoFactory.FileInfo);
        }

        private MockFsInfoFactory _fsInfoFactory;

        private IFile _file;

        [Test]
        public void File_Extenstion()
        {
            Assert.That(_file.Extension, Is.EqualTo(".ext"));
        }

        [Test]
        public void File_Renames_Correctly()
        {
            const string newName = "file11.ext";

            _file.Rename(newName);

            Assert.That(_file.Name, Is.EqualTo(newName));
        }
    }
}
