using System;

namespace Core.IocContainer {
    public interface IRegistrationBuilder {
        void As<T>() where T : class;
        void As(Type type);
        void AsSelf();
    }
}
