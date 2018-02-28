using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore
{
    internal interface IController
    {
        void Run();
        void KillMe();
        Action OnComponentKilled { get; set; }
    }
}
