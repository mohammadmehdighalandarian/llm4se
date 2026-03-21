using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Model
{
    public class AiScore
    {
        public int Passed { get; set; }
        public int Failed { get; set; }
        public int Total => Passed + Failed;
        public double SuccessRate => Total == 0 ? 0 : (double)Passed / Total * 100;
    }
}
