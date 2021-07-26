using System;
using Autofac;
using IAutofacContainer = Autofac.IContainer;

namespace Core.IocContainer {
    public class Container : IContainer {
        public Container() {
            _containerBuilder = new ContainerBuilder();
            _container = new Lazy<IAutofacContainer>(() => _containerBuilder.Build());
        }

        private readonly ContainerBuilder _containerBuilder;
        private readonly Lazy<IAutofacContainer> _container;
    }
}
