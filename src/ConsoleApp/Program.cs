using Autofac;
using ConsoleApp;
using ConsoleApp.Application;

var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterModule<AppModule>();
var container = containerBuilder.Build();

var app = container.Resolve<IApp>();

var exitCode = app.Run(args);
return exitCode;
