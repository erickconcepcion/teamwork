using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSW.Sections
{
    interface ISection
    {
        string SectionName { get; }
        void Execute(IEnumerable<string> parameters);
    }
}
