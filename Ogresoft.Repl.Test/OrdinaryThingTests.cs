using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Ogresoft.Parser;

namespace Ogresoft.Parser.Test
{
    public class OrdinaryThingTests
    {
        [TestMethod]
        public void DefaultThingShouldBeADirectObjectOfTake()
        {
            var adminThing = new AdminThing("some weirdo");
            var roomThing = new RoomThing();
            adminThing.Move(roomThing);

            var uglyDeadFish = new OrdinaryThing("ugly dead fish");
            uglyDeadFish.Move(roomThing);

            Parser repl = new Parser();

            var lastMessage = adminThing.LastMessage;
            repl.Parse("take ugly dead fish", adminThing);

            Assert.IsTrue(adminThing.LastMessage != lastMessage);

            Assert.IsTrue(adminThing.ShallowInventory.Contains(uglyDeadFish));
        }
    }
}
