using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using CommandLine;
using ConsoleApp.Application;
using Core.CoreEngine;
using LanguageExt;
using Utils.Reflection;
using static LanguageExt.Prelude;

namespace ConsoleApp.ArgumentParser
{
    public class ArgParser : IArgParser
    {
        public ArgParser(IOptionsBuilder optionsBuilder)
        {
            _optionsType = optionsBuilder.CreateOptionsType();
            _operationProperties = GetOperationProperties();
        }

        private readonly Type _optionsType;

        private readonly ImmutableArray<PropertyInfo> _operationProperties;

        private ImmutableArray<PropertyInfo> GetOperationProperties() =>
            _optionsType
                .GetProperties()
                .Where(IsOperationProperty)
                .ToImmutableArray();

        public Either<ArgumentParserException, AppOptions> Parse(ImmutableArray<string> args,
            AppOptionsDefault appOptionsDefault)
        {
            var parserResult = InvokeParser(args);
            return ExtractOptions(parserResult)
                .Map(options => new AppOptions(
                    ResolvePath(options, appOptionsDefault.Path),
                    ResolveMode(options, appOptionsDefault.Mode),
                    ResolveOperations(options, appOptionsDefault.Operations)
                ));
        }

        private object InvokeParser(ImmutableArray<string> args)
        {
            var type = Parser.Default.GetType();
            var methodArgs = new object[] {args};
            var parseArguments = ReflectionHelper.GetMethod(type, "ParseArguments", methodArgs.GetTypes());
            var genericParseArguments = parseArguments.MakeGenericMethod(_optionsType);
            return genericParseArguments.Invoke(Parser.Default, methodArgs);
        }

        private Either<ArgumentParserException, OptionsBase> ExtractOptions(object parserResult) =>
            parserResult.GetType().Name.StartsWith("NotParsed") switch
            {
                true => Left(new ArgumentParserException()),
                false => ReflectionHelper.GetField<OptionsBase>(parserResult, "value"),
            };

        private string ResolvePath(OptionsBase parserResult, Lazy<string> defaultPath) =>
            parserResult.Path ?? defaultPath.Value;

        private Mode ResolveMode(OptionsBase options, Mode defaultMode)
        {
            if (options.Album)
                return Mode.Album;
            if (options.Band)
                return Mode.Band;
            if (options.Compilation)
                return Mode.Compilation;
            return defaultMode;
        }

        private ImmutableArray<string> ResolveOperations(OptionsBase options,
            Lazy<ImmutableArray<string>> defaultOperations)
        {
            var operations = _operationProperties
                .Where(p => IsOperationSelected(p, options))
                .Map(GetOperationAttribute)
                .Map(a => a.Name)
                .ToImmutableArray();
            if (!operations.Any())
                return defaultOperations.Value;
            return operations;
        }

        private bool IsOperationProperty(PropertyInfo property) =>
            property.IsDefined(typeof(OperationAttribute), false);

        private bool IsOperationSelected(PropertyInfo property, OptionsBase options)
        {
            var value = property.GetValue(options);
            if (value is not bool boolValue)
                throw new Exception("Operation must be bool type");
            return boolValue;
        }

        private OperationAttribute GetOperationAttribute(PropertyInfo property) =>
            (OperationAttribute)property.GetCustomAttributes(typeof(OperationAttribute), false).First();
    }
}
