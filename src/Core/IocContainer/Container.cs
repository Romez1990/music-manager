using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Utils.Enumerable;
using IAutofacContainer = Autofac.IContainer;

namespace Core.IocContainer {
    public class Container : IContainer {
        public Container() {
            _containerBuilder = new ContainerBuilder();
            _container = new Lazy<IAutofacContainer>(() => _containerBuilder.Build());
        }

        private readonly ContainerBuilder _containerBuilder;
        private readonly Lazy<IAutofacContainer> _container;

        public void ScanAssemblies(params string[] assemblyNames) =>
            RegisterAssemblyTypes(LoadAssemblies(assemblyNames));

        public void ScanAssemblies(IEnumerable<string> assemblyNames) =>
            RegisterAssemblyTypes(LoadAssemblies(assemblyNames));

        public void ScanAssemblies(params Assembly[] assemblies) =>
            RegisterAssemblyTypes(assemblies);

        public void ScanAssemblies(IEnumerable<Assembly> assemblies) =>
            RegisterAssemblyTypes(assemblies);

        private IEnumerable<Assembly> LoadAssemblies(IEnumerable<string> assemblyNames) =>
            assemblyNames.Map(Assembly.Load);

        private void RegisterAssemblyTypes(IEnumerable<Assembly> assemblies) =>
            assemblies.ForEach(assembly =>
                _containerBuilder.RegisterAssemblyTypes(assembly)
                    .Where(IsService)
                    .As(GetAs));

        private bool IsService(Type type) =>
            type.IsDefined(typeof(ServiceAttribute), false);

        private Type GetAs(Type type) {
            var attributes = type.GetCustomAttributes(typeof(ServiceAttribute), false);
            if (attributes.Length > 1)
                throw new Exception("Service attribute must be defined only once");
            var attribute = (ServiceAttribute)attributes.First();
            return attribute.ToSelf
                ? type
                : GetInterface(type);
        }

        private Type GetInterface(Type type) =>
            type.GetInterfaces().First();

        public T Get<T>() where T : class =>
            _container.Value.Resolve<T>();
    }
}
