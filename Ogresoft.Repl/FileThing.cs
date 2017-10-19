using System;
using System.Collections.Generic;

namespace Ogresoft.Parser
{
    public class FileThing : Thing
    {
        public FileThing(string fileName, string directory) : base(fileName)
        {
            this.Directory = directory;

            this._verbHash.Add("take", new Dictionary<Type, Delegate>());
            this._verbHash["take"].Add(typeof(AllowUsageAsDirObjDelegate), new AllowUsageAsDirObjDelegate((taker, myself) => { return true; }));
        }

        public string Directory { get; private set; }
    }
}
