using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ogresoft;

namespace Test.Ogresoft.ReadableThings
{
    [TestClass]
    public class TestMessages
    {
        [TestMethod]
        public void TestSomeMessaging()
        {
            TestThing thisThing = new TestThing("nothing");
            Messages.Action("{o0} {v0look}.", thisThing);
            Assert.IsTrue(thisThing.LastMessage == "You look.");
            Messages.Action("{o0} {v0dance}.", thisThing);
            Assert.IsTrue(thisThing.LastMessage == "You dance.");
            Messages.Action("{o0} {v0ponder}.", thisThing);
            Assert.IsTrue(thisThing.LastMessage == "You ponder.");
            Messages.Action("{o0} {V0look}.", thisThing);
            Assert.IsTrue(thisThing.LastMessage == "You Look.");
            Messages.Action("{o0} {V0dance}.", thisThing);
            Assert.IsTrue(thisThing.LastMessage == "You Dance.");
            Messages.Action("{o0} {V0ponder}.", thisThing);
            Assert.IsTrue(thisThing.LastMessage == "You Ponder.");
        }

        //[TestMethod]
        //public void TestSecondPerson()
        //{
        //    TestThing roomThing = new TestThing("a non-descript room");
        //    TestThing thisThing = new TestThing("nothing");
        //    thisThing.Move(roomThing);
        //    TestThing otherThing = new TestThing("second nothing");
        //    otherThing.IsObserver = true;
        //    otherThing.Move(roomThing);
        //    Messages.Action("{o0} {v0look}.", thisThing);
        //    Assert.IsTrue(thisThing.LastMessage == "You look.");
        //    Assert.IsTrue(otherThing.LastMessage == "Nothing looks.");
        //    Messages.Action("{o0} {v0dance}.", thisThing);
        //    Assert.IsTrue(thisThing.LastMessage == "You dance.");
        //    Assert.IsTrue(otherThing.LastMessage == "Nothing dances.");
        //    Messages.Action("{o0} {v0ponder}.", thisThing);
        //    Assert.IsTrue(thisThing.LastMessage == "You ponder.");
        //    Assert.IsTrue(otherThing.LastMessage == "Nothing ponders.");
        //    Messages.Action("{o0} {V0look}.", thisThing);
        //    Assert.IsTrue(thisThing.LastMessage == "You Look.");
        //    Assert.IsTrue(otherThing.LastMessage == "Nothing Looks.");
        //    Messages.Action("{o0} {V0dance}.", thisThing);
        //    Assert.IsTrue(thisThing.LastMessage == "You Dance.");
        //    Assert.IsTrue(otherThing.LastMessage == "Nothing Dances.");
        //    Messages.Action("{o0} {V0ponder}.", thisThing);
        //    Assert.IsTrue(thisThing.LastMessage == "You Ponder.");
        //    Assert.IsTrue(otherThing.LastMessage == "Nothing Ponders.");
        //}
    }
}
