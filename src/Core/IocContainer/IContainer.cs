using System.Collections.Generic;
using System.Reflection;

namespace Core.IocContainer {
    public interface IContainer {
        void ScanAssemblies(params string[] assemblyNames);
        void ScanAssemblies(IEnumerable<string> assemblyNames);
        void ScanAssemblies(params Assembly[] assemblies);
        void ScanAssemblies(IEnumerable<Assembly> assemblies);
        IRegistrationBuilder Register<T>() where T : class;
        T Get<T>() where T : class;
    }
}
