using System.Text;

namespace Sln2Md
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string? solutionFile = Directory.GetFiles(currentDirectory, "*.sln").FirstOrDefault();

            if (solutionFile == null)
            {
                Console.WriteLine("No solution file found in the current directory.");
                return;
            }

            string solutionName = Path.GetFileNameWithoutExtension(solutionFile);
            string markdownFileName = $"{solutionName}.md";

            var relevantExtensions = new[] { ".xaml", ".cs", ".cshtml", ".aspx", ".ascx", ".asax", ".ashx", ".asmx", ".htm", ".html", ".css", ".js", ".ts", ".json", ".config"};

            var excludedDirectories = new[] { "obj", "bin", "node_modules", "packages", ".vs", "Resources" };

            var markdownBuilder = new StringBuilder();

            markdownBuilder.AppendLine($"# {solutionName} Solution Code Files");
            markdownBuilder.AppendLine();

            ProcessDirectory(currentDirectory);

            File.WriteAllText(markdownFileName, markdownBuilder.ToString());

            Console.WriteLine($"Markdown file '{markdownFileName}' generated successfully.");

            void ProcessDirectory(string directory)
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    if (relevantExtensions.Contains(Path.GetExtension(file), StringComparer.OrdinalIgnoreCase))
                    {
                        string relativePath = file.Replace(currentDirectory, string.Empty).TrimStart('\\');
                        string fileContent = File.ReadAllText(file);

                        markdownBuilder.AppendLine($"## {relativePath}");
                        markdownBuilder.AppendLine();
                        markdownBuilder.AppendLine("```");
                        markdownBuilder.AppendLine(fileContent);
                        markdownBuilder.AppendLine("```");
                        markdownBuilder.AppendLine();
                    }
                }

                foreach (string subdirectory in Directory.GetDirectories(directory))
                {
                    if (!excludedDirectories.Contains(Path.GetFileName(subdirectory), StringComparer.OrdinalIgnoreCase))
                    {
                        ProcessDirectory(subdirectory);
                    }
                }
            }
        }
    }
}