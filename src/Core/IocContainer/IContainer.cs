namespace Core.IocContainer {
    public interface IContainer {
        T Get<T>() where T : class;
    }
}
