namespace Core.Configuration
{
    public interface IConfigDriver
    {
        string GetValueOrNull(string key);
    }
}
