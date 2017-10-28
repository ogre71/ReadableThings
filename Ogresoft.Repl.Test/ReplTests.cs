using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ogresoft.Parser;

namespace Ogresoft.Parser.Test
{
    [TestClass]
    public class ReplTests
    {
        [TestMethod]
        public void ReplShouldAutoCompleteVerbs()
        {
            var parser = new Parser();
            parser.Execute("l");
            var lastMessage = parser.AdminThing.LastMessage;

            parser.Execute("look");
            Assert.IsTrue(parser.AdminThing.LastMessage == lastMessage); 
        }

        [TestMethod]
        public void ReplShouldHandleGarbage()
        {
            var repl = new Parser();
            string garbageString = "thisisgarbage"; 
            repl.Execute(garbageString);
            var lastMessage = repl.AdminThing.LastMessage;

            Assert.IsTrue(repl.AdminThing.LastMessage == string.Format(Parser.Garbage, garbageString));
        }

        [TestMethod]
        public void ShouldSerialize()
        {
            var repl = new Parser();
            var serialized = repl.Serialize();
            System.Console.WriteLine(serialized); 
            Assert.IsTrue(serialized != null); 
        }
    }
}
