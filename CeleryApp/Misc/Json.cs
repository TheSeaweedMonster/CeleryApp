using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeleryApp
{
    class Json
    {
        public class SaveArgs
        {
            public string Name { get; set; }
            public int Value { get; set; }
            public bool Bool { get; set; }
        }

        public class MakeList { public List<SaveArgs> UserInterfaceSettings { get; set; } = new List<SaveArgs>(); }
    }
}
