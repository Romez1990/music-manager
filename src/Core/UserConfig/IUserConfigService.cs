namespace Core.UserConfig {
    public interface IUserConfigService {
        UserConfig Config { get; }
        void Save();
        void Save(UserConfig config);
    }
}
