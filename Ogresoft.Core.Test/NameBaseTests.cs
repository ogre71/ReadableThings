using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ogresoft.Core.Test
{
    [TestClass]
    public class NameBaseTests
    {
        [TestMethod]
        public void ShouldMatchItself()
        {
            NameBase nameBase = new NameBase("Bob");
            Assert.IsTrue(nameBase.Matches("Bob")); 
        }

        [TestMethod]
        public void ShouldMatchDefiniteSelf()
        {
            NameBase nameBase = new NameBase("Bob");
            Assert.IsTrue(nameBase.Matches("the Bob"));
        }

        [TestMethod]
        public void ShouldNotMatchLonger()
        {
            NameBase nameBase = new NameBase("Bob");
            Assert.IsFalse(nameBase.Matches("Bobs"));
        }

        [TestMethod]
        public void ShouldMatchShorter()
        {
            NameBase nameBase = new NameBase("Bob");
            Assert.IsTrue(nameBase.Matches("Bo"));
            Assert.IsTrue(nameBase.Matches("B"));
        }

        [TestMethod]
        public void NameBaseShouldMatchLowerCase()
        {
            NameBase nameBase = new NameBase("Bob");
            Assert.IsTrue(nameBase.Matches("bo"));
        }

        [TestMethod]
        public void NameBaseShouldMatchOddCasing()
        {
            NameBase nameBase = new NameBase("Bob");
            Assert.IsTrue(nameBase.Matches("bOB"));
        }

        [TestMethod]
        public void NameBaseShouldMatchExclaimation()
        {
            NameBase nameBase = new NameBase("Bob");
            Assert.IsTrue(nameBase.Matches("bob!"));
        }

        [TestMethod]
        public void NameBaseShouldMatchWithUnusedPostAdjectives()
        {
            NameBase nameBase = new NameBase("Bob", "the fourth");
            Assert.IsTrue(nameBase.Matches("Bo"));
        }

        [TestMethod]
        public void NameBaseShouldMatchWithUsedPostAdjectives()
        {
            NameBase nameBase = new NameBase("Bob", "the fourth");
            Assert.IsTrue(nameBase.Matches("Bob the fourth"));
            Assert.IsTrue(nameBase.Matches("Bob fourth"));
            Assert.IsTrue(nameBase.Matches("bob fOuRth"));
            Assert.IsTrue(nameBase.Matches("bob fOuR"));
        }

        [TestMethod]
        public void NameBaseShouldNotMatchWithIncorrectPostAdjectives()
        {
            NameBase nameBase = new NameBase("Bob", "the fourth");
            Assert.IsFalse(nameBase.Matches("Bob the fourth minion"));
            Assert.IsFalse(nameBase.Matches("Bob fourthh"));
            Assert.IsFalse(nameBase.Matches("bob minion fOuRth"));
            Assert.IsFalse(nameBase.Matches("bob minion the fOuR"));
        }

        [TestMethod]
        public void NameBaseShouldMatchWithoutAdjective()
        {
            NameBase nameBase = new NameBase("Red Bob");
            Assert.IsTrue(nameBase.Matches("Bob"));
        }

        [TestMethod]
        public void NameBaseShouldNotMatchWitIncorrectAdjective()
        {
            NameBase nameBase = new NameBase("Red Bob");
            Assert.IsFalse(nameBase.Matches("Blue Bob"));
        }

        [TestMethod]
        public void NameBaseShouldMatchWithCorrectAdjective()
        {
            NameBase nameBase = new NameBase("Red Bob");
            Assert.IsTrue(nameBase.Matches("Red Bob"));
            Assert.IsTrue(nameBase.Matches("red Bob"));
            Assert.IsTrue(nameBase.Matches("Re Bob"));
            Assert.IsTrue(nameBase.Matches("re Bob"));
            Assert.IsTrue(nameBase.Matches("R Bob"));
            Assert.IsTrue(nameBase.Matches("r Bob"));
            Assert.IsTrue(nameBase.Matches("r b"));
        }

    }
}
