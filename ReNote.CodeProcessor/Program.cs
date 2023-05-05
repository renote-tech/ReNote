using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReNote.CodeProcessor
{
    internal class Program
    {
        /// <summary>
        /// Regex pattern for String parsing.
        /// </summary>
        const string STRING_PATTERN = "(?<==\\s*\")[^\"]*(?=\";)";
        /// <summary>
        /// Regex pattern for Int parsing.
        /// </summary>
        const string INT_PATTERN    = "(?<==\\s*)\\d+(?=;)";

        /// <summary>
        /// Specifies C# project paths used by some macros.
        /// </summary>
        static readonly Option projectsOption = new Option<string[]>("--projects")
        {
            Description = "Specifies one or multiple projects that ReNote CodeProcessor will take into account",
            AllowMultipleArgumentsPerToken = true
        };

        /// <summary>
        /// Specifies the target C# file.
        /// </summary>
        static readonly Option fileOption = new Option<string>("--file")
        {
            Description = "Specifies the path of the file you want to process"
        };

        /// <summary>
        /// Specifies the target variable in the file.
        /// </summary>
        static readonly Option variableOption = new Option<string>("--var")
        {
            Description = "Specifies the name of the variable you want to modify"
        };

        /// <summary>
        /// Specifies the type of variable.
        /// </summary>
        static readonly Option typeOption = new Option<string>("--type")
        {
            Description = "Specifies the type of the variable"
        };

        /// <summary>
        /// Specifies the new desired value of the variable.
        /// </summary>
        static readonly Option valueOption = new Option<string>("--value")
        {
            Description = "Specifies the new value of the variable"
        };

        /// <summary>
        /// Command line parser.
        /// </summary>
        static readonly RootCommand CommandLine = new RootCommand("Code preprocessor for ReNote")
        { 
            projectsOption,
            fileOption,
            variableOption, 
            typeOption, 
            valueOption 
        };

        /// <summary>
        /// Entry point of the application;
        /// Parses arguments and does the desired action.
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            string[] projects    = Array.Empty<string>();
            string fileName      = string.Empty;
            string variableName  = string.Empty;
            string variableType  = string.Empty;
            string variableValue = string.Empty;

            if (args.Length == 0)
                ShowUsage();

            if (args.Contains("--help") || args.Contains("-h")  || args.Contains("/h") || args.Contains("-?") || args.Contains("/?"))
                ShowUsage();

            if (args.Contains("--version"))
                ShowVersion();

            CommandLine.SetHandler(context =>
            {
                projects       = (string[])context.ParseResult.GetValueForOption(projectsOption);
                fileName       = (string)context.ParseResult.GetValueForOption(fileOption);
                variableName   = (string)context.ParseResult.GetValueForOption(variableOption);
                variableType   = (string)context.ParseResult.GetValueForOption(typeOption);
                variableValue  = (string)context.ParseResult.GetValueForOption(valueOption);
                
                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(variableName) || string.IsNullOrWhiteSpace(variableType) || string.IsNullOrWhiteSpace(variableValue))
                    context.ExitCode = 1;
            });

            int exitCode = await CommandLine.InvokeAsync(args);
            if (exitCode != 0)
                return;

            if (!File.Exists(fileName))
                ShowError("Cannot find the specified file");

            string[] fileContent = File.ReadAllLines(fileName);

            string variableContext = fileContent.FirstOrDefault(x => Regex.IsMatch(x, $@"\b{variableType}\s+{variableName}\b"));

            if (string.IsNullOrWhiteSpace(variableContext))
                ShowError("Couldn't find the specified variable");

            string newVariableContext = string.Empty;
            string originalValue = string.Empty;

            switch (variableType.ToLower())
            {
                case "string":
                    originalValue = Regex.Match(variableContext, STRING_PATTERN).Value;
                    variableValue = await Macros.GetValueAsync(projects, variableValue);

                    if (string.IsNullOrEmpty(originalValue))
                        ShowError("The original value shouldn't be empty");

                    newVariableContext = variableContext.Replace(originalValue, variableValue);
                    break;
                case "int":
                    originalValue = Regex.Match(variableContext, INT_PATTERN).Value;

                    int.TryParse(originalValue, out int oldIntValue);
                    int.TryParse(variableValue, out int newIntValue);

                    int value = variableValue.ToLower() == "$increment$" ? oldIntValue+1 : newIntValue;
                    newVariableContext = variableContext.Replace($"{oldIntValue}", $"{value}");

                    variableValue = value.ToString();
                    break;
                default:
                    ShowError("The given type is invalid");
                    break;
            }

            int index = Array.IndexOf(fileContent, variableContext);
            fileContent[index] = newVariableContext;

            File.WriteAllLines(fileName, fileContent);
            Console.WriteLine($"SUCCESS: Set '{variableName}' value from '{originalValue}' to '{variableValue}'");
        }

        /// <summary>
        /// Shows an error and exits.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public static void ShowError(string message)
        {
            Console.WriteLine($"ERROR: {message}.");
            Environment.Exit(1);
        }

        /// <summary>
        /// Shows how to use the program and exits.
        /// </summary>
        static void ShowUsage()
        {
            CommandLine.Invoke("--help");
            Environment.Exit(2);
        }

        /// <summary>
        /// Shows the project version and exits.
        /// </summary>
        static void ShowVersion()
        {
            Console.WriteLine($"ReNote Code Preprocessor - Version 1.1.2");
            Environment.Exit(2);
        }
    }
}