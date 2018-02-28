using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLogger
{
    public enum DashBoardState
    {
        Expand = 0,
        Collapsed = 1
    }
    public enum TaskState
    {
        NotStarted = 0,
        Processing = 1,
        Suspend = 2,
        Completed = 3
    }
}
