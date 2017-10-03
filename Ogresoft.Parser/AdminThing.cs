using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogresoft.Parser
{
    public class AdminThing : Thing
    {
        public AdminThing(string name) : base(name)
        {
            this.AddVerb("look", new Func<bool>(() => {
                Messages.Action("{N0} {v0look} around.");
                return true;
            }));
        }
    }
}
