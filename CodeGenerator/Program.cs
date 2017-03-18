using System;
using System.Linq;
using System.Reflection;
using Entitas;
using Entitas.CodeGenerator;

namespace CodeGenerator {

    class MainClass {

        public static void Main(string[] args) {

            // Configure code generator in Entitas.properties file
            generate("../../Entitas.properties");
        }

        static void generate(string configPath) {

            EntitasPreferences.CONFIG_PATH = configPath;
            var config = new CodeGeneratorConfig(EntitasPreferences.LoadConfig());

            Console.WriteLine("Generating...");

            var codeGenerator = new Entitas.CodeGenerator.CodeGenerator(
                getEnabled<ICodeGeneratorDataProvider>(config.dataProviders),
                getEnabled<ICodeGenerator>(config.codeGenerators),
                getEnabled<ICodeGenFilePostProcessor>(config.postProcessors)
            );

            var dryFiles = codeGenerator.DryRun();
            var files = codeGenerator.Generate();

            var totalGeneratedFiles = files.Select(file => file.fileName).Distinct().Count();

            var sloc = dryFiles
                .Select(file => file.fileContent.ToUnixLineEndings())
                .Sum(content => content.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length);

            var loc = files
                .Select(file => file.fileContent.ToUnixLineEndings())
                .Sum(content => content.Split(new[] { '\n' }).Length);

            Console.WriteLine("Generated " + totalGeneratedFiles + " files (" + sloc + " sloc, " + loc + " loc)");
        }

        static T[] getEnabled<T>(string[] types) {
            return GetTypes<T>()
                .Where(type => types.Contains(type.FullName))
                .Select(type => (T)Activator.CreateInstance(type))
                .ToArray();
        }

        public static Type[] GetTypes<T>() {
            return Assembly.GetAssembly(typeof(T)).GetTypes()
                           .Where(type => type.ImplementsInterface<T>())
                           .OrderBy(type => type.FullName)
                           .ToArray();
        }
    }
}
