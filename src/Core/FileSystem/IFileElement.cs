namespace Core.FileSystem
{
    public interface IFileElement : IFsNodeElement<IFile>
    {
        IFileElement Rename(string newName);
        
        IFileElement UncheckSilently();

        IFileElement CheckSilently();
    }
}
