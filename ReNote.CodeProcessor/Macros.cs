using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Build.Locator;

namespace ReNote.CodeProcessor
{
    internal class Macros
    {
        const int VERSION_LENGTH = 4;

        /// <summary>
        /// Returns a value from the specified macro name.
        /// </summary>
        /// <param name="projects">Optional: Specify C# project paths.</param>
        /// <param name="macroName">The macro name.</param>
        /// <returns><see cref="string"/></returns>
        public static async Task<string> GetValueAsync(string[] projects, string macroName)
        {
            if (!macroName.StartsWith("$") || !macroName.EndsWith("$"))
                return macroName;

            switch (macroName.Replace("$", "").ToLower())
            {
                case "date":
                    return GetBuildDate();
                case "version":
                    string version = await GetAutoVersionAsync(projects);
                    if (string.IsNullOrWhiteSpace(version))
                        return "0.0.0.0";

                    return version;
                default:
                    return macroName;
            }
        }
        
        /// <summary>
        /// Returns the build date with the format YYMMDDHHII where:
        /// YY is the Year    - Example 23
        /// MM is the Month   - Example 5
        /// DD is the Day     - Exampel 25
        /// HH is the Hour    - Example 1
        /// II is the Minute  - Example 10
        /// </summary>
        /// <returns><see cref="string"/></returns>
        private static string GetBuildDate()
        {
            DateTime now = DateTime.Now;
            return $"{now.Year.ToString().Substring(2)}{now.Month}{now.Day}{now.Hour}{now.Minute}";
        }

        /// <summary>
        /// Returns the automated version with the format X.Y.Z.W based on the specified C# projects.
        /// </summary>
        /// <param name="projects">The projects.</param>
        /// <returns><see cref="string"/></returns>
        private static async Task<string> GetAutoVersionAsync(string[] projects)
        {
            if (projects.Length == 0)
                Program.ShowError("No projects were specified");

            Console.WriteLine($"Calculating metrics analysis");
            MSBuildLocator.RegisterDefaults();

            int linesCount = 0;
            for (int i = 0; i < projects.Length; i++)
                linesCount += await GetProjectAutoVersionAsync(projects[i]);

            string version = linesCount.ToString();
            if (version.Length < VERSION_LENGTH)
                version = version.PadLeft(VERSION_LENGTH, '0');

            char[] versionChars = version.ToCharArray();

            version = "X.Y.Z.W";
            version = version.Replace('W', versionChars[versionChars.Length - 1]);
            version = version.Replace('Z', versionChars[versionChars.Length - 2]);
            version = version.Replace('Y', versionChars[versionChars.Length - 3]);

            string majorVersion = string.Empty;
            if (versionChars.Length > VERSION_LENGTH)
                for (int i = versionChars.Length - VERSION_LENGTH; i >= 0; i--)
                    majorVersion = versionChars[i] + majorVersion;
            else
                majorVersion = versionChars[0].ToString();

            return version.Replace("X", majorVersion);
        }

        /// <summary>
        /// Return the number of executable lines for one C# project.
        /// </summary>
        /// <param name="projectName">The project.</param>
        /// <returns><see cref="int"/></returns>
        private static async Task<int> GetProjectAutoVersionAsync(string projectName)
        {
            if (!File.Exists(projectName))
                return 0;

            Project project = await MSBuildWorkspace.Create().OpenProjectAsync(projectName);

            int linesCount = 0;
            int documentsCount = project.Documents.Count();

            for (int i = 0; i < documentsCount; i++)
            {
                double progress = (int)((i + 1) * 100.0 / documentsCount);
                Console.WriteLine($"Working ({progress}%)");

                SyntaxTree syntaxTree = await project.Documents.ElementAt(i).GetSyntaxTreeAsync();
                SyntaxNode rootNode = syntaxTree.GetRoot();

                linesCount += rootNode.DescendantNodes().Count(x => x is StatementSyntax);
                
                try
                {
                    Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                } catch { }

                Task.Delay(10).Wait();
            }

            Console.WriteLine($"Completed project: {project.Name}");

            return linesCount;
        }
    }
}