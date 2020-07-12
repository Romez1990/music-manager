using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CommandLine;
using Core.Operation;
using Utils.Reflection;

namespace ConsoleApp.ArgumentParser
{
    public class OptionsBuilder : IOptionsBuilder
    {
        public OptionsBuilder(IOperationRepository operations)
        {
            _operations = operations;
        }

        private readonly IOperationRepository _operations;

        public Type CreateOptionsType()
        {
            var assemblyBuilder = CreateAssemblyBuilder();
            var typeBuilder = CreateTypeBuilder(assemblyBuilder);
            CreateProperties(typeBuilder);
            return typeBuilder.CreateType();
        }

        private AssemblyBuilder CreateAssemblyBuilder()
        {
            var assemblyName = new AssemblyName("ConsoleAppOptions");
            return AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        }

        private TypeBuilder CreateTypeBuilder(AssemblyBuilder assemblyBuilder)
        {
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("AppOptions");
            const TypeAttributes attributes = TypeAttributes.Public |
                                              TypeAttributes.Class |
                                              TypeAttributes.AutoClass |
                                              TypeAttributes.AnsiClass |
                                              TypeAttributes.BeforeFieldInit |
                                              TypeAttributes.AutoLayout;
            return moduleBuilder.DefineType("Options", attributes, typeof(OptionsBase));
        }

        private void CreateProperties(TypeBuilder typeBuilder)
        {
            _operations.FindAll().ToList().ForEach(operation =>
                CreateProperty(typeBuilder, operation.Name, typeof(bool), operation.Description));
        }

        private void CreateProperty(TypeBuilder typeBuilder, string name, Type type, string description)
        {
            var fieldBuilder = typeBuilder.DefineField($"_{name}", type, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(name, PropertyAttributes.HasDefault, type, null);
            const MethodAttributes methodAttributes = MethodAttributes.Public |
                                                      MethodAttributes.SpecialName |
                                                      MethodAttributes.HideBySig;
            propertyBuilder.SetGetMethod(CreateGetMethod(typeBuilder, fieldBuilder, name, type, methodAttributes));
            propertyBuilder.SetSetMethod(CreateSetMethod(typeBuilder, fieldBuilder, name, type, methodAttributes));
            propertyBuilder.SetCustomAttribute(CreateOperationAttribute(name));
            propertyBuilder.SetCustomAttribute(CreateOptionAttribute(name, description));
        }

        private MethodBuilder CreateGetMethod(TypeBuilder typeBuilder, FieldBuilder fieldBuilder, string name,
            Type type, MethodAttributes methodAttrs)
        {
            var getMethodBuilder = typeBuilder.DefineMethod($"get_{name}", methodAttrs, type, Type.EmptyTypes);
            var getIlGenerator = getMethodBuilder.GetILGenerator();

            getIlGenerator.Emit(OpCodes.Ldarg_0);
            getIlGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
            getIlGenerator.Emit(OpCodes.Ret);

            return getMethodBuilder;
        }

        private MethodBuilder CreateSetMethod(TypeBuilder typeBuilder, FieldBuilder fieldBuilder, string name,
            Type type, MethodAttributes methodAttrs)
        {
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{name}", methodAttrs, null, new[] {type});
            var setIlGenerator = setMethodBuilder.GetILGenerator();
            var modifyProperty = setIlGenerator.DefineLabel();
            var exitSet = setIlGenerator.DefineLabel();

            setIlGenerator.MarkLabel(modifyProperty);
            setIlGenerator.Emit(OpCodes.Ldarg_0);
            setIlGenerator.Emit(OpCodes.Ldarg_1);
            setIlGenerator.Emit(OpCodes.Stfld, fieldBuilder);

            setIlGenerator.Emit(OpCodes.Nop);
            setIlGenerator.MarkLabel(exitSet);
            setIlGenerator.Emit(OpCodes.Ret);

            return setMethodBuilder;
        }

        private CustomAttributeBuilder CreateOperationAttribute(string name)
        {
            var args = new object[] {name};
            var constructorInfo = typeof(OperationAttribute).GetConstructor(args.GetTypes());
            if (constructorInfo == null)
                throw new ConstructionNotFountException();
            return new CustomAttributeBuilder(constructorInfo, args);
        }

        private CustomAttributeBuilder CreateOptionAttribute(string name, string description)
        {
            var type = typeof(OptionAttribute);

            var args = GetOptionArgs(name);
            var constructorInfo = type.GetConstructor(args.GetTypes());
            if (constructorInfo == null)
                throw new ConstructionNotFountException();

            var helperTextProperty = type.GetProperty("HelpText");
            return new CustomAttributeBuilder(
                constructorInfo, args,
                new[] {helperTextProperty},
                new object[] {description});
        }

        private object[] GetOptionArgs(string name)
        {
            var longName = name.ToLower();
            var shortName = longName[0];
            return new object[] {shortName, longName};
        }
    }
}
