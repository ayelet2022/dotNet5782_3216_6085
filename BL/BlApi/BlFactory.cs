using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BlApi;

namespace BlApi
{
    static public class BlFactory
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IBL GetBl() => BL.BL.Instance;
    }
}
