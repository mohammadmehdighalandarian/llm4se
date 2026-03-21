using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace EvaluatedCodeGeneration
{
    // --- DATA MODELS ---

    public class CodeQualityReport
    {
        // Raw Metrics
        public bool Compiles { get; set; }
        public bool PassesTests { get; set; } // Boolean summary
        public TestInfo TestInfo { get; set; } = new TestInfo(); // Detailed results
        public List<string> ViolationDetails { get; set; } = new List<string>();

        // Static Analysis Metrics
        public int CyclomaticComplexity { get; set; }
        public int MaxNestingDepth { get; set; }
        public int NamingViolations { get; set; }
        public bool HasDocumentation { get; set; }
        public bool HasNullChecks { get; set; }

        // Performance
        public double AverageExecutionTimeNs { get; set; }

        // Normalized Scores (0.0 to 1.0)
        public double Norm_Correctness { get; set; }
        public double Norm_Complexity { get; set; }
        public double Norm_Nesting { get; set; }
        public double Norm_Style { get; set; }
        public double Norm_Performance { get; set; }

        // Final Equal-Weighted Score (0 to 100)
        public double FinalScore { get; set; }
    }

    public class TestInfo
    {
        public bool AllTestPasses { get; set; }
        public int PassTestCount { get; set; }
        public int FailTestCount { get; set; }
        public double SuccessRate { get; set; }
        public double FailRate { get; set; }
    }

    public class TaskSpecification
    {
        public string MethodName { get; set; }
        public List<TestCase> TestCases { get; set; } = new List<TestCase>();
    }

    public class TestCase
    {
        public object[] Inputs { get; set; }
        public object Expected { get; set; }
    }

    // --- EVALUATOR ENGINE ---

    public class CodeEvaluator
    {
        private const int PerformanceIterations = 10000;
        private const int ExecutionTimeoutMs = 5000; // 5 Second Timeout per test

        public CodeQualityReport Evaluate(string aiCode, TaskSpecification spec)
        {
            var report = new CodeQualityReport();

            // --- STEP 1: COMPILATION ---
            var compilation = CompileCode(aiCode);
            using var ms = new MemoryStream();
            var emitResult = compilation.Emit(ms);
            report.Compiles = emitResult.Success;

            if (!report.Compiles)
            {
                foreach (var diag in emitResult.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error))
                {
                    report.ViolationDetails.Add($"Compile Error: {diag.GetMessage()}");
                }
                report.FinalScore = 0;
                return report;
            }

            // Load Assembly
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            // Allow flexibility in class naming (Program, Solution, or Solver)
            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == "Program" || t.Name == "Solution" || t.Name == "Solver");

            var methodInfo = type?.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                                 .FirstOrDefault(m => m.Name == spec.MethodName);

            if (methodInfo == null)
            {
                report.ViolationDetails.Add($"Logic Error: Method '{spec.MethodName}' not found in the compiled assembly.");
                report.FinalScore = 0;
                return report;
            }

            // --- STEP 2: FUNCTIONAL CORRECTNESS (With Timeout) ---
            report.TestInfo = RunFunctionalTests(methodInfo, type, spec, report);
            report.PassesTests = report.TestInfo.AllTestPasses;

            // --- STEP 3: STATIC ANALYSIS ---
            var tree = CSharpSyntaxTree.ParseText(aiCode);
            report.CyclomaticComplexity = CalculateCyclomaticComplexity(tree);
            report.MaxNestingDepth = CalculateMaxNesting(tree);
            AnalyzeNamingConventions(tree, report, spec.MethodName);
            AnalyzeProfessionalism(tree, report);

            // --- STEP 4: PERFORMANCE (Only if correct) ---
            if (report.PassesTests && spec.TestCases.Count > 0)
            {
                // We use the first test case for benchmarking to ensure validity
                report.AverageExecutionTimeNs = MeasurePerformance(methodInfo, type, spec.TestCases[0].Inputs);
            }

            // --- STEP 5: SCORING ---
            report.FinalScore = CalculateScore(report);
            return report;
        }

        private TestInfo RunFunctionalTests(MethodInfo method, Type type, TaskSpecification spec, CodeQualityReport report)
        {
            var testDetail = new TestInfo { AllTestPasses = true };
            int totalTests = spec.TestCases.Count;

            if (totalTests == 0) return testDetail;

            // Create instance if method is not static
            object instance = method.IsStatic ? null : Activator.CreateInstance(type);

            foreach (var test in spec.TestCases)
            {
                try
                {
                    // SAFETY: Run in a Task with a timeout to prevent Infinite Loops
                    var task = Task.Run(() => method.Invoke(instance, test.Inputs));

                    if (task.Wait(TimeSpan.FromMilliseconds(ExecutionTimeoutMs)))
                    {
                        var actual = task.Result;

                        // Use JSON Serialization for deep comparison of Lists/Arrays
                        if (!ObjectsAreEqual(actual, test.Expected))
                        {
                            testDetail.FailTestCount++;
                            testDetail.AllTestPasses = false;
                            report.ViolationDetails.Add($"Fail: Input [{string.Join(", ", test.Inputs)}] -> Got {JsonSerializer.Serialize(actual)}, Exp {JsonSerializer.Serialize(test.Expected)}");
                        }
                        else
                        {
                            testDetail.PassTestCount++;
                        }
                    }
                    else
                    {
                        testDetail.FailTestCount++;
                        testDetail.AllTestPasses = false;
                        report.ViolationDetails.Add($"Timeout: Test took longer than {ExecutionTimeoutMs}ms (Infinite Loop?)");
                    }
                }
                catch (Exception ex)
                {
                    testDetail.FailTestCount++;
                    testDetail.AllTestPasses = false;
                    var msg = ex.InnerException?.Message ?? ex.Message;
                    report.ViolationDetails.Add($"Runtime Error: {msg}");
                }
            }

            testDetail.SuccessRate = totalTests == 0 ? 0 : (double)testDetail.PassTestCount / totalTests * 100;
            return testDetail;
        }

        private double MeasurePerformance(MethodInfo method, Type type, object[] inputs)
        {
            object instance = method.IsStatic ? null : Activator.CreateInstance(type);

            // 1. Warm-up (JIT Compile)
            for (int i = 0; i < 50; i++) method.Invoke(instance, inputs);

            // 2. Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < PerformanceIterations; i++)
            {
                method.Invoke(instance, inputs);
            }
            sw.Stop();

            double totalNs = (double)sw.ElapsedTicks / Stopwatch.Frequency * 1_000_000_000;
            return totalNs / PerformanceIterations;
        }

        private bool ObjectsAreEqual(object actual, object expected)
        {
            if (actual == null && expected == null) return true;
            if (actual == null || expected == null) return false;

            try
            {
                // Serializing to JSON handles arrays and lists correctly (unlike .ToString())
                return JsonSerializer.Serialize(actual) == JsonSerializer.Serialize(expected);
            }
            catch
            {
                return actual.Equals(expected);
            }
        }

        private CSharpCompilation CompileCode(string code)
        {
            // Standard references required for compilation
            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Text.RegularExpressions.Regex).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
                MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location)
            };

            return CSharpCompilation.Create("DynamicAiAssembly_" + Guid.NewGuid())
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(references)
                .AddSyntaxTrees(CSharpSyntaxTree.ParseText(code));
        }

        // --- STATIC ANALYSIS HELPERS ---

        private int CalculateCyclomaticComplexity(SyntaxTree tree)
        {
            var root = tree.GetRoot();
            int decisionPoints = root.DescendantTokens().Count(t =>
                t.IsKind(SyntaxKind.IfKeyword) ||
                t.IsKind(SyntaxKind.WhileKeyword) ||
                t.IsKind(SyntaxKind.ForKeyword) ||
                t.IsKind(SyntaxKind.ForEachKeyword) ||
                t.IsKind(SyntaxKind.CaseKeyword) ||
                t.IsKind(SyntaxKind.CatchKeyword) ||
                t.IsKind(SyntaxKind.QuestionQuestionToken) ||
                t.IsKind(SyntaxKind.AmpersandAmpersandToken) ||
                t.IsKind(SyntaxKind.BarBarToken)
            );
            return decisionPoints + 1;
        }

        private int CalculateMaxNesting(SyntaxTree tree)
        {
            int maxDepth = 0;
            foreach (var method in tree.GetRoot().DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                maxDepth = Math.Max(maxDepth, GetNestingDepth(method.Body));
            }
            return maxDepth;
        }

        private int GetNestingDepth(SyntaxNode node, int depth = 0)
        {
            if (node == null) return depth;
            int max = depth;

            foreach (var child in node.ChildNodes())
            {
                int nextDepth = depth;

                // FIX: Check for concrete loop types instead of the abstract 'IterationStatementSyntax'
                if (child is IfStatementSyntax ||
                    child is ForStatementSyntax ||
                    child is ForEachStatementSyntax ||
                    child is WhileStatementSyntax ||
                    child is DoStatementSyntax)
                {
                    nextDepth++;
                }

                max = Math.Max(max, GetNestingDepth(child, nextDepth));
            }
            return max;
        }

        private void AnalyzeNamingConventions(SyntaxTree tree, CodeQualityReport report, string expectedMethodName)
        {
            var root = tree.GetRoot();

            // Check Method PascalCase
            var method = root.DescendantNodes().OfType<MethodDeclarationSyntax>()
                             .FirstOrDefault(m => m.Identifier.Text == expectedMethodName);

            if (method != null && char.IsLower(method.Identifier.Text[0]))
                report.NamingViolations++;

            // Check Variables/Parameters camelCase
            var identifiers = root.DescendantNodes().OfType<ParameterSyntax>().Select(p => p.Identifier.Text)
                .Concat(root.DescendantNodes().OfType<VariableDeclaratorSyntax>().Select(v => v.Identifier.Text));

            foreach (var name in identifiers)
            {
                if (!string.IsNullOrEmpty(name) && char.IsUpper(name[0]))
                    report.NamingViolations++;
            }
        }

        private void AnalyzeProfessionalism(SyntaxTree tree, CodeQualityReport report)
        {
            var code = tree.ToString();
            report.HasNullChecks = code.Contains("== null") || code.Contains("is null");
            report.HasDocumentation = tree.GetRoot().DescendantTrivia()
                .Any(t => t.IsKind(SyntaxKind.SingleLineDocumentationCommentTrivia));
        }

        private double CalculateScore(CodeQualityReport r)
        {
            if (!r.Compiles) return 0;

            // --- 1. Normalize Functional Correctness (0.0 to 1.0) ---
            // SuccessRate is 0-100, convert to 0-1
            r.Norm_Correctness = r.TestInfo.SuccessRate / 100.0;

            // --- 2. Normalize Complexity (Inverse) ---
            // Threshold: 15. If complexity is > 15, score drops.
            const double MaxAcceptableComplexity = 15.0;
            r.Norm_Complexity = Math.Clamp(1.0 - (r.CyclomaticComplexity / MaxAcceptableComplexity), 0.0, 1.0);

            // --- 3. Normalize Nesting (Inverse) ---
            // Threshold: 5 levels deep.
            const double MaxAcceptableNesting = 5.0;
            r.Norm_Nesting = Math.Clamp(1.0 - (r.MaxNestingDepth / MaxAcceptableNesting), 0.0, 1.0);

            // --- 4. Normalize Style/Naming (Inverse) ---
            // Threshold: 10 bad names.
            const double MaxAcceptableViolations = 10.0;
            r.Norm_Style = Math.Clamp(1.0 - (r.NamingViolations / MaxAcceptableViolations), 0.0, 1.0);

            // --- 5. Normalize Performance (Relative) ---
            // Threshold: 10,000ns. Score decays as time increases.
            const double PerformanceBenchmarkNs = 10000.0;
            if (r.AverageExecutionTimeNs <= 0)
            {
                // If tests failed or code didn't run, perf is 0
                r.Norm_Performance = 0;
            }
            else
            {
                r.Norm_Performance = Math.Clamp(PerformanceBenchmarkNs / (PerformanceBenchmarkNs + r.AverageExecutionTimeNs), 0.0, 1.0);
            }

            // --- FINAL CALCULATION: EQUAL WEIGHT AVERAGE ---
            double sum = r.Norm_Correctness +
                         r.Norm_Complexity +
                         r.Norm_Nesting +
                         r.Norm_Style +
                         r.Norm_Performance;

            // Average of 5 factors, scaled to 100
            return (sum / 5.0) * 100.0;
        }
    }
}