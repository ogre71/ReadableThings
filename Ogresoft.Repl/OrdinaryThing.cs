using System;
using System.Collections.Generic;
using System.Text;

namespace Ogresoft
{
    public class OrdinaryThing : Thing
    {
        public OrdinaryThing(string name) : base(name)
        {
            this.AddDirectObjectHandler("take", 
                (doer, directObject) =>
                {
                    return true;
                });
        }
    }
}
