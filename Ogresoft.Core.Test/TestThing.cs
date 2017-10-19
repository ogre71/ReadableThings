using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ogresoft; 

namespace Test.Ogresoft.ReadableThings
{
    public class TestThing : Thing
    {
        public TestThing(string description) : base(description)
        { }

        public override void Tell(string message)
        {
            base.Tell(message);

            this.LastMessage = message;
            Console.WriteLine(this.Name + " : " + message); 
        }

        public string LastMessage { get; set; }
    }
}
