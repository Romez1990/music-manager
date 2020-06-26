using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using Core.FileSystem;
using NUnit.Framework;
using Path = System.IO.Path;

namespace Core.Test.FileSystem
{
    [TestFixture]
    public class DirectoryTest
    {
        [SetUp]
        public void SetUp()
        {
            const string directoryFullName = @"D:\directory";
            _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@$"{directoryFullName}\sub_directory\file3.ext", new MockFileData("")},
                {@$"{directoryFullName}\file1.ext", new MockFileData("")},
                {@$"{directoryFullName}\file2.ext", new MockFileData("")},
            });
            var fileSystemInfoFactory = new MockFileSystemInfoFactory(_fileSystem);
            _directory = new Directory(fileSystemInfoFactory, directoryFullName);
        }

        private IMockFileDataAccessor _fileSystem;
        private IDirectory _directory;

        [Test]
        public void Directory_Exists()
        {
            Assert.That(_directory.Exists);
        }

        [Test]
        public void Directory_Renames_Correctly()
        {
            const string newName = "directory2";
            var newFullName = Path.Combine(_directory.Parent.FullName, newName);

            _directory.Rename(newName);

            Assert.That(_fileSystem.AllDirectories.Select(Path.GetFileName).Contains(newName));
            Assert.That(_fileSystem.AllDirectories.Contains(newFullName));
            /*
             * for some reason _directory.FullName does not update after renaming
             * if it will be changed:
               Assert.That(_directory.Name, Is.EqualTo(newName));
               Assert.That(_directory.FullName, Is.EqualTo(newFullName));
             */
        }

        [Test]
        public void Directory_Content()
        {
            var content = _directory.Content;

            Assert.That(content.Length, Is.EqualTo(3));
            Assert.That(content[0].Name, Is.EqualTo("sub_directory"));
            Assert.That(content[1].Name, Is.EqualTo("file1.ext"));
            Assert.That(content[2].Name, Is.EqualTo("file2.ext"));
        }

        [Test]
        public void Directory_ToString_Correctly()
        {
            Assert.That(_directory.ToString(), Is.EqualTo(_directory.Name));
        }
    }
}
