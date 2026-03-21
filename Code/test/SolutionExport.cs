using System;
using System.IO;
using System.Collections.Generic;
using ClosedXML.Excel;

public class SolutionExport
{
    public string TaskId { get; set; }
    public string EntryPoint { get; set; }
    public string Benchmark { get; set; }
    public string Test { get; set; }
    public string ChatGpt { get; set; }
    public string Gemini { get; set; }
    public string Claude { get; set; }
    public string Grok { get; set; }
    public static void ExportSolutions(List<SolutionExport> solutions, string outputDir)
    {
        Directory.CreateDirectory(outputDir);

        foreach (var sol in solutions)
        {
            ExportCode(sol.TaskId, sol.EntryPoint, sol.ChatGpt, "ChatGptSolver", outputDir);
            ExportCode(sol.TaskId, sol.EntryPoint, sol.Gemini, "GeminiSolver", outputDir);
            ExportCode(sol.TaskId, sol.EntryPoint, sol.Claude, "ClaudeSolver", outputDir);
            ExportCode(sol.TaskId, sol.EntryPoint, sol.Grok, "GrokSolver", outputDir);
            // Optionally export Benchmark as well
        }
    }

    private static void ExportCode(string taskId, string entryPoint, string code, string className, string outputDir)
    {
        if (string.IsNullOrWhiteSpace(code)) return;

        var ns = $"Solutions.{taskId.Replace("/", "_")}";
        var fileName = Path.Combine(outputDir, $"{ns}.{className}.cs");

        var wrappedCode = $@"
    using System;
    namespace {ns}
    {{
        public class {className}
        {{
            // Entry point: {entryPoint}
    {IndentCode(code, 2)}
        }}
    }}
    ";
        File.WriteAllText(fileName, wrappedCode);
    }

    private static string IndentCode(string code, int indentLevel)
    {
        var indent = new string(' ', indentLevel * 4);
        var lines = code.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        return string.Join(Environment.NewLine, lines.Select(l => indent + l));
    }


    public static List<SolutionExport> ReadSolutions(string filePath)
    {
        var solutions = new List<SolutionExport>();
        using (var workbook = new XLWorkbook(filePath))
        {
            var ws = workbook.Worksheet("Tasks");
            var rows = ws.RowsUsed().Skip(1); // Skip header

            foreach (var row in rows)
            {
                solutions.Add(new SolutionExport
                {
                    TaskId = row.Cell("A").GetString(),
                    EntryPoint = row.Cell("F").GetString(),
                    Benchmark = row.Cell("J").GetString(),
                    Test = row.Cell("C").GetString(),
                    ChatGpt = row.Cell("K").GetString(),
                    Gemini = row.Cell("L").GetString(),
                    Grok = row.Cell("M").GetString(),
                    Claude = row.Cell("N").GetString()
                });
            }
        }
        return solutions;
    }

}



