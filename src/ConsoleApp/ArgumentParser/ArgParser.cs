using System;
using System.Collections.Immutable;
using System.Linq;
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
            _optionsBuilder = optionsBuilder;
        }

        private readonly IOptionsBuilder _optionsBuilder;

        public Either<ArgumentParserException, AppOptions> Parse(ImmutableArray<string> args,
            AppOptionsDefault appOptionsDefault)
        {
            var optionsType = _optionsBuilder.CreateOptionsType();
            var parserResult = InvokeParser(args, optionsType);
            return ExtractOptions(parserResult)
                .Map(options => new AppOptions(
                    ResolvePath(options, appOptionsDefault.Path),
                    ResolveMode(options, appOptionsDefault.Mode),
                    ResolveOperations(options, appOptionsDefault.Operations)
                ));
        }

        private object InvokeParser(ImmutableArray<string> args, Type optionsType)
        {
            var type = Parser.Default.GetType();
            var methodArgs = new object[] {args};
            var parseArguments = ReflectionHelper.GetMethod(type, "ParseArguments", methodArgs.GetTypes());
            var genericParseArguments = parseArguments.MakeGenericMethod(optionsType);
            return genericParseArguments.Invoke(Parser.Default, methodArgs);
        }

        private Either<ArgumentParserException, OptionsBase> ExtractOptions(object parserResult)
        {
            if (parserResult.GetType().Name.StartsWith("NotParsed"))
                return Left(new ArgumentParserException());
            return ReflectionHelper.GetField<OptionsBase>(parserResult, "value");
        }

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
            var type = options.GetType();
            var attributeType = typeof(OperationAttribute);
            var operations = type.GetProperties()
                .Where(p => p.IsDefined(attributeType, false) && (bool)p.GetValue(options, null))
                .Select(p => (OperationAttribute)p.GetCustomAttributes(attributeType, false).First())
                .Select(a => a.Name)
                .ToImmutableArray();
            if (operations.Length == 0)
                return defaultOperations.Value;
            return operations;
        }
    }
}
