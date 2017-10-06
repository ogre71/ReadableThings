using System;
using System.Collections.Generic;

namespace Ogresoft.Parser
{
    public class Exit : Thing
    {
        public Exit(string name, Thing destination) : this(name, () => destination)
        { }

        public Exit(string name, Func<Thing> getDestination) : base(name)
        {
            this.GetDestination = getDestination;

            this.Preposition = "through"; 

            this._verbHash.Add("go", new Dictionary<Type, Delegate>());
            this._verbHash["go"].Add(typeof(AllowUsageAsDirObjDelegate), new AllowUsageAsDirObjDelegate((goer, myself) => { return true; }));
        }

        public Func<Thing> GetDestination { get; private set; }

        public string Preposition { get; }
    }
}
