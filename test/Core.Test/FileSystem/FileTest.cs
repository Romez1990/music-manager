using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using Core.FileSystem;
using NUnit.Framework;
using Path = System.IO.Path;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class FileTest
    {
        [SetUp]
        public void SetUp()
        {
            const string fileFullName = @"D:\file.ext";
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {fileFullName, new MockFileData("")},
            });
            var fileSystemInfoFactory = new MockFileSystemInfoFactory(fileSystem);
            _file = new File(fileSystemInfoFactory, fileFullName);
        }

        private IFile _file;

        [Test]
        public void File_Exists()
        {
            Assert.That(_file.Exists);
        }

        [Test]
        public void File_Renames_Correctly()
        {
            const string newName = "file2.ext";
            var newFullName = Path.Combine(_file.DirectoryName, newName);

            _file.Rename(newName);

            Assert.That(_file.Name, Is.EqualTo(newName));
            Assert.That(_file.FullName, Is.EqualTo(newFullName));
        }

        [Test]
        public void File_Extenstion()
        {
            Assert.That(_file.Extension, Is.EqualTo(".ext"));
        }

        [Test]
        public void File_ToString_Correctly()
        {
            Assert.That(_file.ToString(), Is.EqualTo(_file.Name));
        }
    }
}
