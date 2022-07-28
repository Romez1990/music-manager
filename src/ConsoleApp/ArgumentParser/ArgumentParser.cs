using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandLine;
using ConsoleApp.App;
using Core.IocContainer;
using Core.OperationEngine;
using LanguageExt;
using Utils.Reflection;

namespace ConsoleApp.ArgumentParser {
    [Service]
    public class ArgumentParser : IArgumentParser {
        public ArgumentParser(IOptionsBuilder optionsBuilder) {
            _optionsType = optionsBuilder.CreateOptionsType();
            _operationProperties = GetOperationProperties();
        }

        private readonly Type _optionsType;

        private readonly IReadOnlyList<PropertyInfo> _operationProperties;

        private IReadOnlyList<PropertyInfo> GetOperationProperties() =>
            _optionsType
                .GetProperties()
                .Where(IsOperationProperty)
                .ToArray();

        private bool IsOperationProperty(PropertyInfo property) =>
            property.IsDefined(typeof(OperationAttribute));

        public Either<ArgumentParserException, IAppOptions> Parse(IEnumerable<string> args,
            IAppOptions defaultAppOptions) {
            // TODO check result type at runtime
            var parseResult = ParseArguments(args);
            return GetOptionsFromResult(parseResult)
                .Map(options =>
                    (IAppOptions)new AppOptions(
                        GetPath(options, defaultAppOptions),
                        GetDirectoryMode(options, defaultAppOptions),
                        GetOperations(options, defaultAppOptions)
                    ));
        }

        private object ParseArguments(IEnumerable<string> args) {
            var typeArgs = new[] { _optionsType };
            var methodArgs = new object[] { args };
            return Parser.Default.InvokeGeneric<object>("ParseArguments", typeArgs, methodArgs);
        }

        private Either<ArgumentParserException, OptionsBase> GetOptionsFromResult(object parseResult) =>
            parseResult.GetType().Name.StartsWith("NotParsed") switch {
                true => new ArgumentParserException(),
                false => parseResult.GetPropertyValue<OptionsBase>("Value"),
            };

        private string GetPath(OptionsBase parsedOptions, IAppOptions defaults) =>
            parsedOptions.Path is null
                ? defaults.Path
                : parsedOptions.Path.TrimEnd('"');

        private DirectoryMode GetDirectoryMode(OptionsBase parsedOptions, IAppOptions defaults) {
            if (parsedOptions.Album)
                return DirectoryMode.Album;
            if (parsedOptions.Band)
                return DirectoryMode.Band;
            if (parsedOptions.Compilation)
                return DirectoryMode.Compilation;
            return defaults.DirectoryMode;
        }

        private IReadOnlyList<string> GetOperations(OptionsBase parsedOptions, IAppOptions defaults) {
            var operations = _operationProperties
                .Where(p => IsOperationSelected(p, parsedOptions))
                .Map(GetOperationAttribute)
                .Map(operationAttribute => operationAttribute.Name)
                .ToArray();
            return operations.Any()
                ? operations
                : defaults.Operations;
        }

        private bool IsOperationSelected(PropertyInfo property, OptionsBase parsedOptions) =>
            property.GetValue<bool>(parsedOptions);

        private OperationAttribute GetOperationAttribute(PropertyInfo property) {
            var attributes = property.GetCustomAttributes(typeof(OperationAttribute)).ToArray();
            if (attributes.Length > 1)
                throw new Exception("Operation attribute must be defined only once");
            return (OperationAttribute)attributes.First();
        }
    }
}
