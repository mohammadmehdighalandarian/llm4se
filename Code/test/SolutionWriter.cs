using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public class SolutionWriter
{
    public void CreateFiles(SolutionExport data)
    {
        string safeTaskId = data.TaskId.Replace("/", "_").Replace("-", "_").Replace(" ", "");

        // This calculates the real source folder, not the bin folder
        string projectRoot = GetProjectRoot();

        string baseSolutionFolder = Path.Combine(projectRoot, "Solution");

        string solversFolder = Path.Combine(baseSolutionFolder, "Solvers");
        string benchmarksFolder = Path.Combine(baseSolutionFolder, "Benchmarks");
        string testsFolder = Path.Combine(baseSolutionFolder, "Tests");

        if (!Directory.Exists(solversFolder)) Directory.CreateDirectory(solversFolder);
        if (!Directory.Exists(benchmarksFolder)) Directory.CreateDirectory(benchmarksFolder);
        if (!Directory.Exists(testsFolder)) Directory.CreateDirectory(testsFolder);

        Console.WriteLine($"--- Processing Task: {safeTaskId} ---");

        CreateSolversFile(data, safeTaskId, solversFolder);
        CreateBenchmarkFile(data, safeTaskId, benchmarksFolder);
        CreateTestFile(data, safeTaskId, testsFolder);
    }

    // ---------------------------------------------------------
    // 1. SOLVERS GENERATION
    // ---------------------------------------------------------
    private void CreateSolversFile(SolutionExport data, string safeTaskId, string folder)
    {
        string fileName = $"Solvers_Task_{safeTaskId}.cs";
        string fullPath = Path.Combine(folder, fileName);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"// Auto-generated Solvers for Task: {safeTaskId}");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("using System.Text.RegularExpressions;");
        sb.AppendLine("using System.Text;");
        sb.AppendLine();

        // GPT
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.GPT");
        sb.AppendLine("{");
        sb.AppendLine(CleanCode(data.ChatGpt));
        sb.AppendLine("}");
        sb.AppendLine();

        // Gemini
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.Gemini");
        sb.AppendLine("{");
        sb.AppendLine(CleanCode(data.Gemini));
        sb.AppendLine("}");
        sb.AppendLine();

        // Claude
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.Claude");
        sb.AppendLine("{");
        sb.AppendLine(CleanCode(data.Claude));
        sb.AppendLine("}");
        sb.AppendLine();

        // Grok
        sb.AppendLine($"namespace Benchmarks.{safeTaskId}.Grok");
        sb.AppendLine("{");
        sb.AppendLine(CleanCode(data.Grok));
        sb.AppendLine("}");

        File.WriteAllText(fullPath, sb.ToString());
        Console.WriteLine($"   [Created] Solver: {fileName}");
    }

    // ---------------------------------------------------------
    // 2. BENCHMARK GENERATION
    // ---------------------------------------------------------
    private void CreateBenchmarkFile(SolutionExport data, string safeTaskId, string folder)
    {
        string fileName = $"Benchmark_Task_{safeTaskId}.cs";
        string fullPath = Path.Combine(folder, fileName);

        string content = CleanCode(data.Benchmark);

        if (!content.Contains("using BenchmarkDotNet"))
        {
            content = "using BenchmarkDotNet.Attributes;\n" + content;
        }

        // Standardize naming
        content = content.Replace(".ChatGpt.Solver", ".GPT.Solver");

        File.WriteAllText(fullPath, content);
        Console.WriteLine($"   [Created] Benchmark: {fileName}");
    }

    // ---------------------------------------------------------
    // 3. TEST GENERATION
    // ---------------------------------------------------------
    private void CreateTestFile(SolutionExport data, string safeTaskId, string folder)
    {
        string fileName = $"Tests_Task_{safeTaskId}.cs";
        string fullPath = Path.Combine(folder, fileName);
        string entryPoint = data.EntryPoint;

        // 1. Extract the raw body
        string rawBody = ExtractMethodBodyRobust(data.Test);

        // 2. CONVERT TO NON-BREAKING LOGIC
        string instrumentedBody = InstrumentTestCode(rawBody);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"// Auto-generated Tests for Task: {safeTaskId}");
        sb.AppendLine("using System;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using System.Linq;");
        sb.AppendLine("using KellermanSoftware.CompareNetObjects;");
        sb.AppendLine("using Tests.Shared;"); // Import our new helper
        sb.AppendLine();

        sb.AppendLine($"namespace Tests.{safeTaskId}");
        sb.AppendLine("{");
        sb.AppendLine("    public class TestRunner");
        sb.AppendLine("    {");

        sb.AppendLine(CreateTestMethod("Run_GPT", instrumentedBody, entryPoint, $"Benchmarks.{safeTaskId}.GPT.Solver"));
        sb.AppendLine(CreateTestMethod("Run_Gemini", instrumentedBody, entryPoint, $"Benchmarks.{safeTaskId}.Gemini.Solver"));
        sb.AppendLine(CreateTestMethod("Run_Claude", instrumentedBody, entryPoint, $"Benchmarks.{safeTaskId}.Claude.Solver"));
        sb.AppendLine(CreateTestMethod("Run_Grok", instrumentedBody, entryPoint, $"Benchmarks.{safeTaskId}.Grok.Solver"));

        sb.AppendLine("    }");
        sb.AppendLine("}");

        File.WriteAllText(fullPath, sb.ToString());
        Console.WriteLine($"   [Created] Test: {fileName}");
    }

    // --- HELPER METHODS ---

    private string InstrumentTestCode(string code)
    {
        // Replaces: if (!x.AreEqual) { throw... } 
        // With: TestUtils.Check(x, "...");
        string pattern = @"if\s*\(\!([a-zA-Z0-9_]+)\.AreEqual\)\s*\{\s*throw new Exception\(\""(.*?)\""\);\s*\}";
        string replacement = "TestUtils.Check($1, \"$2\");";
        return Regex.Replace(code, pattern, replacement);
    }

    private string CreateTestMethod(string methodName, string body, string entryPoint, string namespacePrefix)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"        public void {methodName}()");
        sb.AppendLine("        {");
        sb.AppendLine("            TestUtils.Reset();");
        sb.AppendLine("            try {");
        sb.AppendLine(InjectNamespace(body, entryPoint, namespacePrefix));
        sb.AppendLine("            } catch (Exception ex) { Console.WriteLine(\"CRITICAL FAIL: \" + ex.Message); }");
        sb.AppendLine("        }");
        return sb.ToString();
    }

    // Robust extractor using brace counting
    private string ExtractMethodBodyRobust(string rawCode)
    {
        if (string.IsNullOrEmpty(rawCode)) return "// No test code found";

        var mainMatch = Regex.Match(rawCode, @"Main\s*\(.*?\)");
        if (!mainMatch.Success) return rawCode;

        int startIndex = rawCode.IndexOf('{', mainMatch.Index);
        if (startIndex == -1) return rawCode;

        int openBraces = 0;
        int endIndex = -1;

        for (int i = startIndex; i < rawCode.Length; i++)
        {
            if (rawCode[i] == '{') openBraces++;
            if (rawCode[i] == '}') openBraces--;

            if (openBraces == 0)
            {
                endIndex = i;
                break;
            }
        }

        if (endIndex == -1) return "// Error parsing test body";

        return rawCode.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
    }

    private string InjectNamespace(string codeBody, string entryPoint, string fullNamespace)
    {
        string pattern = $@"\b{entryPoint}\s*\(";
        string replacement = $"{fullNamespace}.{entryPoint}(";
        return Regex.Replace(codeBody, pattern, replacement);
    }

    private string CleanCode(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return "";
        return raw.Replace("```csharp", "").Replace("```", "").Trim();
    }

    // --- CRITICAL FIX: FINDING THE REAL PROJECT ROOT ---
    private string GetProjectRoot()
    {
        // Start where the .exe runs (usually bin/Debug/net8.0)
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        DirectoryInfo dir = new DirectoryInfo(currentDirectory);

        // Keep going up ONE folder at a time until we find a .csproj file
        while (dir != null && dir.GetFiles("*.csproj").Length == 0)
        {
            dir = dir.Parent;
        }

        // Return the folder containing the .csproj, or current dir if failed
        return dir != null ? dir.FullName : currentDirectory;
    }
}