using System;
using System.Collections.Generic;
using System.Threading;
using KellermanSoftware.CompareNetObjects;

namespace Tests.Shared
{
    public static class TestUtils
    {
        private static ThreadLocal<Score> _currentScore
            = new ThreadLocal<Score>(() => new Score());

        public class Score
        {
            public int Passed { get; set; } = 0;
            public int Failed { get; set; } = 0;
            public List<string> FailureMessages { get; set; } = new List<string>();
        }

        /// <summary>
        /// Resets the score for the current thread/AI model.
        /// </summary>
        public static void Reset()
        {
            _currentScore.Value = new Score();
        }

        /// <summary>
        /// Gets the final score for the current run.
        /// </summary>
        public static Score GetScore()
        {
            return _currentScore.Value;
        }

        /// <summary>
        /// Used for normal logical assertions (CompareNetObjects).
        /// </summary>
        public static void Check(ComparisonResult result, string errorMessage)
        {
            if (result.AreEqual)
            {
                _currentScore.Value.Passed++;
            }
            else
            {
                _currentScore.Value.Failed++;
                // Use DifferencesString to get the specific mismatch details
                string diff = result.DifferencesString.Trim();
                _currentScore.Value.FailureMessages.Add($"{errorMessage}. Details: {diff}");
            }
        }

        /// <summary>
        /// Used when the test throws an Exception (Runtime Error).
        /// This fixes the "ComparisonResult constructor" error.
        /// </summary>
        public static void RecordCrash(string errorMessage)
        {
            _currentScore.Value.Failed++;
            _currentScore.Value.FailureMessages.Add($"CRITICAL EXCEPTION: {errorMessage}");
        }
    }
}