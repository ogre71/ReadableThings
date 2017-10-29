using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Ogresoft.Core.Test
{
    [TestClass]
    public class ThingTests
    {
        [TestMethod]
        public void ThingShouldSerialize()
        {
            var thing = new Thing("thing"); 

            Assert.IsTrue(!string.IsNullOrEmpty(thing.Serialize()));
        }

        [TestMethod]
        public void ThingShouldDeserialize()
        {
            var thing = new Thing("thing");

            string serializedThing = thing.Serialize();

            Assert.IsTrue(!string.IsNullOrEmpty(serializedThing));

            var newThing = JsonConvert.DeserializeObject<Thing>(serializedThing);

            Assert.IsTrue(newThing.Name == "thing"); 
        }

        [TestMethod]
        public void ThingShouldSerializeInventory()
        {
            var thing = new Thing("thing");
            var stuff = new Thing("stuff");
            stuff.Move(thing);
            Assert.IsTrue(thing.ShallowInventory.Contains(stuff));
            var serializedThing = thing.Serialize(); 

            Assert.IsTrue(!string.IsNullOrEmpty(serializedThing));

            var newThing = JsonConvert.DeserializeObject<Thing>(serializedThing);

            Assert.IsTrue(newThing.ShallowInventory.Count == 1);
            Assert.IsTrue(newThing.ShallowInventory.Find(t => t.Name == stuff) != null);
        }
    }
}
