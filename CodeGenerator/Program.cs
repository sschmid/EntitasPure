using System;
using System.Linq;
using Entitas.CodeGenerator;

namespace CodeGenerator {

    class MainClass {

        public static void Main(string[] args) {

            // Configure code generator in Entitas.properties file
            generate("../../Entitas.properties");
        }

        static void generate(string configPath) {

            Console.WriteLine("Generating...");

            var codeGenerator = CodeGeneratorUtil.CodeGeneratorFromConfig(configPath);

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
    }
}
