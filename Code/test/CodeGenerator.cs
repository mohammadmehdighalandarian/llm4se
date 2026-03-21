using System;
using System.Text;

namespace test;

public class ProblemData
{
    public string TaskId { get; set; }      // e.g., "csharp/1"
    public string EntryPoint { get; set; }  // e.g., "SumProduct"
    public string Benchmark { get; set; }   // The BenchmarkRunner code
    public string ChatGpt { get; set; }     // AI Answer 1
    public string Gemini { get; set; }      // AI Answer 2
    public string Claude { get; set; }      // AI Answer 3
    public string Grok { get; set; }        // AI Answer 4
}
public class CodeGenerator
{
    public string GenerateSourceFile(SolutionExport data)
    {
        // 1. Sanitize the TaskId to make it a valid Namespace
        // "csharp/1" -> "csharp_1"
        string safeTaskId = data.TaskId
            .Replace("/", "_")
            .Replace("-", "_")
            .Replace(" ", "");

        StringBuilder sb = new StringBuilder();

        // 2. Add Global Imports (Optional but helpful)
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("using System.Text;");
        sb.AppendLine("using BenchmarkDotNet.Attributes;");
        sb.AppendLine();

        // 3. Wrap ChatGpt Code
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.ChatGpt");
        sb.AppendLine("{");
        sb.AppendLine(CleanAiOutput(data.ChatGpt));
        sb.AppendLine("}");
        sb.AppendLine();

        // 4. Wrap Gemini Code
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.Gemini");
        sb.AppendLine("{");
        sb.AppendLine(CleanAiOutput(data.Gemini));
        sb.AppendLine("}");
        sb.AppendLine();

        // 5. Wrap Claude Code
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.Claude");
        sb.AppendLine("{");
        sb.AppendLine(CleanAiOutput(data.Claude));
        sb.AppendLine("}");
        sb.AppendLine();

        // 6. Wrap Grok Code
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.Grok");
        sb.AppendLine("{");
        sb.AppendLine(CleanAiOutput(data.Grok));
        sb.AppendLine("}");
        sb.AppendLine();

        // 7. Add the Benchmark Runner (The orchestrator)
        // We put this in the parent namespace for the task
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}");
        sb.AppendLine("{");
        sb.AppendLine(CleanAiOutput(data.Benchmark));
        sb.AppendLine("}");

        return sb.ToString();
    }

    /// <summary>
    /// Removes Markdown code blocks (```csharp) if the AI included them.
    /// </summary>
    private string CleanAiOutput(string rawCode)
    {
        if (string.IsNullOrEmpty(rawCode)) return string.Empty;

        return rawCode
            .Replace("```csharp", "") // Remove start of markdown
            .Replace("```", "")       // Remove end of markdown
            .Trim();                  // Remove extra whitespace
    }
}
