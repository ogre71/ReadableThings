using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogresoft.Parser
{
    public class RoomThing : Thing
    {
        public RoomThing() : base("non-descript room") { }

        public RoomThing(string name) : base(name) { }

    }
}
