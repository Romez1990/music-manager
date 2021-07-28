using System;
using System.IO;
using Core.IocContainer;
using Core.Serializers;
using Utils.Enumerable;

namespace Core.UserConfig {
    [Service]
    public class UserConfigService : IUserConfigService {
        public UserConfigService(ISerializerFactory serializerFactory) {
            _serializer = serializerFactory.GetSerializer(Format.Yaml, NamingConvention.SnakeCase);
            Config = ReadConfig();
        }

        private readonly ISerializer _serializer;

        public UserConfig Config { get; }

        private UserConfig ReadConfig() {
            var path = GetConfigPath();
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                return GetDefaultConfig();
            var configContent = File.ReadAllText(path);
            var config = _serializer.Deserialize<UserConfig>(configContent);
            return config ?? GetDefaultConfig();
        }

        private string GetConfigPath() {
            var extension = _serializer.FileExtension;
            var fileName = $"config{extension}";
            if (OperatingSystem.IsWindows())
                return GetConfigPathInWindows(fileName);
            if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                return GetConfigPathInUnix(fileName);
            throw new PlatformNotSupportedException();
        }

        private string GetConfigPathInWindows(string fileName) {
            const string directoryName = "MusicManager";
            var appData = Environment.GetEnvironmentVariable("APPDATA");
            if (appData is null)
                throw new SystemException("No APPDATA environment variable");
            return Path.Combine(appData, directoryName, fileName);
        }

        private string GetConfigPathInUnix(string fileName) {
            const string directoryName = "music-manager";
            var home = Environment.GetEnvironmentVariable("HOME");
            if (home is null)
                throw new SystemException("No HOME environment variable");
            return Path.Combine(home, ".config", directoryName, fileName);
        }

        private UserConfig GetDefaultConfig() =>
            new();

        public void Save() {
            var path = GetConfigPath();
            var directoryPath = Path.GetDirectoryName(path);
            if (directoryPath is null)
                throw new NullReferenceException();
            Directory.CreateDirectory(directoryPath);
            var configContent = _serializer.Serialize(Config);
            File.WriteAllText(path, configContent);
        }

        public void Save(UserConfig config) {
            CopyValues(config, Config);
            Save();
        }

        private void CopyValues(UserConfig from, UserConfig to) =>
            to.GetType()
                .GetProperties()
                .ForEach(property => {
                    var value = property.GetValue(from);
                    property.SetValue(to, value);
                });
    }
}
