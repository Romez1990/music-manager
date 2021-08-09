namespace Core.IocContainer {
    public abstract class Module : IModule {
        public void Load(Container container) {
            _container = container;
            Load();
        }

        private Container _container;

        protected abstract void Load();

        protected RegistrationBuilder Register<T>() where T : class =>
            new((toSelf, serviceInterface) => {
                var registrationBuilder = _container.Register<T>();
                if (toSelf) {
                    registrationBuilder.AsSelf();
                } else {
                    registrationBuilder.As(serviceInterface);
                }
            });
    }
}
