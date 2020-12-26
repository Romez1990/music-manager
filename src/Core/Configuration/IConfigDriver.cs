#nullable enable
namespace Core.Configuration
{
    public interface IConfigDriver
    {
        string? GetString(string key);
    }
}
