using System;
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
            Assert.IsTrue(parser.AdminThing.LastMessage == "You look around.");
        }
    }
}
