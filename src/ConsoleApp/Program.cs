using System.Reflection;
using ConsoleApp.App;
using Core.IocContainer;

var container = new Container();
container.ScanAssemblies(Assembly.GetExecutingAssembly().FullName, "Core");

var app = container.Get<IApp>();

return app.Run(args);
