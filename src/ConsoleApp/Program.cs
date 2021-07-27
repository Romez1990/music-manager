using System;
using System.Reflection;
using Core.IocContainer;

var container = new Container();
container.ScanAssemblies(Assembly.GetExecutingAssembly().FullName, "Core");

Console.WriteLine("Hello, world!");
