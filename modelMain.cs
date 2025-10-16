using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfSimpleCal.modelMain
{
    public class modelMain
    {

        public int ProcessId { get; }
        public string ProgramName { get; }
        public DateTime AddedTime { get; }

        public modelMain(string name, int id, DateTime job)
        {
            ProgramName = name;
            ProcessId = id;
            AddedTime = job;
        }
    }
}
