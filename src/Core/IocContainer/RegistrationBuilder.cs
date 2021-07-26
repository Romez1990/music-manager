using System;

namespace Core.IocContainer {
    public class RegistrationBuilder : IRegistrationBuilder {
        public RegistrationBuilder(Action<bool, Type> callback) {
            _callback = callback;
        }

        ~RegistrationBuilder() {
            if (!_registered)
                throw new Exception();
        }

        private bool _registered;

        private readonly Action<bool, Type> _callback;

        public void As<T>() where T : class {
            _registered = true;
            _callback(false, typeof(T));
        }

        public void As(Type type) {
            _registered = true;
            _callback(false, type);
        }

        public void AsSelf() {
            _registered = true;
            _callback(true, null);
        }
    }
}
