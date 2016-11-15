using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amazon.Scheduling
{
    public interface IException
    {
        bool IsTransient { get; }
    }
}
