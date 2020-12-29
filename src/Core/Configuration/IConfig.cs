namespace Core.Configuration
{
    public interface IConfig
    {
        string GetString(string key);
        bool GetBool(string key);
    }
}
