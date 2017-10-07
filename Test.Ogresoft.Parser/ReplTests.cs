using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ogresoft.Parser;

namespace Test.Ogresoft.Parser
{
    [TestClass]
    public class ReplTests
    {
        [TestMethod]
        public void ReplShouldAutoCompleteVerbs()
        {
            var parser = new Repl();
            parser.Execute("l");
            var lastMessage = parser.AdminThing.LastMessage;

            parser.Execute("look");
            Assert.IsTrue(parser.AdminThing.LastMessage == lastMessage); 
        }

        [TestMethod]
        public void ReplShouldHandleGarbage()
        {
            var repl = new Repl();
            string garbageString = "thisisgarbage"; 
            repl.Execute(garbageString);
            var lastMessage = repl.AdminThing.LastMessage;

            Assert.IsTrue(repl.AdminThing.LastMessage == string.Format(Repl.Garbage, garbageString));
        }

        [TestMethod]
        public void ShouldSerialize()
        {
            var repl = new Repl();
            var serialized = repl.Serialize();
            System.Console.WriteLine(serialized); 
            Assert.IsTrue(serialized != null); 
        }
    }
}
