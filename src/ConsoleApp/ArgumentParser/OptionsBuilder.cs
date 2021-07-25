using System;
using System.Reflection;
using System.Reflection.Emit;
using CommandLine;
using Core.IocContainer;
using Core.Operation;
using Utils.Enumerable;
using Utils.Reflection;

namespace ConsoleApp.ArgumentParser {
    [Service]
    public class OptionsBuilder : IOptionsBuilder {
        public OptionsBuilder(IOperationRepository operations) {
            _operations = operations;
        }

        private readonly IOperationRepository _operations;

        public Type CreateOptionsType() {
            var assemblyBuilder = CreateAssemblyBuilder();
            var typeBuilder = CreateTypeBuilder(assemblyBuilder);
            CreateProperties(typeBuilder);
            return typeBuilder.CreateType();
        }

        private AssemblyBuilder CreateAssemblyBuilder() {
            var assemblyName = new AssemblyName("ConsoleAppOptions");
            return AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        }

        private TypeBuilder CreateTypeBuilder(AssemblyBuilder assemblyBuilder) {
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("ConsoleAppOptions");
            const TypeAttributes attrs = TypeAttributes.Public |
                                         TypeAttributes.Class |
                                         TypeAttributes.AutoClass |
                                         TypeAttributes.AnsiClass |
                                         TypeAttributes.BeforeFieldInit |
                                         TypeAttributes.AutoLayout;
            return moduleBuilder.DefineType("Options", attrs, typeof(OptionsBase));
        }

        private void CreateProperties(TypeBuilder typeBuilder) =>
            _operations.FindAll()
                .ForEach(operation => CreateOptionProperty(typeBuilder, operation));

        private void CreateOptionProperty(TypeBuilder typeBuilder, IOperation operation) {
            var type = typeof(bool);
            var fieldName = $"_{operation.Name.ToLower()}";
            var propertyName = operation.Name;
            var fieldBuilder = typeBuilder.DefineField(fieldName, type, FieldAttributes.Private);
            var propertyBuilder = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, type, null);
            const MethodAttributes methodAttrs = MethodAttributes.Public |
                                                 MethodAttributes.SpecialName |
                                                 MethodAttributes.HideBySig;
            propertyBuilder.SetGetMethod(CreateGetMethod(typeBuilder, fieldBuilder, propertyName, type, methodAttrs));
            propertyBuilder.SetSetMethod(CreateSetMethod(typeBuilder, fieldBuilder, propertyName, type, methodAttrs));
            propertyBuilder.SetCustomAttribute(CreateOptionAttribute(operation));
            propertyBuilder.SetCustomAttribute(CreateOperationAttribute(operation));
        }

        private MethodBuilder CreateGetMethod(TypeBuilder typeBuilder, FieldInfo fieldInfo, string name, Type type,
            MethodAttributes methodAttrs) {
            var getMethodBuilder = typeBuilder.DefineMethod($"get_{name}", methodAttrs, type, Type.EmptyTypes);
            var getIlGenerator = getMethodBuilder.GetILGenerator();
            getIlGenerator.Emit(OpCodes.Ldarg_0);
            getIlGenerator.Emit(OpCodes.Ldfld, fieldInfo);
            getIlGenerator.Emit(OpCodes.Ret);
            return getMethodBuilder;
        }

        private MethodBuilder CreateSetMethod(TypeBuilder typeBuilder, FieldInfo fieldInfo, string name, Type type,
            MethodAttributes methodAttrs) {
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{name}", methodAttrs, null, new[] { type });
            var setIlGenerator = setMethodBuilder.GetILGenerator();
            var modifyProperty = setIlGenerator.DefineLabel();
            var exitSet = setIlGenerator.DefineLabel();

            setIlGenerator.MarkLabel(modifyProperty);
            setIlGenerator.Emit(OpCodes.Ldarg_0);
            setIlGenerator.Emit(OpCodes.Ldarg_1);
            setIlGenerator.Emit(OpCodes.Stfld, fieldInfo);

            setIlGenerator.Emit(OpCodes.Nop);
            setIlGenerator.MarkLabel(exitSet);
            setIlGenerator.Emit(OpCodes.Ret);

            return setMethodBuilder;
        }

        private CustomAttributeBuilder CreateOptionAttribute(IOperation operation) {
            var type = typeof(OptionAttribute);
            var longName = operation.Name.ToLower();
            var shortName = longName[0];
            var args = new object[] { shortName, longName };
            var constructorInfo = type.GetConstructorInfo(args);
            var helperTextProperty = type.GetPropertyInfo("HelpText");
            return new CustomAttributeBuilder(
                constructorInfo,
                args,
                new[] { helperTextProperty },
                new object[] { operation.Description });
        }

        private CustomAttributeBuilder CreateOperationAttribute(IOperation operation) {
            var args = new object[] { operation.Name };
            var constructorInfo = typeof(OperationAttribute).GetConstructorInfo(args);
            return new CustomAttributeBuilder(constructorInfo, args);
        }
    }
}
