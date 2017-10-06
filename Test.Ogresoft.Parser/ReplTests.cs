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
    }
}
