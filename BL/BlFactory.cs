using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    static public class BlFactory
    {
        static public BL GetBl() { return BL.Instance; }
    }
}
