using System.Collections.Generic;
using System.Reflection;

namespace Core.IocContainer {
    public interface IContainer {
        void ScanAssemblies(params string[] assemblyNames);
        void ScanAssemblies(IEnumerable<string> assemblyNames);
        void ScanAssemblies(params Assembly[] assemblies);
        void ScanAssemblies(IEnumerable<Assembly> assemblies);
        void RegisterModule<T>() where T : class, IModule;
        IRegistrationBuilder Register<T>() where T : class;
        T Get<T>() where T : class;
    }
}
