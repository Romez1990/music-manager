using System;
using System.IO;
using System.Reflection;

namespace Core.Test.FileSystem
{
    public class FilesHelper
    {
        public DirectoryInfo GetSolutionDirectory()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var assemblyDirectoryInfo = Directory.GetParent(assemblyLocation);
            return GetSolutionDirectoryHelper(assemblyDirectoryInfo);
        }

        private DirectoryInfo GetSolutionDirectoryHelper(DirectoryInfo directoryInfo)
        {
            if (directoryInfo.GetFiles("*.sln").Length != 0)
                return directoryInfo;

            var parent = directoryInfo.Parent;
            if (parent == null)
                throw new NullReferenceException("Put assembly inside the solution directory");
            return GetSolutionDirectoryHelper(parent);
        }
    }
}
