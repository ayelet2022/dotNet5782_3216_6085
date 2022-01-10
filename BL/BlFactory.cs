using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;

namespace BL
{
    static public class BlFactory
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        static public BL GetBl() { return BL.Instance; }
    }
}
