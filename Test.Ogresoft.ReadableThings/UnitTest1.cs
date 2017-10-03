using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ogresoft;

namespace Test.Ogresoft.ReadableThings
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCompleteVerbShouldFindWholeVerb()
        {
            Thing thisThing = new Thing("nothing");
            Func<bool> lookAction = new Func<bool>(() => { return true; });
            thisThing.AddVerb("look", new Thing.AllowUseAloneDelegate(lookAction));

            Assert.IsTrue(thisThing.AllowUseAlone("look"));
        }

        [TestMethod]
        public void TestCompleteVerbShouldFindPartialVerb()
        {
            Thing thisThing = new Thing("nothing");
            Func<bool> lookAction = new Func<bool>(() => {
                Messages.Action("{D0 v0look}", thisThing); 
                return true;
            });
            thisThing.AddVerb("l", new Thing.AllowUseAloneDelegate(lookAction));

            Assert.IsTrue(thisThing.AllowUseAlone("l"));
        }


    }
}
